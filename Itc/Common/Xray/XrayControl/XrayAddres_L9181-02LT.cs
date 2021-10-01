using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XrayControl
{
    public class XrayAddres_L9181_02LT: IXrayAddres_L9181_02LT
    {

        private readonly string XrayPath = Path.Combine(Directory.GetCurrentDirectory(), "TXS", "XrayTable", "L9181-02LT");

        private readonly string PCtoXrayFile = "PCtoXray.csv";

        private readonly string StaTableFile = "StsTable.csv";

        private readonly string XraytoPCFile = "XraytoPC.csv";
        /// <summary>
        /// 一括コマンド　SAR（状態、X線電圧、X線電流)
        /// </summary>
        private readonly string SARTableFile = "SARTable.csv";
        /// <summary>
        /// 一括コマンド　SVI（設定電圧、設定電流)
        /// </summary>
        private readonly string SVITableFile = "SVITable.csv";
        /// <summary>
        /// 焦点サイズ
        /// </summary>
        private readonly string SCFTableFile = "SFCTable.csv";
        /// <summary>
        /// PCからXrayのコマンド
        /// </summary>
        public IEnumerable<PCtoXray> PCtoXrayCmd { get; private set; }
        /// <summary>
        /// X線の状態及び番号
        /// </summary>
        public IEnumerable<STS> STSlist { get; private set; }
        /// <summary>
        /// XrayからPCのコマンド(頻繁)
        /// </summary>
        public IEnumerable<XraytoPC> XraytoPCCmdFreq { get; private set; }
        /// <summary>
        /// XrayからPCのコマンド(たまに)
        /// </summary>
        public IEnumerable<XraytoPC> XraytoPCCmdIreg { get; private set; }
        /// <summary>
        /// 出力値一括コマンド　SAR（状態、X線電圧、X線電流)
        /// </summary>
        public IEnumerable<SAR> SARs { get; private set; }
        /// <summary>
        /// 設定値 一括コマンド　SVI（設定電圧、設定電流)
        /// </summary>
        public IEnumerable<SVI> SVIs { get; private set; }
        /// <summary>
        /// 焦点サイズ SFC
        /// </summary>
        public IEnumerable<SFC> SFCs { get; private set; }
        /// <summary>
        /// イベント
        /// </summary>
        public event EventHandler EndLoadFiles;
        public XrayAddres_L9181_02LT()
        {
            string readpath = Path.Combine(XrayPath, PCtoXrayFile);
            if (File.Exists(readpath))
            {
                //CSVファイル読込 Tuple
                List<Tuple<string, string>> fileObj 
                    = File.ReadAllLines(readpath, Encoding.GetEncoding("Shift_JIS"))
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[0]) && Regex.IsMatch(j[0], "^[a-zA-Z]+$") && !string.IsNullOrEmpty(j[1]))
                        .Select(c => new Tuple<string, string>(c[0], c[1])).ToList();

                List<PCtoXray> pctoxraylist = new List<PCtoXray>();


                foreach (var loadmes in fileObj)
                {
                    PCtoXray ptx = new PCtoXray()
                    {
                        Param = loadmes.Item1,
                        Cmd = loadmes.Item2,
                    };
                    pctoxraylist.Add(ptx);
                }
                PCtoXrayCmd = pctoxraylist;
            }
            else
            {
                throw new Exception($"{Path.Combine(XrayPath, PCtoXrayFile)}is not exist!");
            }

            readpath = Path.Combine(XrayPath, StaTableFile);
            if (File.Exists(readpath))
            {
                //CSVファイル読込 Tuple
                List<Tuple<string, string>> fileObj = File.ReadAllLines(readpath, Encoding.GetEncoding("Shift_JIS"))
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[0]) && Regex.IsMatch(j[0], "^[a-zA-Z]+$") && !string.IsNullOrEmpty(j[1]))
                        .Select(c => new Tuple<string, string>(c[0], c[1])).ToList();

                List<STS> pctoxraylist = new List<STS>();


                foreach (var loadmes in fileObj)
                {
                    STS ptx = new STS()
                    {
                        Status = loadmes.Item1,
                        StatusNum = loadmes.Item2,
                    };
                    pctoxraylist.Add(ptx);
                }
                STSlist = pctoxraylist;
            }
            else
            {
                throw new Exception($"{Path.Combine(readpath)}is not exist!");
            }

            readpath = Path.Combine(XrayPath, XraytoPCFile);
            if (File.Exists(readpath))
            {
                //CSVファイル読込 Tuple
                List<Tuple<string, string,string>> fileObj = File.ReadAllLines(readpath, Encoding.GetEncoding("Shift_JIS"))
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[0]) && Regex.IsMatch(j[0], "^[a-zA-Z]+$") && !string.IsNullOrEmpty(j[1])&& Regex.IsMatch(j[2], "^(?i)(true|false)$") && !string.IsNullOrEmpty(j[2]))
                        .Select(c => new Tuple<string, string, string>(c[0], c[1], c[2])).ToList();

                List<XraytoPC> pctoxraylistFrq = new List<XraytoPC>();

                List<XraytoPC> pctoxraylistIrg = new List<XraytoPC>();

                foreach (var loadmes in fileObj)
                {
                    if (bool.Parse(loadmes.Item3))
                    {
                        XraytoPC ptx = new XraytoPC()
                        {
                            Param = loadmes.Item1,
                            Cmd = loadmes.Item2,
                        };
                        pctoxraylistFrq.Add(ptx);
                    }
                    else
                    {
                        XraytoPC ptx = new XraytoPC()
                        {
                            Param = loadmes.Item1,
                            Cmd = loadmes.Item2,
                        };
                        pctoxraylistIrg.Add(ptx);
                    }
                }
                XraytoPCCmdFreq = pctoxraylistFrq;

                XraytoPCCmdIreg = pctoxraylistIrg;
            }
            else
            {
                throw new Exception($"{readpath}is not exist!");
            }

            readpath = Path.Combine(XrayPath, SARTableFile);
            if (File.Exists(readpath))
            {
                //SARTableFile CSVファイル読込 Tuple
                List<Tuple<string, string>> fileObj = File.ReadAllLines(readpath, Encoding.GetEncoding("Shift_JIS"))
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[0]) && Regex.IsMatch(j[1], "^[a-zA-Z]+$") && !string.IsNullOrEmpty(j[1]))
                        .Select(c => new Tuple<string, string>(c[0], c[1])).ToList();

                List<SAR> sartable = new List<SAR>();

                foreach (var loadmes in fileObj)
                {
                    var tmpsar = new SAR()
                    {
                        Num = int.Parse(loadmes.Item1),
                        Status = loadmes.Item2,
                    };
                    sartable.Add(tmpsar);
                }
                SARs = sartable;
            }
            else
            {
                throw new Exception($"{readpath}is not exist!");
            }

            readpath = Path.Combine(XrayPath, SVITableFile);
            if (File.Exists(readpath))
            {
                //SARTableFile CSVファイル読込 Tuple
                List<Tuple<string, string>> fileObj = File.ReadAllLines(readpath, Encoding.GetEncoding("Shift_JIS"))
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[0]) && Regex.IsMatch(j[1], "^[a-zA-Z]+$") && !string.IsNullOrEmpty(j[1]))
                        .Select(c => new Tuple<string, string>(c[0], c[1])).ToList();

                List<SVI> sartable = new List<SVI>();

                foreach (var loadmes in fileObj)
                {
                    var tmpsar = new SVI()
                    {
                        Num = int.Parse(loadmes.Item1),
                        Status = loadmes.Item2,
                    };
                    sartable.Add(tmpsar);
                }
                SVIs = sartable;
            }
            else
            {
                throw new Exception($"{readpath}is not exist!");
            }

            readpath = Path.Combine(XrayPath, SCFTableFile);
            if (File.Exists(readpath))
            {
                //SARTableFile CSVファイル読込 Tuple
                List<Tuple<string, string>> fileObj = File.ReadAllLines(readpath, Encoding.GetEncoding("Shift_JIS"))
                        .Where(l => !string.IsNullOrEmpty(l))
                        .Select(v => v.Split(','))
                        .Where(j => !string.IsNullOrEmpty(j[0]) && Regex.IsMatch(j[1], "^[a-zA-Z]+$") && !string.IsNullOrEmpty(j[1]))
                        .Select(c => new Tuple<string, string>(c[0], c[1])).ToList();

                List<SFC> sartable = new List<SFC>();

                foreach (var loadmes in fileObj)
                {
                    var tmpsar = new SFC()
                    {
                        Num = int.Parse(loadmes.Item1),
                        Status = loadmes.Item2,
                    };
                    sartable.Add(tmpsar);
                }
                SFCs = sartable;
            }
            else
            {
                throw new Exception($"{readpath}is not exist!");
            }

            
        }
        /// <summary>
        /// 
        /// </summary>
        public void RequestParam() 
            => EndLoadFiles?.Invoke(this, new EventArgs());

    }

    public interface IXrayAddres_L9181_02LT
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler EndLoadFiles;
        /// <summary>
        /// 
        /// </summary>
        void RequestParam();
    }
    /// <summary>
    /// 
    /// </summary>
    public class PCtoXray
    {
        /// <summary>
        /// パラメータ名
        /// </summary>
        public string Param { get; set; }
        /// <summary>
        /// コマンド名
        /// </summary>
        public string Cmd { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class XraytoPC
    {
        /// <summary>
        /// パラメータ名
        /// </summary>
        public string Param { get; set; }
        /// <summary>
        /// コマンド名
        /// </summary>
        public string Cmd { get; set; }
        /// <summary>
        /// 受信時のバイト数
        /// </summary>
        public int ExpByteNum { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class STS
    {
        /// <summary>
        /// ステータス名
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// ステータス番号
        /// </summary>
        public string StatusNum { get; set; }
    }
    /// <summary>
    /// 出力値一括
    /// </summary>
    public class SAR
    {
        /// <summary>
        /// ステータス名
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 番号
        /// </summary>
        public int Num { get; set; }
    }
    /// <summary>
    /// 設定一括
    /// </summary>
    public class SVI
    {
        /// <summary>
        /// ステータス名
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 番号
        /// </summary>
        public int Num { get; set; }
    }
    /// <summary>
    /// 焦点
    /// </summary>
    public class SFC
    {
        /// <summary>
        /// ステータス名
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 番号
        /// </summary>
        public int Num { get; set; }
    }
}
