using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace PLCController
{
    public class PLCAddress : IPLCAddress, IDisposable
    {
        private readonly string HeaderPath = 
            Path.Combine(Directory.GetCurrentDirectory(),"TXS", "Mecha", "PLCTable");
        /// <summary>
        /// PLCの読込マップ
        /// </summary>
        public IEnumerable<ReadPLCMap> ReadPLCMap { get; private set; }
        /// <summary>
        /// PCからPLCへの書き込みマップ
        /// </summary>
        public PCtoPLCMap PCtoPLCMap { get; private set; }
        /// <summary>
        /// ファイル読込完了
        /// </summary>
        public event EventHandler EndLoadCSV;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PLCAddress(string type)
        {
            var readmap = new List<ReadPLCMap>();

            //string bitwrite = Path.Combine(HeaderPath, type, "FuncSel_PLCtoPC_Bit_B.csv");
            
            //ReadPLCMap map1 = new ReadPLCMap()
            //{
            //    PLCContext = LoadWriteMap(bitwrite),
            //    PLCDataType = PLCDataType.ParamBitFunc,
            //};
            //readmap.Add(map1);

            var bitwrite = Path.Combine(HeaderPath, type, "Param_PLCtoPC_Bit_C.csv");
            ReadPLCMap map2 = new ReadPLCMap()
            {
                PLCContext = LoadWriteMap(bitwrite),
                PLCDataType = PLCDataType.ParamBit,
            };
            readmap.Add(map2);

            bitwrite = Path.Combine(HeaderPath, type, "Param_PLCtoPC_Word.csv");
            ReadPLCMap map3 = new ReadPLCMap()
            {
                PLCContext = LoadWriteMap(bitwrite),
                PLCDataType = PLCDataType.ParamWord,
            };
            readmap.Add(map3);

            if (type != "CT30K")
            {

                bitwrite = Path.Combine(HeaderPath, type, "Colit_PLCtoPC_Word.csv");
                ReadPLCMap map4 = new ReadPLCMap()
                {
                    PLCContext = LoadWriteMap(bitwrite),
                    PLCDataType = PLCDataType.ParamXrayColiWord,
                };

                readmap.Add(map4);

                bitwrite = Path.Combine(HeaderPath, type, "FCD_PLCtoPC_Word.csv");
                ReadPLCMap map5 = new ReadPLCMap()
                {
                    PLCContext = LoadWriteMap(bitwrite),
                    PLCDataType = PLCDataType.ParamFCDWord,
                };

                readmap.Add(map5);

                bitwrite = Path.Combine(HeaderPath, type, "FDD_PLCtoPC_Word.csv");
                ReadPLCMap map6 = new ReadPLCMap()
                {
                    PLCContext = LoadWriteMap(bitwrite),
                    PLCDataType = PLCDataType.ParamFDDWord,
                };

                readmap.Add(map6);

                bitwrite = Path.Combine(HeaderPath, type, "TblY_PLCtoPC_Word.csv");
                ReadPLCMap map7 = new ReadPLCMap()
                {
                    PLCContext = LoadWriteMap(bitwrite),
                    PLCDataType = PLCDataType.ParamTBLYWord,
                };

                readmap.Add(map7);
            }

            ReadPLCMap = readmap;


            bitwrite = Path.Combine(HeaderPath, type, "Param_PCtoPLC_Bit.csv");


            string wordwrite = Path.Combine(HeaderPath, type, "Param_PCtoPLC_Word.csv");

            PCtoPLCMap = new PCtoPLCMap()
            {
                PCtoPLCBitTbl = LoadBitMap(bitwrite),
                PCtoPLCWordTbl = LoadWordPCtoPLCMap(wordwrite),
            };
        }

        /// <summary>
        /// PC->PLCに通知するためのBitデータを取得する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mesbox"></param>
        /// <returns></returns>
        private IEnumerable<PCtoPLCBitTbl> LoadBitMap(string path)
        {
            if (!ChkFileExist(path, null)){ throw new Exception($"{path}is not exist!.{Environment.NewLine}"); }


            //CSVファイル読込 Tuple
            List<Tuple<string, string, string>> FObj = File.ReadAllLines(path)
                    .Where(l => !string.IsNullOrEmpty(l))
                    .Select(v => v.Split(','))
                    .Where(j => !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]))
                    .Select(c => new Tuple<string, string, string>(c[0], c[1], c[2])).ToList();

            List<PCtoPLCBitTbl> tmplist = new List<PCtoPLCBitTbl>();
            foreach (var Obj in FObj)
            {
                if (bool.TryParse(Obj.Item3, out bool res))
                {
                    if (res)
                    {
                        var tmp = new PCtoPLCBitTbl()
                        {
                            DevAddress = Obj.Item1,
                            DevName = Obj.Item2,
                            Command = Convert.ToChar(0x5) + "00" + "FF" + "JW" + "0" + Obj.Item1 + "01",
                        };
                        tmplist.Add(tmp);
                    }
                }
            }
            return tmplist;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytenum"></param>
        public void UpdateExpectByteNum(PLCDataType type , int bytenum)
        {
            foreach(var cot in ReadPLCMap)
            {
                if(type == cot.PLCDataType)
                {
                    cot.PLCContext.ExpectByteNum = bytenum;
                    break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytenum"></param>
        public void UpdateModelIdx(PLCDataType type,int orgidx, int modelidx)
        {
            foreach (var cot in ReadPLCMap)
            {
                if (type == cot.PLCDataType)
                {
                    foreach(var aa in cot.PLCContext.ReadTbl)
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
        /// 機能選択用のBitデータを取得する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mesbox"></param>
        /// <returns></returns>
        private ReadPLCContext LoadWriteMap(string path, Action<string> mesbox = null)
        {
            if (!ChkFileExist(path, mesbox)) { throw new Exception($"{path}is not exist!.{Environment.NewLine}"); }

            List<ReadPLCTbl> tmplist = new List<ReadPLCTbl>();

            string Protcolword = "JR";

            PLCReadOrder order = PLCReadOrder.Non;
            try
            {
                string[] tmppath = Path.GetFileNameWithoutExtension(path).Split('_');
                string tmpchar = tmppath[tmppath.Length - 1];
                switch (tmpchar)
                {
                    case ("B"):
                        order = PLCReadOrder.Bit;
                        tmplist = LoadBitWriteMap(path).ToList();
                        Protcolword = "QR";
                        break;
                    case ("C"):
                        order = PLCReadOrder.Char;
                        tmplist = LoadBitWriteMap(path).ToList();
                        break;
                    case ("Word"):
                        order = PLCReadOrder.Int;
                        tmplist = LoadWordWriteMap(path).ToList();
                        Protcolword = "QR";
                        break;
                    default:
                        throw new Exception($"{path} have invalid value!.{Environment.NewLine}");
                }
            }
            catch(Exception e)
            {
                throw new Exception($"{path} have invalid value!.{Environment.NewLine}{e.Message}");
            }

            IEnumerable<string> addlist = tmplist.Select(f => Convert.ToString(Convert.ToInt32(f.DevAddress.Substring(f.DevAddress.Length - 4), 16), 10));

            if(addlist == null)
            {
                throw new Exception($"{path} have invalid value!.{Environment.NewLine}");
            }

            var miniadd = Convert.ToString(int.Parse(addlist.Min()),16).PadLeft(4, '0').ToUpper();
            var adheader = tmplist.First().DevAddress.Substring(0, 3);

            string command = string.Empty;
            switch (order)
            {
                //case (PLCReadOrder.Bit):
                //    command
                //    break;
                default:
                    command = Convert.ToChar(0x5) + "00" + "FF" + Protcolword + "0" + adheader + miniadd + Convert.ToString((tmplist.Max(p => p.Idx) + 2), 16).PadLeft(2, '0') + "\r\n";//なぜ＋2必要かわからない..
                    break;

            }

            ReadPLCContext context = new ReadPLCContext()
            {
                ReadTbl = tmplist,
                NumberOfData = tmplist.Max(p => p.Idx),
                ReadCommand = command,
                Order = order,

            };

            return context;
        }
        /// <summary>
        /// Bit情報読込テーブルの読込
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<ReadPLCTbl> LoadBitWriteMap(string path)
        {
            //CSVファイル読込 Tuple
            List<Tuple<string, string, string, string>> FObj = File.ReadAllLines(path)
                    .Where(l => !string.IsNullOrEmpty(l))
                    .Select(v => v.Split(','))
                    .Where(j => Regex.IsMatch(j[0], @"^[0-9]+$") && !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]))
                    .Select(c => new Tuple<string, string, string, string>(c[0], c[1], c[2], c[3])).ToList();


            List<ReadPLCTbl> tmplist = new List<ReadPLCTbl>();
            foreach (var Obj in FObj)
            {
                if (bool.TryParse(Obj.Item4, out bool res))
                {
                    if (res)
                    {
                        ReadPLCTbl tbl = new ReadPLCTbl()
                        {
                            Idx = int.Parse(Obj.Item1),
                            DevAddress = Obj.Item2,
                            DevName = Obj.Item3,
                        };
                        tmplist.Add(tbl);
                    }
                }
            }

            return tmplist;
        }
        /// <summary>
        /// FCD制御に関するWordデータを取得する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mesbox"></param>
        /// <returns></returns>
        private IEnumerable<ReadPLCTbl> LoadWordWriteMap(string path)
        {

            //CSVファイル読込 Tuple
            List<Tuple<string, string, string, string, string, string, string>> FObj = File.ReadAllLines(path)
                    .Where(l => !string.IsNullOrEmpty(l))
                    .Select(v => v.Split(','))
                    .Where(j => !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]))
                    .Select(c => new Tuple<string, string, string, string, string, string, string>(c[0], c[1], c[2], c[3], c[4], c[5], c[6])).ToList();

            List<ReadPLCTbl> tmplist = new List<ReadPLCTbl>();


            foreach (var Obj in FObj)
            {
                if (bool.TryParse(Obj.Item7, out bool res))
                {
                    if (res)
                    {
                        if (Enum.TryParse(Obj.Item5.ToUpper(), out PLCVType cType))
                        {
                            var tmp = new ReadPLCTbl()
                            {
                                Idx = int.Parse(Obj.Item1),
                                DevAddress = Obj.Item2,
                                DevName = Obj.Item3,
                                Endian = int.Parse(Obj.Item4),
                                VType = cType,
                                Scale = int.Parse(Obj.Item6),
                            };
                            tmplist.Add(tmp);
                        }
                        else
                        {
                            throw new Exception($"{path} have invalid value!.{Environment.NewLine}");
                        }
                    }
                }

            }
            return tmplist;
        }

        /// <summary>
        /// PC->PLCに通知するためのWordデータを取得する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mesbox"></param>
        /// <returns></returns>
        private IEnumerable<PCtoPLCWordTbl> LoadWordPCtoPLCMap(string path)
        {
            if (!ChkFileExist(path, null)) { throw new Exception($"{path}is not exist!.{Environment.NewLine}"); }

            //CSVファイル読込 Tuple
            List<Tuple<string, string, string, string, string, string>> FObj = File.ReadAllLines(path)
                    .Where(l => !string.IsNullOrEmpty(l))
                    .Select(v => v.Split(','))
                    .Where(j => Regex.IsMatch(j[0], @"^[0-9A-F]{4}") && !string.IsNullOrEmpty(j[1]) && !string.IsNullOrEmpty(j[2]))
                    .Select(c => new Tuple<string, string, string, string, string, string>(c[0], c[1], c[2], c[3], c[4], c[5])).ToList();

            List<PCtoPLCWordTbl> tmplist = new List<PCtoPLCWordTbl>();
            foreach (var Obj in FObj)
            {
                if (bool.TryParse(Obj.Item6, out bool res))
                {
                    if (res)
                    {
                        if (Enum.TryParse(Obj.Item4.ToUpper(), out PLCVType cType))
                        {

                            var tmp = new PCtoPLCWordTbl()
                            {
                                DevAddress = Obj.Item1,
                                DevName = Obj.Item2,
                                Endian = int.Parse(Obj.Item3),
                                VType = cType,
                                Scale = int.Parse(Obj.Item5),
                                Command = Convert.ToChar(0x5) + "00" + "FF" + "QW" + "0" + Obj.Item1 + int.Parse(Obj.Item3).ToString("00"),
                            };
                            tmplist.Add(tmp);
                        }
                        else
                        {
                            throw new Exception($"{path}{Environment.NewLine} may be illegal file.");
                        }
                    }
                }
                else
                {
                    throw new Exception($"{path}{Environment.NewLine}{Obj.Item6}{Environment.NewLine} may be illegal file.");
                }

            }

            return tmplist;
        }
        /// <summary>
        /// ファイルの存在チェック
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="mesbox"></param>
        /// <returns></returns>
        private bool ChkFileExist(string csv, Action<string> mesbox)
        {
            if (!File.Exists(csv))
            {
                mesbox?.Invoke(string.Format("{0} dosn't exists", csv));//ファイルなし
                return false;
            }
            return true;
        }
        /// <summary>
        /// パラメータ要求
        /// </summary>
        public void RequestParam() 
            => EndLoadCSV?.Invoke(this, new EventArgs());
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            //BindBitTbl = null;

        }
    }

    /// <summary>
    /// PLCの読込コマンドのテーブル一覧
    /// </summary>
    public class ReadPLCMap
    {
        /// <summary>
        /// PLCのデータタイプ
        /// </summary>
        public PLCDataType PLCDataType { get; set; }
        /// <summary>
        /// PLCのデータ群
        /// </summary>
        public ReadPLCContext PLCContext { get; set; }
    }
    /// <summary>
    /// 読込コンテキスト
    /// </summary>
    public class ReadPLCContext
    {
        /// <summary>
        /// 読込オーダー
        /// </summary>
        public PLCReadOrder Order { get; set; }
        /// <summary>
        /// 読込用コマンド
        /// </summary>
        public string ReadCommand { get; set; }
        /// <summary>
        /// 最大データ数
        /// </summary>
        public int NumberOfData { get; set; }
        /// <summary>
        /// 当該コマンドを送信後にPLCから返却されるバイトの総数
        /// </summary>
        public int ExpectByteNum { get; set; }
        /// <summary>
        /// 読込テーブル
        /// </summary>
        public IEnumerable<ReadPLCTbl> ReadTbl { get; set; }
    }
    /// <summary>
    /// 読込テーブル
    /// </summary>
    public class ReadPLCTbl
    {
        /// <summary>
        /// 番号
        /// </summary>
        public int Idx { get; set; }
        /// <summary>
        /// デバイスアドレス
        /// </summary>
        public string DevAddress { get; set; }
        /// <summary>
        /// 変数名
        /// </summary>
        public string DevName { get; set; }
        /// <summary>
        /// デバイスのエンディアン
        /// </summary>
        public int Endian { get; set; }
        /// <summary>
        /// PLCのWord変換後の型
        /// </summary>
        public PLCVType VType { get; set; }
        /// <summary>
        /// スケール
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// モデルIdx
        /// </summary>
        public int ModelIdx { get; set; }
    }
    /// <summary>
    /// PLCタイプ
    /// </summary>
    public enum PLCDataType
    {
        /// <summary>
        /// なにもしない
        /// </summary>
        Non,
        /// <summary>
        /// bit
        /// </summary>
        ParamBit,
        /// <summary>
        /// Word
        /// </summary>
        ParamWord,
        /// <summary>
        /// 機能選択
        /// </summary>
        ParamBitFunc,
        /// <summary>
        /// FCD_Word
        /// </summary>
        ParamFCDWord,
        /// <summary>
        /// FDD_Word
        /// </summary>
        ParamFDDWord,
        /// <summary>
        /// TabY_Word
        /// </summary>
        ParamTBLYWord,
        /// <summary>
        /// 浜ホト干渉_Word
        /// </summary>
        ParamXrayColiWord,
    }
    /// <summary>
    /// PLCをC#側で利用する型
    /// </summary>
    public enum PLCVType
    {
        /// <summary>
        /// 未定義
        /// </summary>
        NON,
        /// <summary>
        /// int型
        /// </summary>
        INT,
        /// <summary>
        /// Float型
        /// </summary>
        FLOAT,
        /// <summary>
        /// Bool型
        /// </summary>
        BOOL,
    }

    /// <summary>
    /// PLCの読み込みオーダー
    /// </summary>
    public enum PLCReadOrder
    {
        /// <summary>
        /// 未定義
        /// </summary>
        Non,
        /// <summary>
        /// Char型
        /// </summary>
        Char,
        /// <summary>
        /// 16bit
        /// </summary>
        Bit,
        /// <summary>
        /// Int数値
        /// </summary>
        Int,
    }
    /// <summary>
    /// PCからPLCへ送信するコマンドマップ
    /// </summary>
    public class PCtoPLCMap
    {
        /// <summary>
        /// PC->PLC用Wordテーブル
        /// </summary>
        public IEnumerable<PCtoPLCWordTbl> PCtoPLCWordTbl { get; set; }
        /// <summary>
        /// PLC->PC用Bitテーブル
        /// </summary>
        public IEnumerable<PCtoPLCBitTbl> PCtoPLCBitTbl { get; set; }
    }

    /// <summary>
    /// PLCのBitテーブル
    /// </summary>
    public class PCtoPLCBitTbl
    {
        /// <summary>
        /// デバイス名
        /// </summary>
        public string DevName { get; set; }
        /// <summary>
        /// デバイスアドレス
        /// </summary>
        public string DevAddress { get; set; }
        /// <summary>
        /// コマンド
        /// 最終的に
        /// 書込みﾋﾞｯﾄﾃﾞｰﾀ
        /// BitData = Data? "1" : "0";
        /// 終端文字
        /// "\r\n"
        /// を追加する
        /// </summary>
        public string Command { get; set; }
    }

    /// <summary>
    /// PLCのBitテーブル
    /// </summary>
    public class PCtoPLCWordTbl
    {
        /// <summary>
        /// デバイス名
        /// </summary>
        public string DevName { get; set; }
        /// <summary>
        /// デバイスアドレス
        /// </summary>
        public string DevAddress { get; set; }
        /// <summary>
        /// デバイスのエンディアン
        /// </summary>
        public int Endian { get; set; }
        /// <summary>
        /// PLCのWord変換後の型
        /// </summary>
        public PLCVType VType { get; set; }
        /// <summary>
        /// スケール
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// コマンド
        /// 最終的に
        /// 書込みWordﾃﾞｰﾀ
        /// 終端文字
        /// "\r\n"
        /// を追加する
        /// </summary>
        public string Command { get; set; }
    }
    /// <summary>
    /// PLCAddressクラスI/F
    /// </summary>
    public interface IPLCAddress
    {
        ///// <summary>
        ///// PLCの読込マップ
        ///// </summary>
        //IEnumerable<ReadPLCMap> ReadPLCMap { get; }
        ///// <summary>
        ///// PCtoPLCのマップ
        ///// </summary>
        //PCtoPLCMap PCtoPLCMap { get; }
        /// <summary>
        /// PLCアドレスマップの生成
        /// </summary>
        /// <param name="mesbox"></param>
        /// <returns></returns>
        void UpdateExpectByteNum(PLCDataType type, int bytenum);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="orgidx"></param>
        /// <param name="modelidx"></param>
        //void UpdateModelIdx(PLCDataType type, int orgidx, int modelidx);
        /// <summary>
        /// ファイル読込完了
        /// </summary>
        event EventHandler EndLoadCSV;
        /// <summary>
        /// パラメータ要求
        /// </summary>
        void RequestParam();
    }
}
