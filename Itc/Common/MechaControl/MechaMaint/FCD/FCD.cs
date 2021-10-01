using Itc.Common.Event;
using Itc.Common.Extensions;
using Itc.Common.TXEnum;
using PLCController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TXSMechaControl.BusyCtrl;
using TXSMechaControl.Common;

namespace MechaMaintCnt.FCD
{
    //using NumUpdateEventHandler = Action<object, NumUpdateEventArgs>;
    //using MessageEventHandler = Action<object, MessageEventArgs>;
    ///// <summary>
    ///// FCD_Provider監視用拡張メソッド
    ///// </summary>
    //public static class PropertyChangedExtensions
    //{
    //    public static IObservable<PropertyChangedEventArgs> ObserveProperty(this INotifyPropertyChanged self, string propertyName)
    //    {
    //        return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
    //            h => (s, e) => h(e),
    //            h => self.PropertyChanged += h,
    //            h => self.PropertyChanged -= h)
    //            .Where(e => e.PropertyName == propertyName);

    //    }
    //    public static IObservable<PropertyChangedEventArgs> ObserveProperty<TProp>(this INotifyPropertyChanged self, Expression<Func<TProp>> propertyName)
    //    {
    //        var name = ((MemberExpression)propertyName.Body).Member.Name;
    //        return self.ObserveProperty(name);
    //    }
    //}
    /// <summary>
    /// FCD関係
    /// </summary>
    public class FCD : IFCD, IDisposable
    {
        /// <summary>
        /// 原点復帰中か？
        /// </summary>
        public bool IsOriginMode { get; private set; }
        /// <summary>
        /// インデックスモードか？
        /// </summary>
        public bool IsIndexMode { get; private set; }
        /// <summary>
        /// マニュアルモードか？
        /// </summary>
        public bool IsManualMode { get; private set; }
        /// <summary>
        /// コマンド変更
        /// </summary>
        public event EventHandler CmdChanged;
        /// <summary>
        /// FCD固定パラメータ
        /// </summary>
        public FCD_Param Param { get; }
        /// <summary>
        /// FCDに関する何かしらが変更された
        /// </summary>
        public event EventHandler FCDChanged;
        /// <summary>
        /// FCDの変更通知
        /// </summary>
        public FCD_Provider FcdProvider { get; }
        /// <summary>
        /// PLC制御I/F
        /// </summary>
        private readonly IPLCMonitor _PLC;
        /// <summary>
        /// キャンセルトークンソース
        /// </summary>
        public CancellationTokenSource Cts { get; private set; } = new CancellationTokenSource();
        /// <summary>
        /// FCDコンストラクタ
        /// </summary>
        public FCD(IPLCMonitor plc)
        {
            _PLC = plc;
            FcdProvider = new FCD_Provider(plc);

            //FCDが変更されたとき
            FcdProvider.ObserveProperty(() => FcdProvider.StsFCD)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //FCD_Homeが変更されたとき
            FcdProvider.ObserveProperty(() => FcdProvider.FCDHome)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });
            //FCD_Originが変更されたとき
            FcdProvider.ObserveProperty(() => FcdProvider.FCDOrigin)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //スピード更新
            FcdProvider.ObserveProperty(() => FcdProvider.StsSpeed)
                    .Subscribe(_ =>
                    {
                        Param.CurrentSpd.SPD = FcdProvider.StsSpeed;
                        FCDChanged?.Invoke(this, new EventArgs());
                    });            

