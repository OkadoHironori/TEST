using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrayControl
{
    /// <summary>
    /// X線からPCへの通信
    /// </summary>
    public class XrayToPC : IXrayToPC, IDisposable, INotifyPropertyChanged
    {
        /// <summary>
        /// 変更通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// 出力管電流
        /// </summary>
        private int _TubeCurrent;
        /// <summary>
        /// 出力管電流
        /// </summary>
        public int TubeCurrent
        {
            get => _TubeCurrent;
            set
            {
                if (_TubeCurrent == value)
                    return;
                _TubeCurrent = value;
                RaisePropertyChanged(nameof(TubeCurrent));
            }
        }
        /// <summary>
        /// 出力管電圧
        /// </summary>
        private int _TubeVoltage;
        /// <summary>
        /// 出力管電圧
        /// </summary>
        public int TubeVoltage
        {
            get => _TubeVoltage;
            set
            {
                if (_TubeVoltage == value)
                    return;
                _TubeVoltage = value;
                RaisePropertyChanged(nameof(TubeVoltage));
            }
        }
        /// <summary>
        /// X線状態
        /// </summary>
        public string _STS;
        /// <summary>
        /// X線状態
        /// </summary>
        public string STS
        {
            get => _STS;
            set
            {
                if (_STS == value)
                    return;
                _STS = value;
                RaisePropertyChanged(nameof(STS));
            }
        }
        /// <summary>
        /// 設定管電流
        /// </summary>
        private int _SetTubeCurrent;
        /// <summary>
        /// 設定管電流
        /// </summary>
        public int SetTubeCurrent
        {
            get => _SetTubeCurrent;
            set
            {
                if (_SetTubeCurrent == value)
                    return;
                _SetTubeCurrent = value;
                RaisePropertyChanged(nameof(SetTubeCurrent));
            }
        }
        /// <summary>
        /// 設定管電圧
        /// </summary>
        private int _SetTubeVoltage;
        /// <summary>
        /// 設定管電圧
        /// </summary>
        public int SetTubeVoltage
        {
            get => _SetTubeVoltage;
            set
            {
                if (_SetTubeVoltage == value)
                    return;
                _SetTubeVoltage = value;
                RaisePropertyChanged(nameof(SetTubeVoltage));
            }
        }
        /// <summary>
        /// X線源の型名
        /// </summary>
        private string _TYP;
        /// <summary>
        /// X線源の型名
        /// </summary>
        public string TYP
        {
            get => _TYP;
            set
            {
                if (_TYP == value)
                    return;
                _TYP = value;
                RaisePropertyChanged(nameof(TYP));
            }
        }
        /// <summary>
        /// インターロックの状態を返します。 
        /// 「SIN 0」：インターロックが閉じている場合
        /// 「SIN 1」：インターロックが開いている場合の応答
        /// </summary>
        private string _SIN;
        /// <summary>
        /// インターロックの状態を返します。 
        /// 「SIN 0」：インターロックが閉じている場合
        /// 「SIN 1」：インターロックが開いている場合の応答
        /// </summary>
        public string SIN
        {
            get => _SIN;
            set
            {
                if (_SIN == value)
                    return;
                _SIN = value;
                RaisePropertyChanged(nameof(SIN));
            }
        }
        /// <summary>
        /// 焦点状態
        /// </summary>
        private string _SFC;
        /// <summary>
        /// 焦点状態
        /// </summary>
        public string SFC
        {
            get => _SFC;
            set
            {
                if (_SFC == value)
                    return;
                _SFC = value;
                RaisePropertyChanged(nameof(SFC));
            }
        }
        /// <summary>
        /// X線の状態及び番号
        /// </summary>
        public IEnumerable<STS> STSlist { get; private set; }
        /// <summary>
        /// XrayからPCのコマンド(頻繁)
        /// </summary>
        public IEnumerable<XraytoPC> XraytoPCCmd { get; private set; }
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
        /// L9181_02LT 130kV浜松ホト
        /// </summary>
        private readonly IXrayAddres_L9181_02LT _XrayAddress;
        /// <summary>
        /// 
        /// </summary>
        private readonly IXraySerialService _XraySerial;
        /// <summary>
        /// X線からPC
        /// </summary>
        /// <param name="address"></param>
        public XrayToPC(IXrayAddres_L9181_02LT address, IXraySerialService xraySerial)
        {
            _XrayAddress = address;
            _XrayAddress.EndLoadFiles += (s, e) => 
            {
                SARs = (s as XrayAddres_L9181_02LT).SARs;
                SVIs = (s as XrayAddres_L9181_02LT).SVIs;
                SFCs = (s as XrayAddres_L9181_02LT).SFCs;
                STSlist = (s as XrayAddres_L9181_02LT).STSlist;


                List<XraytoPC> tmp = new List<XraytoPC>();
                foreach(var f in (s as XrayAddres_L9181_02LT).XraytoPCCmdFreq)
                {
                    tmp.Add(f);
                }
                foreach (var i in (s as XrayAddres_L9181_02LT).XraytoPCCmdIreg)
                {
                    tmp.Add(i);
                }
                XraytoPCCmd = tmp;
            };
            _XrayAddress.RequestParam();

            _XraySerial = xraySerial;
            _XraySerial.GetSerialParam += (s, e) => 
            {
                XraySerialService xss = s as XraySerialService;
                string tmpmes = Encoding.GetEncoding("Shift_JIS").GetString(xss.Respons).ToString();
                DoAnalysisSerialCmd(tmpmes);
            };
        }
        public void DoAnalysisSerialCmd(string cmd)
        {
            if(cmd.Substring(cmd.Length - 2,2)==$"{Environment.NewLine}")
            {
                cmd = cmd.Substring(0,cmd.Length - 2);
            }

            string cmdtype = cmd.Substring(0, 3);
            XraytoPC quer = XraytoPCCmd.ToList().Find(p => p.Cmd == cmdtype);
            if(quer!=null)
            {
                string[] cmdarry = cmd.Split(' ');

                switch (quer.Cmd)
                {
                    case(nameof(SAR)):

                        foreach (var tmp_sar in SARs)
                        {
                            switch(tmp_sar.Status)
                            {
                                case (nameof(STS)):
                                    var quersts = STSlist.ToList().Find(p => p.StatusNum == cmdarry.ElementAt(tmp_sar.Num));
                                    if (quersts != null)
                                    {
                                        STS = quersts.Status;
                                    }
                                    else
                                    {
                                        throw new Exception($"Couldn't find {cmd} param.");
                                    }
                                    break;
                                case (nameof(TubeVoltage)):
                                    var dd = cmdarry.ElementAt(tmp_sar.Num);

                                    TubeVoltage =int.Parse(cmdarry.ElementAt(tmp_sar.Num));
                                    break;
                                case (nameof(TubeCurrent)):
                                    TubeCurrent = int.Parse(cmdarry.ElementAt(tmp_sar.Num));
                                    break;
                            }
                        }
                        break;
                    case (nameof(SVI)):

                        foreach (var tmp_svi in SVIs)
                        {
                            switch (tmp_svi.Status)
                            {
                                case (nameof(SetTubeVoltage)):
                                    SetTubeVoltage = int.Parse(cmdarry.ElementAt(tmp_svi.Num));
                                    break;
                                case (nameof(SetTubeCurrent)):
                                    SetTubeCurrent = int.Parse(cmdarry.ElementAt(tmp_svi.Num));
                                    break;
                            }
                        }
                        break;
                    case (nameof(TYP)):
                        TYP = cmdarry.ElementAt(1);
                        TYP = TYP.Substring(0, TYP.Length - 2);//「改行」を消す
                        break;
                    case (nameof(SIN)):
                        int tmp =int.Parse(cmdarry.ElementAt(1));
                        if (tmp == 0)
                        {
                            SIN = "InterlockOpen";
                        }
                        else
                        {
                            SIN = "InterlockClose";
                        }
                        break;
                    case (nameof(SFC)):

                        int sfcnum = int.Parse(cmdarry.ElementAt(1));

                        var quersfc = SFCs.ToList().Find(p => p.Num == sfcnum);
                        if (quersfc != null)
                        {
                            SFC = quersfc.Status;
                        }
                        else
                        {
                            throw new Exception($"Couldn't find {nameof(SFC)} param.");
                        }
                        break;
                }
            }
            else
            {
                Debug.WriteLine($"失敗コマンド{cmd}");
                //throw new Exception($"Couldn't find {cmd} param.");
            }
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {

        }
    }

    public interface IXrayToPC
    {
        /// <summary>
        /// シリアルコマンドの解析
        /// </summary>
        /// <param name="cmd"></param>
        void DoAnalysisSerialCmd(string cmd);
        /// <summary>
        /// 変更通知
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;
    }
}
