
using Itc.Common.Event;
using Itc.Common.TXEnum;
using PLCController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TXSMechaControl.Common;
using Itc.Common.Extensions;
using TXSMechaControl.TblY;

namespace MechaMaintCnt.TblY
{
    /// <summary>
    /// Provider 監視用拡張メソッド
    /// </summary>
    public static class PropertyChangedExtensions
    {
        public static IObservable<PropertyChangedEventArgs> ObserveProperty(this INotifyPropertyChanged self, string propertyName)
        {
            return Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => (s, e) => h(e),
                h => self.PropertyChanged += h,
                h => self.PropertyChanged -= h)
                .Where(e => e.PropertyName == propertyName);

        }
        public static IObservable<PropertyChangedEventArgs> ObserveProperty<TProp>(this INotifyPropertyChanged self, Expression<Func<TProp>> propertyName)
        {
            var name = ((MemberExpression)propertyName.Body).Member.Name;
            return self.ObserveProperty(name);
        }
    }
    /// <summary>
    /// テーブルY軸のクラス
    /// </summary>
    public class TblY : BindableBase, ITblY, IDisposable
    {
        /// <summary>
        /// 原点復帰中か？
        /// </summary>
        public bool IsOriginMode { get; private set; }
        /// <summary>
        /// マニュアルモードか？
        /// </summary>
        public bool IsManualMode { get; private set; }
        /// <summary>
        /// インデックスモードか？
        /// </summary>
        public bool IsIndexMode { get; private set; }
        /// <summary>
        /// コマンド変更
        /// </summary>
        public event EventHandler CmdChanged;
        /// <summary>
        /// FDD固定値
        /// </summary>
        public TblY_Param Param { get; }
        /// <summary>
        /// FCDの変更通知
        /// </summary>
        public TblY_Provider TblYProvider { get; }
        /// <summary>
        /// TblYに関する何かしらが変更された
        /// </summary>
        public event EventHandler TblYChanged;
        /// <summary>
        /// PLC制御
        /// </summary>
        private readonly IPLCMonitor _PLC;
        /// <summary>
        /// キャンセルトークンソース
        /// </summary>
        public CancellationTokenSource Cts { get; private set; } = new CancellationTokenSource();
        /// <summary>
        /// FCDコンストラクタ
        /// </summary>
        public TblY(IPLCMonitor plc)
        {
            _PLC = plc;

            TblYProvider = new TblY_Provider(plc);

            //TblYが変更されたとき
            TblYProvider.ObserveProperty(() => TblYProvider.StsTblY)
                    .Subscribe(_ =>
                    {
                        TblYChanged?.Invoke(this, new EventArgs());
                    });

            //スピード更新
            TblYProvider.ObserveProperty(() => TblYProvider.StsSpeed)
                    .Subscribe(_ =>
                    {
                        Param.CurrentSpd.SPD = TblYProvider.StsSpeed;
                        TblYChanged?.Invoke(this, new EventArgs());
                    });

            //リミットに触れたとき
            TblYProvider.ObserveProperty(() => TblYProvider.StsLLimit)
                    .Subscribe(_ =>
                    {
                        TblYChanged?.Invoke(this, new EventArgs());
                    });
            TblYProvider.ObserveProperty(() => TblYProvider.StsRLimit)
                    .Subscribe(_ =>
                    {
                        TblYChanged?.Invoke(this, new EventArgs());
                    });

            //ビジー
            TblYProvider.ObserveProperty(() => TblYProvider.StsBusy)
                    .Subscribe(_ =>
                    {
                        TblYChanged?.Invoke(this, new EventArgs());
                    });
            //原点復帰
            TblYProvider.ObserveProperty(() => TblYProvider.StsOrigin)
                    .Subscribe(_ =>
                    {
                        TblYChanged?.Invoke(this, new EventArgs());
                    });


            //浜ホト1
            TblYProvider.ObserveProperty(() => TblYProvider.TblY_Hama1)
                    .Subscribe(_ =>
                    {
                        TblYChanged?.Invoke(this, new EventArgs());
                    });

            //浜ホト2
            TblYProvider.ObserveProperty(() => TblYProvider.TblY_Hama2)
                    .Subscribe(_ =>
                    {
                        TblYChanged?.Invoke(this, new EventArgs());
                    });

            //速度の初期設定が変更されたとき
            TblYProvider.SpdChanged += (s, e) =>
            {
                TblYChanged?.Invoke(this, new EventArgs());
            };


            Param = new TblY_Param();

            if (!Param.SetParam())
            {
                throw new Exception($"{nameof(Param)} is failed");
            }
        }
        /// <summary>
        /// 更新要求
        /// </summary>
        public void UpdateAll()
        {
            TblYChanged?.Invoke(this, new EventArgs());
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

            _PLC.SendMessage("TblYOrigin", true, errormes);

            com.prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.TABLE_Y_AXIS} {MechaRes.RESET_Process}" });

            bool res = WaitOrginMethod(errormes, TimeSpan.FromSeconds(70), com.prog, com.ctoken);//70秒待っても完了しなかったらエラーとしたい。

            if(res)
            {
                com.prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.TABLE_Y_AXIS} {MechaRes.PROCESS_COMPLEAT}" });
            }
            else
            {
                Stop(errormes);
                com.prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.TABLE_Y_AXIS} {MechaRes.PROCESS_STOP}" });
            }

            IsOriginMode = false;

            CmdChanged?.Invoke(this, new EventArgs());

            return res;
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
            List<bool> busymoni = new List<bool>();

            var source = new CancellationTokenSource();
            source.CancelAfter(timeout);
            Task t = Task.Factory.StartNew(() =>
            {
                int step = 0;
                while (true)
                {
                    if (source.IsCancellationRequested)
                    {
                        break;
                    }

                    if (busymoni.Count > 20 && !busymoni.Last())
                    {
                        step = 1;
                    }
                    else if (step == 1 && TblYProvider.StsOrigin)
                    {
                        break;
                    }

                    if (busymoni.Count > 20 && TblYProvider.StsOrigin)
                    {
                        break;
                    }

                    if (ctoken.IsCancellationRequested)
                    {
                        break;
                    }

                    Task.WaitAll(Task.Delay(100));//100msec
                    busymoni.Add(TblYProvider.StsBusy);
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
        /// マニュアル動作
        /// </summary>
        public bool Manual(MDirMode mode, Action<string> errormes)
        {
            IsManualMode = true;
            switch (mode)
            {
                case (MDirMode.Forward):
                    _PLC.SendMessage("TblYRight", true, errormes);
                    break;

                case (MDirMode.Backward):
                    _PLC.SendMessage("TblYLeft", true, errormes);

                    break;
                default:
                    Stop(errormes);//ストップ
                    break;
            }
            return true;
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="errormes"></param>
        /// <returns></returns>
        public bool Stop(Action<string> errormes)
        {
            Cts?.Cancel();

            _PLC.SendMessage("TblYLeft", false, errormes);

            _PLC.SendMessage("TblYRight", false, errormes);

            _PLC.SendMessage("TblYIndexStop", true, errormes);

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

            _PLC.SendMessage("TblYIndexPosition", posi, errormes);

            Task.WaitAll(Task.Delay(400));

            _PLC.SendMessage("TblYIndex", true, errormes);

            //総距離
            float length = (float)Math.Abs(TblYProvider.StsTblY - posi);

            bool res = WaitIndexMethod(errormes, TimeSpan.FromSeconds(60), com.prog, com.ctoken, posi, length);//60秒待っても完了しなかったらエラーとしたい。
            
            if(res)
            {
                com.prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.TABLE_Y_AXIS} {MechaRes.PROCESS_COMPLEAT }" });
            }
            else
            {
                Stop(errormes);
                com.prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.TABLE_Y_AXIS} {MechaRes.PROCESS_STOP }" });
            }

            IsIndexMode = false;
            CmdChanged?.Invoke(this, new EventArgs());

            return res;
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
                    int tmpPercent = (int)Math.Min(Math.Abs(TblYProvider.StsTblY - Object) / (float)Total * 100, 100);

                    prog.Report(new CalProgress { Percent = 0, Status = $"{MechaRes.TABLE_Y_AXIS} {MechaRes.INDEX_Process} {tmpPercent.ToString()}%" });

                    if (source.IsCancellationRequested)
                    {
                        break;
                    }

                    if (TblYProvider.StsTblY == Object)
                    {
                        break;
                    }

                    if (busymoni.Count > 20 && !busymoni.Last())
                    {
                        break;
                    }
                    busymoni.Add(TblYProvider.StsBusy);

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

            if (ctoken.IsCancellationRequested)
            {
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
                return false;
            }

            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });

            return true;
        }
        /// <summary>
        /// 原点位置の更新
        /// </summary>
        public void UpdateOrigin(float origin, Action<string> errormes)
        {
            _PLC.SendMessage("TblYOrigin", origin, errormes);
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
                _PLC.SendMessage("TblYSpeed", quer.SPD, mes);
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
            if (selspddisp != TblYProvider.StsSpeed)//スピードの設定値が同じなら抜ける
            {
                var quer = Param.Speedlist.ToList().Find(p => p.SPD == selspddisp);
                if (quer != null)
                {
                    _PLC.SendMessage("TblYSpeed", quer.SPD, mes);
                }
                else
                {
                    _PLC.SendMessage("TblYSpeed", selspddisp, mes);
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

                        if (TblYProvider.StsSpeed == selspddisp)
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
        /// X線干渉位置 1の更新
        /// </summary>
        public void UpdateHamamatu1(float hama1, Action<string> errormes)
        {
            _PLC.SendMessage("TblY_Coli1", hama1, errormes);
        }
        /// <summary>
        /// X線干渉位置 2の更新
        /// </summary>
        public void UpdateHamamatu2(float hama2, Action<string> errormes)
        {
            _PLC.SendMessage("TblY_Coli2", hama2, errormes);
        }
        /// <summary>
        /// 有効桁数
        /// </summary>
        /// <returns></returns>
        public int GetTblYDeip() => _PLC.ConvertScaleToDecip("stsTblYPosition");
        /// <summary>
        /// テーブルY軸位置
        /// </summary>
        /// <returns></returns>
        public float GetTblY() => TblYProvider.StsTblY;
        /// <summary>
        /// テーブルY 原点位置の取得
        /// </summary>
        /// <returns></returns>
        public float GetTblYOrigin() => TblYProvider.TblYOrigin;
        /// <summary>
        /// FCD 原点位置の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetTblYOriginDeip() => _PLC.ConvertScaleToDecip("TblYOrigin");
        /// <summary>
        /// TblY 浜松干渉位置１の取得
        /// </summary>
        /// <returns></returns>
        public float GetTblYHama1() => TblYProvider.TblY_Hama1;
        /// <summary>
        ///TblY 浜松干渉位置１の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetTblYHama1Deip() => _PLC.ConvertScaleToDecip("TblY_Coli1");
        /// <summary>
        /// TblY 浜松干渉位置2の取得
        /// </summary>
        /// <returns></returns>
        public float GetTblYHama2() => TblYProvider.TblY_Hama2;
        /// <summary>
        /// TblY 浜松干渉位置２の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetTblYHama2Deip() => _PLC.ConvertScaleToDecip("TblY_Coli2");
        /// <summary>
        /// 原点位置か？
        /// </summary>
        /// <param name="isbesy"></param>
        public void SetOrg(bool isorg)
        {
            TblYProvider.StsOrigin = isorg;
        }
        /// <summary>
        /// ビジー状態か？
        /// </summary>
        /// <param name="isbesy"></param>
        public void SetBesy(bool isbesy)
        {
            TblYProvider.StsBusy = isbesy;
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}
