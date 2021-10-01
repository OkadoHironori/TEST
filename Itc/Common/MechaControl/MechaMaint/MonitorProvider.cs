using Itc.Common.Extensions;
using Itc.Common.TXEnum;
using PLCController;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MechaMaintCnt
{
    public class MonitorProvider : BindableBase
    {
        /// <summary>
        /// PLC変更通知リスト
        /// </summary>
        public ObservableCollection<PLCmodel> PLCModelList { get; set; }
        /// <summary>
        /// PLCProviderのイベントハンドラ
        /// </summary>
        public event Property​Changed​Event​Handler PLCValueChanged;
        /// <summary>
        /// イベントハンドラ（最終的にはObservableCollectionを使いたい）
        /// </summary>
        public event PropertyChangedEventHandler ValueChanged;
        /// <summary>
        /// PLCアドレス
        /// </summary>
        private readonly IPLCAddress _PLCAddress;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        public MonitorProvider(IPLCAddress address)
        {
            _PLCAddress = address;
        }
        public void Create()
        {
            if (PLCModelList == null)
            {
                PLCModelList = new ObservableCollection<PLCmodel>();
                PLCModelList.CollectionChanged += (s, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Replace)
                    {
                        foreach (PLCmodel item in e.OldItems)
                            item.PropertyChanged -= PLCValueChanged;
                        foreach (PLCmodel item in e.NewItems)
                            item.PropertyChanged += PLCValueChanged;
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach (PLCmodel item in e.NewItems)
                            item.PropertyChanged += PLCValueChanged;
                    }
                    else if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        foreach (PLCmodel item in e.OldItems)
                            item.PropertyChanged -= PLCValueChanged;
                    }
                };
                foreach (var element in _PLCAddress.ReadPLCMap)
                {
                    switch (element.PLCDataType)
                    {
                        case (PLCDataType.ParamBit):
                        case (PLCDataType.ParamBitFunc):
                            foreach (var cont in element.PLCContext.ReadTbl)
                            {
                                PLCmodel cmodel = new PLCmodel()
                                {
                                    Idx = cont.Idx,
                                    Element = cont.DevName,
                                    BoolStatus = false,
                                    IntStatus = 0,
                                    Scale = 0,
                                    VarType = PLCVType.BOOL,
                                    DataType = element.PLCDataType
                                };

                                _PLCAddress.UpdateModelIdx(element.PLCDataType, cont.Idx, PLCModelList.Count);

                                PLCModelList.Add(cmodel);
                            }



                            break;
                        case (PLCDataType.ParamFCDWord):
                        case (PLCDataType.ParamFDDWord):
                        case (PLCDataType.ParamTBLYWord):
                        case (PLCDataType.ParamWord):
                        case (PLCDataType.ParamXrayColiWord):
                            foreach (var cont in element.PLCContext.ReadTbl)
                            {
                                switch (cont.VType)
                                {
                                    case (PLCVType.FLOAT):
                                        PLCmodel cmodelf = new PLCmodel()
                                        {
                                            Idx = cont.Idx,
                                            Element = cont.DevName,
                                            BoolStatus = false,
                                            IntStatus = 0,
                                            Scale = cont.Scale,
                                            VarType = PLCVType.FLOAT,
                                            DataType = element.PLCDataType
                                        };

                                        _PLCAddress.UpdateModelIdx(element.PLCDataType, cont.Idx, PLCModelList.Count);

                                        PLCModelList.Add(cmodelf);
                                        break;
                                    case (PLCVType.INT):

                                        PLCmodel cmodeli = new PLCmodel()
                                        {
                                            Idx = cont.Idx,
                                            Element = cont.DevName,
                                            BoolStatus = false,
                                            IntStatus = 0,
                                            Scale = 0,
                                            VarType = PLCVType.INT,
                                            DataType = element.PLCDataType
                                        };

                                        _PLCAddress.UpdateModelIdx(element.PLCDataType, cont.Idx, PLCModelList.Count);

                                        PLCModelList.Add(cmodeli);
                                        break;
                                    default:
                                        throw new Exception("undeveloped type");
                                }
                            }
                            break;
                    }
                }
            }
        
        }
        /// <summary>
        /// データセット
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public void SetSerialData(string data, PLCDataType type)
        {
            var quer = _PLCAddress.ReadPLCMap.ToList().Find(p => p.PLCDataType == type).PLCContext.ReadTbl.ToList();

            var startproto = data.Substring(0, 5);
            if ((startproto == "\u000200FF" || startproto == "\u000600FF"))
            {
                if (data != null)
                {
                    string allmes = data.ToString().Substring(5);//最初の5文字を除去
                    string[] text = allmes.StrSplit(4);//拡張メソッドで4文字毎に分割
                    foreach (var qinf in quer)
                    {
                        foreach (var bitinf in data.Select((v, i) => new { v, i }))
                        {
                            if (qinf.Idx == bitinf.i)
                            {
                                int stsvalue = 0;

                                if (qinf.Endian == 2)
                                {
                                    string forword = text.ElementAt(qinf.Idx);
                                    string backword = text.ElementAt(qinf.Idx + 1);
                                    if (Regex.IsMatch(forword, @"^[0-9A-F]{4}") && Regex.IsMatch(backword, @"^[0-9A-F]{4}"))
                                    {
                                        stsvalue = Convert.ToInt32("0x" + backword + forword, 16);
                                    }
                                    else
                                    {
                                        return;//途中で途切れた
                                    }
                                }
                                else
                                {
                                    if (Regex.IsMatch(text.ElementAt(qinf.Idx), @"^[0-9A-F]{4}"))
                                    {
                                        stsvalue = Convert.ToInt32("0x" + data.ElementAt(qinf.Idx), 16);
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }

                                switch (qinf.VType)
                                {
                                    case (PLCVType.INT):

                                        var OldInt = PLCModelList[qinf.ModelIdx].IntStatus;
                                        PLCModelList[qinf.ModelIdx].IntStatus = stsvalue;
                                        if (OldInt != stsvalue)
                                        {
                                            ValueChanged?.Invoke(PLCModelList[qinf.ModelIdx], new PropertyChangedEventArgs(PLCModelList[qinf.ModelIdx].Element));
                                        }
                                        break;
                                    case (PLCVType.FLOAT):

                                        var OldFLOAT = PLCModelList[qinf.ModelIdx].FloatStatus;
                                        var Current = (float)Math.Round(((double)stsvalue / (double)qinf.Scale), 3);
                                        PLCModelList[qinf.ModelIdx].FloatStatus = Current;
                                        if (OldFLOAT != Current)
                                        {
                                            ValueChanged?.Invoke(PLCModelList[qinf.ModelIdx], new PropertyChangedEventArgs(PLCModelList[qinf.ModelIdx].Element));
                                        }


                                        break;
                                    default:
                                        throw new Exception("Undeveloped Type");
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// データセット
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public void SetSerialData(byte[] data, PLCDataType type)
        {

            var quer = _PLCAddress.ReadPLCMap.ToList().Find(p => p.PLCDataType == type).PLCContext.ReadTbl.ToList();

            switch (type)
            {
                case (PLCDataType.ParamBit):

                    List<bool> boollistChar = ConvertBoolListChar(data);
                    if (boollistChar != null)
                    {
                        foreach (var qinf in quer)
                        {
                            foreach (var bitinf in boollistChar.Select((v, i) => new { v, i }))
                            {
                                if (qinf.Idx == bitinf.i)
                                {
                                    bool Old = PLCModelList[qinf.ModelIdx].BoolStatus;

                                    PLCModelList[qinf.ModelIdx].BoolStatus = bitinf.v;

                                    if (Old != bitinf.v)
                                    {
                                        ValueChanged?.Invoke(PLCModelList[qinf.ModelIdx], new PropertyChangedEventArgs(PLCModelList[qinf.ModelIdx].Element));
                                    }
                                }
                            }
                        }
                    }

                    break;

                case (PLCDataType.ParamBitFunc):

                    List<bool> boollist = ConvertBoolList(data);
                    if (boollist != null)
                    {
                        foreach (var qinf in quer)
                        {
                            foreach (var bitinf in boollist.Select((v, i) => new { v, i }))
                            {
                                if (qinf.Idx == bitinf.i)
                                {
                                    bool Old = PLCModelList[qinf.ModelIdx].BoolStatus;
                                    PLCModelList[qinf.ModelIdx].BoolStatus = bitinf.v;

                                    if (Old != bitinf.v)
                                    {
                                        ValueChanged?.Invoke(PLCModelList[qinf.ModelIdx], new PropertyChangedEventArgs(PLCModelList[qinf.ModelIdx].Element));
                                    }
                                }
                            }
                        }
                    }
                    break;
                case (PLCDataType.ParamWord):
                case (PLCDataType.ParamFCDWord):
                case (PLCDataType.ParamFDDWord):
                case (PLCDataType.ParamTBLYWord):
                case (PLCDataType.ParamXrayColiWord):

                    string tmpmes = Encoding.GetEncoding(Encoding.ASCII.EncodingName).GetString(data).ToString();
                    var startproto = tmpmes.Substring(0, 5);
                    if ((startproto == "\u000200FF" || startproto == "\u000600FF"))
                    {
                        if (data != null)
                        {
                            string allmes = tmpmes.ToString().Substring(5);//最初の5文字を除去
                            string[] text = allmes.StrSplit(4);//拡張メソッドで4文字毎に分割
                            foreach (var qinf in quer)
                            {
                                foreach (var bitinf in data.Select((v, i) => new { v, i }))
                                {
                                    if (qinf.Idx == bitinf.i)
                                    {
                                        int stsvalue = 0;

                                        if (qinf.Endian == 2)
                                        {
                                            string forword = text.ElementAt(qinf.Idx);
                                            string backword = text.ElementAt(qinf.Idx + 1);
                                            if (Regex.IsMatch(forword, @"^[0-9A-F]{4}") && Regex.IsMatch(backword, @"^[0-9A-F]{4}"))
                                            {
                                                stsvalue = Convert.ToInt32("0x" + backword + forword, 16);
                                            }
                                            else
                                            {
                                                return;//途中で途切れた
                                            }
                                        }
                                        else
                                        {
                                            if (Regex.IsMatch(text.ElementAt(qinf.Idx), @"^[0-9A-F]{4}"))
                                            {
                                                stsvalue = Convert.ToInt32("0x" + data.ElementAt(qinf.Idx), 16);
                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }

                                        switch (qinf.VType)
                                        {
                                            case (PLCVType.INT):

                                                var OldInt = PLCModelList[qinf.ModelIdx].IntStatus;
                                                PLCModelList[qinf.ModelIdx].IntStatus = stsvalue;
                                                if (OldInt != stsvalue)
                                                {
                                                    ValueChanged?.Invoke(PLCModelList[qinf.ModelIdx], new PropertyChangedEventArgs(PLCModelList[qinf.ModelIdx].Element));
                                                }
                                                break;
                                            case (PLCVType.FLOAT):

                                                var OldFLOAT = PLCModelList[qinf.ModelIdx].FloatStatus;
                                                var Current = (float)Math.Round(((double)stsvalue / (double)qinf.Scale), 3);
                                                PLCModelList[qinf.ModelIdx].FloatStatus = Current;
                                                if (OldFLOAT != Current)
                                                {
                                                    ValueChanged?.Invoke(PLCModelList[qinf.ModelIdx], new PropertyChangedEventArgs(PLCModelList[qinf.ModelIdx].Element));
                                                }


                                                break;
                                            default:
                                                throw new Exception("Undeveloped Type");
                                        }
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        return;
                    }

                    break;
            }


        }
        /// <summary>
        /// boolリスト変換
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<bool> ConvertBoolListChar(byte[] data)
        {
            List<bool> boollist = new List<bool>();

            string allmes = Encoding.GetEncoding(Encoding.ASCII.EncodingName).GetString(data).ToString().Substring(5);//最初の(1文字("F") = 5文字)を除去
            allmes = allmes.ToString().Substring(0, allmes.Length - 3);//終わりの4文字を除去
            if (Regex.IsMatch(allmes, @"^[0-9A-F]{4}"))
            {
                string[] strm = allmes.StrSplit(1);//文字分割
                foreach (var mes in strm)
                {
                    if (int.TryParse(mes, out int res))
                    {
                        bool flg = res == 1 ? true : false;
                        boollist.Add(flg);
                    }
                }

                return boollist;
            }
            else
            {
                return null;
            }
        }
            /// <summary>
            /// boolリストに変換
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            private List<bool> ConvertBoolList(byte[] data)
        {
            if (data.Length < 8) { return null; }

            List<bool> boollist = new List<bool>();//boolリスト
            string allmes = Encoding.GetEncoding(Encoding.ASCII.EncodingName).GetString(data).ToString().Substring(8);//最初の(4文字＋4文字("F000") = 8文字)を除去
            allmes = allmes.ToString().Substring(0, allmes.Length - 4);//終わりの4文字を除去

            if (Regex.IsMatch(allmes, @"^[0-9A-F]{4}"))
            {
                string[] strm = allmes.StrSplit(1);//文字分割

                IEnumerable<int> dataIdx = Enumerable.Range(0, allmes.Length);
                foreach (var ddd in dataIdx)
                {
                    var pp = Convert.ToString(Convert.ToInt32(strm[ddd], 16), 2);//16進数→2進数の変換
                    pp = string.Format("{0:D16}", int.Parse(pp));//0埋め
                    string[] pptrm = pp.StrSplit(1);//分割
                    IEnumerable<int> Idx16 = Enumerable.Range(0, pptrm.Count()).Reverse();//逆カウント
                    foreach (var d in Idx16)
                    {
                        bool flg = int.Parse(pptrm[d]) == 1 ? true : false;
                        boollist.Add(flg);
                    }
                }
            }
            return boollist;
        }
    }
}
