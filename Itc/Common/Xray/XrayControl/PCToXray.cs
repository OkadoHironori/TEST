using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XrayControl
{
    /// <summary>
    /// PCからX線への送信
    /// </summary>
    public class PCToXray : IPCToXray,IDisposable
    {
        /// <summary>
        /// X線からPCへの通信I/F
        /// </summary>
        private readonly IXrayToPC _XrayToPC;
        /// <summary>
        /// 
        /// </summary>
        private readonly IXrayAddres_L9181_02LT _XrayAddress;
        /// <summary>
        /// 
        /// </summary>
        private readonly IXraySerialService _XraySerial;
        /// <summary>
        /// PCからXrayのコマンド
        /// </summary>
        public IEnumerable<PCtoXray> PCtoXrayCmd { get; private set; }
        /// <summary>
        /// 焦点サイズ SFC
        /// </summary>
        public IEnumerable<SFC> SFCs { get; private set; }
        /// <summary>
        /// X線の状態及び番号
        /// </summary>
        public IEnumerable<STS> STSlist { get; private set; }
        /// <summary>
        /// STS状態
        /// </summary>
        public STS STSProperty { get; private set; }
        /// <summary>
        /// 出力管電圧
        /// </summary>
        public int TubeVoltage { get; private set; }
        /// <summary>
        /// 出力管電圧
        /// </summary>
        public int TubeCurrent { get; private set; }
        /// <summary>
        /// 出力管電圧
        /// </summary>
        public int SetTubeVoltage { get; private set; }
        /// <summary>
        /// 出力管電圧
        /// </summary>
        public int SetTubeCurrent { get; private set; }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string GUIError { get; private set; }
        /// <summary>
        /// 焦点状態
        /// </summary>
        public string SFC { get; private set; }
        /// <summary>
        /// エラー発生
        /// </summary>
        public event EventHandler GUIErrorEvent;
        /// <summary>
        /// 設定切替完了
        /// </summary>
        public event EventHandler EndChangedSts;
        /// <summary>
        /// WUP完了
        /// </summary>
        public event EventHandler EndWUP;
        /// <summary>
        /// X線ON完了
        /// </summary>
        public event EventHandler EndXrayON;
        /// <summary>
        /// X線OFF完了
        /// </summary>
        public event EventHandler EndXrayOFF;
        /// <summary>
        /// PCからX線への送信
        /// </summary>
        public PCToXray(IXrayAddres_L9181_02LT address, IXraySerialService serial, IXrayToPC xraytopc)
        {
            _XrayAddress = address;
            _XrayAddress.EndLoadFiles += (s, e) => 
            {
                PCtoXrayCmd = (s as XrayAddres_L9181_02LT).PCtoXrayCmd;
                SFCs = (s as XrayAddres_L9181_02LT).SFCs;
                STSlist = (s as XrayAddres_L9181_02LT).STSlist;
            };
            _XrayAddress.RequestParam();

            _XrayToPC = xraytopc;
            _XrayToPC.PropertyChanged += (s, e) => 
            {
                if (e.PropertyName == nameof(STS))
                {
                    STSProperty = STSlist.ToList().Find(p => p.Status == (s as XrayToPC).STS);
                };

                if (e.PropertyName == nameof(TubeCurrent))
                {
                    TubeCurrent = (s as XrayToPC).TubeCurrent;
                };

                if (e.PropertyName == nameof(TubeVoltage))
                {
                    TubeVoltage = (s as XrayToPC).TubeVoltage;
                };

                if (e.PropertyName == nameof(SetTubeCurrent))
                {
                    SetTubeCurrent = (s as XrayToPC).SetTubeCurrent;
                };

                if (e.PropertyName == nameof(SetTubeVoltage))
                {
                    SetTubeVoltage = (s as XrayToPC).SetTubeVoltage;
                };

                if (e.PropertyName == nameof(SFC))
                {
                    SFC = (s as XrayToPC).SFC;
                };

            };
            //X線シリアル通信の開始
            _XraySerial = serial;
            if (STSProperty==null)
            {
                _XraySerial.Start();
                Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
            }            
        }
        /// <summary>
        /// Focusセット
        /// </summary>
        /// <param name="focus"></param>
        public void DoSetFocus(string focus)
        {
            var querFocus = SFCs.ToList().Find(p => p.Status == focus);
            
            if (querFocus != null)
            {
                var quer = PCtoXrayCmd.ToList().Find(p => p.Cmd == "CFS");
                if (quer != null)
                {
                    _XraySerial.DoCommand($"{quer.Cmd} {querFocus.Num}");
                }
                else
                {
                    throw new Exception($"Could not find CFS Commad");
                }
                _XraySerial.DoCommand($"{quer} {querFocus.Num}");
            }
            else
            {
                throw new Exception($"Could not find focus Commad");
            }

            int idx = 0;
            while (true)
            {
                if (SFC == focus)
                {
                    break;
                }

                if (idx == 10)
                {
                    GUIError = "Xray Focus Changed Error";
                    GUIErrorEvent.Invoke(this, new EventArgs());
                    break;
                }
                Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
                idx++;
            }

            EndChangedSts?.Invoke(this, new EventArgs());

        }
        /// <summary>
        /// 管電圧セット
        /// </summary>
        /// <param name="volt"></param>
        public void DoSetVolate(int volt)
        {
            //TODO
            //ここに管電圧の制限処理を記載する

            var quer = PCtoXrayCmd.ToList().Find(p => p.Cmd == "HIV");
            if (quer != null)
            {
                _XraySerial.DoCommand($"{quer.Cmd} {volt}");
            }
            else
            {
                throw new Exception($"Could not find HIV Commad");
            }

            int idx = 0;
            while (true)
            {
                if (SetTubeVoltage == volt)
                {
                    break;
                }

                if (idx == 10)
                {
                    GUIError = "Xray Voltage Changed Error";
                    GUIErrorEvent.Invoke(this, new EventArgs());
                    break;
                }
                Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
                idx++;
            }
            EndChangedSts?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 管電流セット
        /// </summary>
        /// <param name="volt"></param>
        public void DoSetCurrent(int current)
        {
            //TODO
            //ここに管電流の制限処理を記載する

            var quer = PCtoXrayCmd.ToList().Find(p => p.Cmd == "CUR");
            if (quer != null)
            {
                _XraySerial.DoCommand($"{quer.Cmd} {current}");
            }
            else
            {
                throw new Exception($"Could not find CUR Commad");
            }

            int idx = 0;
            while (true)
            {
                if (SetTubeCurrent == current)
                {
                    break;
                }

                if (idx == 10)
                {
                    GUIError = "Xray Current Changed Error";
                    GUIErrorEvent.Invoke(this, new EventArgs());
                    break;
                }
                Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
                idx++;
            }
            EndChangedSts?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// X線ON
        /// </summary>
        /// <param name="volt"></param>
        public void DoXrayOn()
        {
            if (STSProperty.Status == "NeedWup")
            {
                DoWUP();
            }

            var quer = PCtoXrayCmd.ToList().Find(p => p.Cmd == "XON");
            if (quer != null)
            {
                _XraySerial.DoCommand($"{quer.Cmd}");
            }
            else
            {
                throw new Exception($"Could not find XON Commad");
            }

            int idx = 0;
            while (true)
            {
                if (SetTubeCurrent == TubeCurrent&& SetTubeVoltage == TubeVoltage)
                {
                    break;
                }

                if(idx==10)
                {
                    GUIError = "Xray ON Error";
                    GUIErrorEvent.Invoke(this, new EventArgs());
                    break;
                }
                Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
                idx++;
            }
            EndXrayON?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// X線OFF
        /// </summary>
        /// <param name="volt"></param>
        public void DoXrayOFF()
        {
            var quer = PCtoXrayCmd.ToList().Find(p => p.Cmd == "XOF");
            if (quer != null)
            {
                _XraySerial.DoCommand($"{quer.Cmd}");
            }
            else
            {
                throw new Exception($"Could not find XOF Commad");
            }

            int idx = 0;
            while (true)
            {
                if (STSProperty.Status == "XrayOnReady"&& 0 == TubeCurrent && 0 == TubeVoltage)
                {
                    break;
                }

                if (idx == 10)
                {
                    GUIError = "Xray OFF Error";
                    GUIErrorEvent.Invoke(this, new EventArgs());
                    break;
                }
                Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));
                idx++;
            }

            EndXrayOFF?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// ウォームアップ
        /// </summary>
        /// <param name="volt"></param>
        public void DoWUP()
        {
            if (STSProperty.Status != "NeedWup")
            {
                EndWUP?.Invoke(this, new EventArgs());
                return;
            }

            var quer = PCtoXrayCmd.ToList().Find(p => p.Cmd == "WUP");
            if (quer != null)
            {
                _XraySerial.DoCommand($"{quer}");
            }
            else
            {
                throw new Exception($"Could not find WUP Commad");
            }

            while(true)
            {
                Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(1)));

                if (STSProperty.Status != "Wormup")
                {
                    break;
                }
            }

            EndWUP?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            _XraySerial?.Stop();
        }
    }

    public interface IPCToXray
    {
        /// <summary>
        /// Focusセット
        /// </summary>
        /// <param name="focus"></param>
        void DoSetFocus(string focus);
        /// <summary>
        /// 管電圧セット
        /// </summary>
        /// <param name="volt"></param>
        void DoSetVolate(int volt);
        /// <summary>
        /// 管電流セット
        /// </summary>
        /// <param name="volt"></param>
        void DoSetCurrent(int current);
        /// <summary>
        /// X線ON
        /// </summary>
        /// <param name="volt"></param>
        void DoXrayOn();
        /// <summary>
        /// X線OFF
        /// </summary>
        /// <param name="volt"></param>
        void DoXrayOFF();
        /// <summary>
        /// ウォームアップ
        /// </summary>
        /// <param name="volt"></param>
        void DoWUP();
        /// <summary>
        /// WUP完了
        /// </summary>
        event EventHandler EndWUP;
        /// <summary>
        /// エラー発生
        /// </summary>
        event EventHandler GUIErrorEvent;
        /// <summary>
        /// X線ON完了
        /// </summary>
        event EventHandler EndXrayON;
        /// <summary>
        /// X線OFF完了
        /// </summary>
        event EventHandler EndXrayOFF;
        /// <summary>
        /// 設定切替完了
        /// </summary>
        event EventHandler EndChangedSts;
    }
}
