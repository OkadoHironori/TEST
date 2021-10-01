using Itc.Common.Extensions;
using Itc.Common.TXEnum;
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

namespace PLCController
{
    /// <summary>
    /// PLC情報の供給クラス
    /// TODO もっときれいになると思う
    /// </summary>
    public class PLCProvider :BindableBase, IPLCProvider, IDisposable
    {
        /// <summary>
        /// PLC変更通知リスト
        /// </summary>
        public ObservableCollection<PLCmodel> PLCModelList { get; private set; }
        /// <summary>
        /// PLCの読込マップ
        /// </summary>
        public IEnumerable<ReadPLCMap> ReadPLCMap { get; private set; }
        /// <summary>
        /// Int
        /// </summary>
        public int IntStatus { get; private set; }
        /// <summary>
        /// Bool
        /// </summary>
        public bool BoolStatus { get; private set; }
        /// <summary>
        /// 浮動小数点
        /// </summary>
        public float FloatStatus { get; private set; }
        /// <summary>
        /// タイプ
        /// </summary>
        public string ElementType { get; private set; }
        /// <summary>
        /// エレメント名
        /// </summary>
        public string ElementName { get; private set; }
        /// <summary>
        /// PLCが変化した
        /// </summary>
        public event EventHandler PLCChanged;
        /// <summary>
        /// PLCProviderのイベントハンドラ
        /// </summary>
        public event Property​Changed​Event​Handler PLCValueChanged;
        /// <summary>
        /// PLCリーダー
        /// </summary>
        private readonly IPLCtoPC _PlCtoPC;
        /// <summary>
        /// PLCアドレス一覧
        /// </summary>
        private readonly IPLCAddress _PLCAddress;
        /// <summary>
        /// PLCサービス
        /// </summary>
        private readonly IPLCServer _PLCService;
        /// <summary>
        /// PLC供給
        /// </summary>
        /// <param name="server"></param>
        public PLCProvider(IPLCtoPC ctoPC, IPLCAddress address, IPLCServer plcservce)
        {
            _PLCService = plcservce;


            _PLCAddress = address;
            _PLCAddress.EndLoadCSV += (s, e) =>
            {
                var pa = s as PLCAddress;
                ReadPLCMap = pa.ReadPLCMap;
            };
            if(ReadPLCMap==null)
            {
                _PLCAddress.RequestParam();
            }




            _PlCtoPC = ctoPC;
            _PlCtoPC.PLCRecived += (s, e) =>
            {
                PLCtoPC plcservice = s as PLCtoPC;
                IEnumerable<bool> bdata = plcservice.PLCBools;
                string[] sdata = plcservice.PLCMes;

                if (Enum.TryParse(plcservice.CmdType, out PLCDataType type))
                {
                    switch (type)
                    {
                        case (PLCDataType.ParamBit):
                        case (PLCDataType.ParamBitFunc):
                            AssignBitMethod(bdata, type);
                            //Task.Run(() => AssignBitMethod(bdata, type)).ConfigureAwait(false);
                            break;
                        case (PLCDataType.ParamFCDWord):
                        case (PLCDataType.ParamFDDWord):
                        case (PLCDataType.ParamTBLYWord):
                        case (PLCDataType.ParamWord):
                        case (PLCDataType.ParamXrayColiWord):
                            AssignWordMethod(sdata, type);

                            //Task.Run(() => AssignWordMethod(sdata, type)).ConfigureAwait(false);
                            break;
                        default:
                            throw new Exception($"{nameof(PLCServer)}Unknow CMD Type");
                    }
                }
                else
                {
                    throw new Exception("Failer");
                }
            };

            Create();


        }
        /// <summary>
        /// Bitデータをそれぞれのコントロールに割り当てる
        /// </summary>
        /// <param name="e"></param>
        private void AssignBitMethod(IEnumerable<bool> data, PLCDataType type)
        {
            var quer = ReadPLCMap.ToList().Find(p => p.PLCDataType == type).PLCContext.ReadTbl.ToList();
            foreach (var qinf in quer)
            {
                foreach (var bitinf in data.Select((v, i) => new { v, i }))
                {
                    if(qinf.Idx == bitinf.i)
                    {
                        if (PLCModelList[qinf.ModelIdx].BoolStatus != bitinf.v)
                        {
                            ElementName = PLCModelList[qinf.ModelIdx].Element;
                            ElementType = nameof(PLCVType.BOOL);
                            BoolStatus = bitinf.v;
                            PLCModelList[qinf.ModelIdx].BoolStatus = bitinf.v;
                            PLCChanged?.Invoke(this, new EventArgs());
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Wordデータをそれぞれのコントロールに割り当てる
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private void AssignWordMethod(string[] data, PLCDataType type)
        {
            var quer = ReadPLCMap.ToList().Find(p => p.PLCDataType == type).PLCContext.ReadTbl.ToList();

            foreach (var qinf in quer)
            {
                foreach (var bitinf in data.Select((v, i) => new { v, i }))
                {
                    if (qinf.Idx == bitinf.i)
                    {
                        int stsvalue = 0;

                        if (qinf.Endian == 2)
                        {
                            string forword = data.ElementAt(qinf.Idx);
                            string backword = data.ElementAt(qinf.Idx + 1);
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
                            if (Regex.IsMatch(data.ElementAt(qinf.Idx), @"^[0-9A-F]{4}"))
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

                                if(PLCModelList[qinf.ModelIdx].IntStatus != stsvalue)
                                {
                                    ElementName = PLCModelList[qinf.ModelIdx].Element;
                                    ElementType = nameof(PLCVType.INT);
                                    IntStatus = stsvalue;
                                    PLCModelList[qinf.ModelIdx].IntStatus = stsvalue;
                                    PLCChanged?.Invoke(this, new EventArgs());
                                }

                                //PLCModelList[qinf.ModelIdx].IntStatus = stsvalue;

                                break;
                            case (PLCVType.FLOAT):

                                if (PLCModelList[qinf.ModelIdx].FloatStatus != (float)Math.Round(((double)stsvalue / (double)qinf.Scale), 3))
                                {
                                    ElementName = PLCModelList[qinf.ModelIdx].Element;
                                    ElementType = nameof(PLCVType.FLOAT);
                                    FloatStatus = (float)Math.Round(((double)stsvalue / (double)qinf.Scale), 3);
                                    PLCModelList[qinf.ModelIdx].FloatStatus = (float)Math.Round(((double)stsvalue / (double)qinf.Scale), 3);
                                    PLCChanged?.Invoke(this, new EventArgs());
                                }

                                //PLCModelList[qinf.ModelIdx].FloatStatus = (float)Math.Round(((double)stsvalue / (double)qinf.Scale), 3);

                                break;
                            default:
                                throw new Exception("Undeveloped Type");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// PLCモデル作成
        /// </summary>
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

                foreach (var element in ReadPLCMap)
                {
                    switch(element.PLCDataType)
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



                                UpdateModelIdx(element.PLCDataType, cont.Idx, PLCModelList.Count);

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

                                        //_PLCAddress.UpdateModelIdx(element.PLCDataType, cont.Idx, PLCModelList.Count);
                                        UpdateModelIdx(element.PLCDataType, cont.Idx, PLCModelList.Count);

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

                                        //_PLCAddress.UpdateModelIdx(element.PLCDataType, cont.Idx, PLCModelList.Count);
                                        UpdateModelIdx(element.PLCDataType, cont.Idx, PLCModelList.Count);

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
        /// 
        /// </summary>
        /// <param name="bytenum"></param>
        public void UpdateModelIdx(PLCDataType type, int orgidx, int modelidx)
        {
            foreach (var cot in ReadPLCMap)
            {
                if (type == cot.PLCDataType)
                {
                    foreach (var aa in cot.PLCContext.ReadTbl)
                    {
                        if (aa.Idx == orgidx)
                        {
                            aa.ModelIdx = modelidx;
                        }
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// UT用
        /// </summary>
        public void Start()
        {
            _PLCService?.Start();
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
           // throw new NotImplementedException();
        }
    }
    /// <summary>
    /// PLCProviderのI/F
    /// </summary>
    public interface IPLCProvider
    {
        /// <summary>
        /// PLCの値変更時のイベント
        /// </summary>
        event Property​Changed​Event​Handler PLCValueChanged;
        /// <summary>
        /// UT用
        /// </summary>
        void Start();
        /// <summary>
        /// PLCが変化した
        /// </summary>
        event EventHandler PLCChanged;

    }
}