            //リミットに触れたとき
            FcdProvider.ObserveProperty(() => FcdProvider.StsBLimit)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });
            FcdProvider.ObserveProperty(() => FcdProvider.StsFLimit)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //前進時接触センサ若しくは後退時接触センサに触れたとき
            FcdProvider.ObserveProperty(() => FcdProvider.StsFTouch)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });
            FcdProvider.ObserveProperty(() => FcdProvider.StsBTouch)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //FCDのオーバーヒートエラーが発生
            FcdProvider.ObserveProperty(() => FcdProvider.StsFcdDriverHeat)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //ラージテーブル切替
            FcdProvider.ObserveProperty(() => FcdProvider.StsRotLargeTable)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //ビジー
            FcdProvider.ObserveProperty(() => FcdProvider.StsBusy)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });
            //原点復帰
            FcdProvider.ObserveProperty(() => FcdProvider.StsOrigin)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });


            //速度の初期設定が変更されたとき
            FcdProvider.SpdChanged += (s, e) =>
            {
                FCDChanged?.Invoke(this, new EventArgs());
            };


            Param = new FCD_Param();
            if (!Param.SetParam(plc.SYSTEM_MODEL))
            {
                throw new Exception($"{nameof(Param)} is failed");
            }
        }
        /// <summary>
        /// 更新要求
        /// </summary>
        public void UpdateAll()
        {
            FCDChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 原点復帰
        /// </summary>
        public bool Origin(Action<string> errormes, ComProgress com)
        {
            IsOriginMode = true;
            CmdChanged?.Invoke(this, new EventArgs());

            Cts = new CancellationTokenSource();

            com.ctoken = Cts;

            _PLC.SendMessage("FcdOrigin", true, errormes);//実行コマンド

            com.prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.FCD_AXIS} {MechaRes.RESET_Process}" });

            bool res = WaitOrginMethod(errormes, TimeSpan.FromSeconds(120), com.prog, com.ctoken);//120秒待っても完了しなかったらエラー。

            if (res)
            {
                com.prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.FCD_AXIS} {MechaRes.PROCESS_COMPLEAT}" });
            }
            else
            {
                Stop(errormes);
                com.prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.FCD_AXIS} {MechaRes.PROCESS_STOP}" });
            }

            IsOriginMode = false;

            CmdChanged?.Invoke(this, new EventArgs());

            return true;
        }
        /// <summary>
        /// リセット動作の待ちメソッド
        /// </summary>
        /// <param name="mesbox">message</param>
        /// <param name="timeout">timeout</param>
        /// <param name="prog">進捗</param>
        /// <param name="ctoken">キャンセル</param>
        /// <returns></returns>
        private bool WaitOrginMethod(Action<string> mesbox, TimeSpan timeout, IProgress<CalProgress> prog, CancellationTokenSource ctoken)
        {
            var source = new CancellationTokenSource();
            source.CancelAfter(timeout);
            Task t = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (source.IsCancellationRequested)
                    {
                        break;
                    }

                    if (FcdProvider.StsOrigin)
                    {
                        break;
                    }

                    if (ctoken.IsCancellationRequested)
                    {
                        break;
                    }
                    Task.WaitAll(Task.Delay(100));//100msec
                }
            });

            try
            {
                t.Wait(source.Token);
            }
            catch (OperationCanceledException)
            {   //タイムアウト処理
                mesbox?.Invoke($"{MechaRes.TIMEOUT_ERROR} {timeout.TotalSeconds.ToString()}{Environment.NewLine}{MechaRes.CONFIRM_OPERATION}");
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.TIMEOUT_ERROR });
                return false;
            }
            catch (AggregateException)
            {   //例外処理
                mesbox?.Invoke($"{MechaRes.EXCEPTION_ERROR}{Environment.NewLine}{MechaRes.CONFIRM_OPERATION}");
                return false;
            }

            if (ctoken.IsCancellationRequested)
            {
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
                return false;
            }
            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
            return true;
        }
        /// <summary>
        /// Index操作の待ち処理
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="bnum"></param>
        /// <param name="timeout">タイムアウト時間</param>
        /// <returns></returns>
        private bool WaitIndexMethod(Action<string> mesbox, TimeSpan timeout, IProgress<CalProgress> prog, CancellationTokenSource ctoken, float Object, float Total)
        {
            List<bool> busymoni = new List<bool>();
            var source = new CancellationTokenSource();
            source.CancelAfter(timeout);
            Task t = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    int tmpPercent = (int)Math.Min(Math.Abs(FcdProvider.StsFCD - Object) / (float)Total * 100, 100);

                    prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.FCD_AXIS} {MechaRes.INDEX_Process} {tmpPercent.ToString()}%" });

                    if (source.IsCancellationRequested)
                    {
                        break;
                    }

                    if (FcdProvider.StsFCD == Object)
                    {
                        break;
                    }

                    if (busymoni.Count > 20 && !busymoni.Last())
                    {
                        break;
                    }
                    busymoni.Add(FcdProvider.StsBusy);

                    if (busymoni.Count > 20 && !busymoni.Last())
                    {
                        break;
                    }
                    busymoni.Add(FcdProvider.StsBusy);

                    if (ctoken.IsCancellationRequested)
                    {
                        break;
                    }

                    Task.WaitAll(Task.Delay(100));//100msec
                }
            }, source.Token);

            try
            {
                t.Wait(source.Token);
            }
            catch (OperationCanceledException)
            {   //タイムアウト処理
                mesbox?.Invoke($"{MechaRes.TIMEOUT_ERROR} {timeout.TotalSeconds.ToString()}{Environment.NewLine}{MechaRes.TIMEOUT_ERROR}");
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.TIMEOUT_ERROR });
                return false;
            }
            catch (AggregateException)
            {   //例外処理
                mesbox?.Invoke($"{MechaRes.EXCEPTION_ERROR}{Environment.NewLine}{MechaRes.CONFIRM_OPERATION}");
                return false;
            }

            if(ctoken.IsCancellationRequested)
            {
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
                return false;
            }

            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
            return true;
        }
        /// <summary>
        /// マニュアル動作
        /// </summary>
        public bool Manual(MDirMode mode, Action<string> errormes)
        {
            IsManualMode = true;
            switch (mode)
            {
                case (MDirMode.Forward):
                    _PLC.SendMessage("FcdForward", true, errormes);
                    break;
                case (MDirMode.Backward):
                    _PLC.SendMessage("FcdBackward", true, errormes);
                    break;
                default:
                    Stop(errormes);//ストップ
                    break;
            }

            return true;
        }
        /// <summary>
        /// 停止 兎に角とめるスタンス
        /// </summary>
        /// <param name="errormes"></param>
        /// <returns></returns>
        public bool Stop(Action<string> errormes)
        {

            Cts?.Cancel();

            _PLC.SendMessage("FcdBackward", false, errormes);

            _PLC.SendMessage("FcdForward", false, errormes);

            _PLC.SendMessage("FcdIndexStop", true, errormes);

            _PLC.SendMessage("PanelInhibit", false, errormes);

            IsManualMode = false;
            IsIndexMode = false;
            IsOriginMode = false;
            CmdChanged?.Invoke(this, new EventArgs());

            return true;
        }
        /// <summary>
        /// インデックス操作
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="errormes"></param>
        /// <returns></returns>
        public bool Index(float posi, Action<string> errormes, ComProgress com)
        {
            IsIndexMode = true;

            CmdChanged?.Invoke(this, new EventArgs());

            Cts = new CancellationTokenSource();

            com.ctoken = Cts;

            _PLC.SendMessage("FcdIndexPosition", posi, errormes);

            Task.WaitAll(Task.Delay(400));

            _PLC.SendMessage("FcdIndex", true, errormes);

            //総距離
            float length = (float)Math.Abs(FcdProvider.StsFCD - posi);

            bool res =  WaitIndexMethod(errormes, TimeSpan.FromSeconds(60), com.prog, com.ctoken, posi, length);//60秒待っても完了しなかったらエラーとしたい。

            if (res)
            {
                com.prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.FCD_AXIS} {MechaRes.PROCESS_COMPLEAT }" });
            }
            else
            {
                Stop(errormes);
                com.prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.FCD_AXIS} {MechaRes.PROCESS_STOP }" });
            }

            IsIndexMode = false;
            CmdChanged?.Invoke(this, new EventArgs());

            return res;
        }
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name="selspddisp"></param>
        /// <param name="mes"></param>
        public void UpdateSpeed(string selspddisp, Action<string> mes)
        {
            var quer = Param.Speedlist.ToList().Find(p => p.DispName == selspddisp);
            if (quer != null)
            {
                _PLC.SendMessage("FCDspeed", quer.SPD, mes);
            }
            else
            {
                throw new Exception($"{nameof(Param.Speedlist)} is failed");
            }
        }
        /// <summary>
        /// 速度更新(直入力　float)
        /// </summary>
        public bool UpdateSpeed(float selspddisp, Action<string> mes)
        {
            if (selspddisp != FcdProvider.StsSpeed)//スピードの設定値が同じなら抜ける
            {
                var quer = Param.Speedlist.ToList().Find(p => p.SPD == selspddisp);
                if (quer != null)
                {
                    _PLC.SendMessage("FCDspeed", quer.SPD, mes);
                }
                else
                {
                    _PLC.SendMessage("FCDspeed", selspddisp, mes);
                }


                var source = new CancellationTokenSource();
                source.CancelAfter(TimeSpan.FromSeconds(4));
                Task t = Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        if (source.IsCancellationRequested)
                        {
                            break;
                        }

                        if (FcdProvider.StsSpeed == selspddisp)
                        {
                            break;
                        }
                        Task.WaitAll(Task.Delay(100));//100msec
                }
                }, source.Token);


                try
                {
                    t.Wait(source.Token);
                }
                catch (OperationCanceledException)
                {   //タイムアウト処理
                    mes?.Invoke($"{MechaRes.TIMEOUT_ERROR} {TimeSpan.FromSeconds(3).ToString()}{Environment.NewLine}{MechaRes.SPEED_SET_ERROR}");
                    return false;
                }
            }

            return true;

        }
        /// <summary>
        /// 原点位置の更新
        /// </summary>
        public void UpdateOrigin(float origin, Action<string> errormes)
        {
            _PLC.SendMessage("FCDOrigin", origin, errormes);
        }
        /// <summary>
        /// ホーム位置の更新
        /// </summary>
        public void UpdateHome(float home, Action<string> errormes)
        {
            _PLC.SendMessage("FCDHome", home, errormes);
        }
        /// <summary>
        /// X線干渉位置 1の更新
        /// </summary>
        public void UpdateHamamatu1(float hama1, Action<string> errormes)
        {
            _PLC.SendMessage("FCD_Coli1", hama1, errormes);
        }
        /// <summary>
        /// X線干渉位置 2の更新
        /// </summary>
        public void UpdateHamamatu2(float hama2, Action<string> errormes)
        {
            _PLC.SendMessage("FCD_Coli2", hama2, errormes);
        }
        /// <summary>
        /// FCD値を要求
        /// </summary>
        /// <returns></returns>
        public float GetFCD() => FcdProvider.StsFCD;
        /// <summary>
        /// FCDの有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFCDDeip() => _PLC.ConvertScaleToDecip("stsFCD");
        /// <summary>
        /// FCD Home値の取得
        /// </summary>
        /// <returns></returns>
        public float GetFCDHome() => FcdProvider.FCDHome;
        /// <summary>
        /// FCD Homeの有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFCDHomeDeip() => _PLC.ConvertScaleToDecip("FCDHome");
        /// <summary>
        /// FCD原点位置の取得
        /// </summary>
        /// <returns></returns>
        public float GetFCDOrigin() => FcdProvider.FCDOrigin;
        /// <summary>
        /// FCD 原点位置の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFCDOriginDeip() => _PLC.ConvertScaleToDecip("FCDOrigin");
        /// <summary>
        /// FCD 浜松干渉位置１の取得
        /// </summary>
        /// <returns></returns>
        public float GetFCDHama1() => FcdProvider.FCD_Hama1;
        /// <summary>
        /// FCD 浜松干渉位置１の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFCDHama1Deip() => _PLC.ConvertScaleToDecip("FCD_Coli1");
        /// <summary>
        /// FCD 浜松干渉位置2の取得
        /// </summary>
        /// <returns></returns>
        public float GetFCDHama2() => FcdProvider.FCD_Hama2;
        /// <summary>
        /// FCD 浜松干渉位置２の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFCDHama2Deip() => _PLC.ConvertScaleToDecip("FCD_Coli2");
        /// <summary>
        /// 原点位置か？
        /// </summary>
        /// <param name="isbesy"></param>
        public void SetOrg(bool isorg)
        { 
            FcdProvider.StsOrigin = isorg;
        }
        /// <summary>
        /// ビジー状態か
        /// </summary>
        /// <param name="isbesy"></param>
        public void SetBesy(bool isbesy)
        { 
            FcdProvider.StsBusy = isbesy;
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            FcdProvider.Dispose(); 
        }
    }
}
