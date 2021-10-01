using Itc.Common.Event;
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
using TXSMechaControl.Common;
using TXSMechaControl.FCD;


namespace TXSMechaControl.FDD
{
    using NumUpdateEventHandler = Action<object, NumUpdateEventArgs>;
    /// <summary>
    /// FCD_Provider監視用拡張メソッド
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
    public class FDD : IFDD, IDisposable
    {
        /// <summary>
        /// FDD固定パラメータ
        /// </summary>
        public FDD_Param Param { get; }
        /// <summary>
        /// FCDに関する何かしらが変更された
        /// </summary>
        public event EventHandler FDDChanged;
        /// <summary>
        /// FCDの変更通知
        /// </summary>
        public FDD_Provider FddProvider { get; }
        /// <summary>
        /// PLC制御
        /// </summary>
        private readonly IPLCControl _PLC;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FDD(IPLCControl pLC)
        {
            _PLC = pLC;

            FddProvider = new FDD_Provider(pLC);

            //FDDが変更されたとき
            FddProvider.ObserveProperty(() => FddProvider.StsFDD)
                    .Subscribe(_ =>
                    {
                        FDDChanged?.Invoke(this, new EventArgs());
                    });

            //スピード更新
            FddProvider.ObserveProperty(() => FddProvider.StsSpeed)
                    .Subscribe(_ =>
                    {
                        Param.CurrentSpd.SPD = FddProvider.StsSpeed;
                        FDDChanged?.Invoke(this, new EventArgs());
                    });

            //リミットに触れたとき
            FddProvider.ObserveProperty(() => FddProvider.StsBLimit)
                    .Subscribe(_ =>
                    {
                        FDDChanged?.Invoke(this, new EventArgs());
                    });
            FddProvider.ObserveProperty(() => FddProvider.StsFLimit)
                    .Subscribe(_ =>
                    {
                        FDDChanged?.Invoke(this, new EventArgs());
                    });

            //FDDのオーバーヒートエラーが発生
            FddProvider.ObserveProperty(() => FddProvider.StsFddDriverHeat)
                    .Subscribe(_ =>
                    {
                        FDDChanged?.Invoke(this, new EventArgs());
                    });

            //速度の初期設定が変更されたとき
            FddProvider.SpdChanged += (s, e) =>
            {
                FDDChanged?.Invoke(this, new EventArgs());
            };



            Param = new FDD_Param();

            if (!Param.SetParam())
            {
                throw new Exception($"{nameof(Param)} is failed");
            }

        }
        /// <summary>
        /// 原点復帰
        /// </summary>
        public bool Origin(Action<string> errormes, IProgress<CalProgress> prog, CancellationToken ctoken)
        {
            _PLC.SendMessage("PanelInhibit", true, errormes);//タッチパネル操作禁止

            _PLC.SendMessage("FddOrigin", true, errormes);

            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.RESET_Process });

            if (!WaitOrginMethod(errormes, TimeSpan.FromSeconds(60), prog, ctoken))//60秒待っても完了しなかったらエラーとしたい。
            {
                Stop(errormes);//念のためのストップ
                _PLC.SendMessage("PanelInhibit", false, errormes);//タッチパネル操作禁止解除
                return false;
            }

            _PLC.SendMessage("PanelInhibit", false, errormes);//タッチパネル操作禁止解除
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
        private bool WaitOrginMethod(Action<string> mesbox, TimeSpan timeout, IProgress<CalProgress> prog, CancellationToken ctoken)
        {
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

                    //if (FddProvider.StsOrigin)
                    //{
                    //    break;
                    //}
                    if (step == 0 && (FddProvider.StsFLimit || FddProvider.StsBLimit || FddProvider.StsBusy))
                    {
                        step = 1;
                    }
                    else if (step == 1 && FddProvider.StsOrigin)
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
                //Task.WaitAll(Task.Delay(1000));//1秒間表示
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
                //Task.WaitAll(Task.Delay(1000));//1秒間表示
                return false;
            }
            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
            //Task.WaitAll(Task.Delay(1000));//1秒間表示
            return true;
        }
        /// <summary>
        /// マニュアル動作
        /// </summary>
        public bool Manual(MDirMode mode, Action<string> errormes)
        {
            switch (mode)
            {
                case (MDirMode.Forward):
                    _PLC.SendMessage("FddForward", true, errormes);
                    break;
                case (MDirMode.Backward):
                    _PLC.SendMessage("FddBackward", true, errormes);
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
            _PLC.SendMessage("FddForward", false, errormes);

            _PLC.SendMessage("FddBackward", false, errormes);

            _PLC.SendMessage("FddIndexStop", true, errormes);

            _PLC.SendMessage("PanelInhibit", false, errormes);

            return true;
        }
        /// <summary>
        /// インデックス操作
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="errormes"></param>
        /// <returns></returns>
        public bool Index(float posi, Action<string> errormes, IProgress<CalProgress> prog, CancellationToken ctoken)
        {

            //速度は最速にする
            float current = FddProvider.StsSpeed;
            float quermax = Param.Speedlist.ToList().Max(p => p.SPD);

            if (UpdateSpeed(quermax, errormes))
            {
                //_PLC.SendMessage("PanelInhibit", true, errormes);
                _PLC.SendMessage("FddindexPosition", posi, errormes);
                Task.WaitAll(Task.Delay(400));
                _PLC.SendMessage("FddIndex", true, errormes);

                //総距離
                float length = (float)Math.Abs(posi - FddProvider.StsFDD);

                if (!WaitIndexMethod(errormes, TimeSpan.FromSeconds(60), prog, ctoken, posi, length))//60秒待っても完了しなかったらエラーとしたい。
                {
                    //Stop(errormes);//念のためのストップ
                    ////速度を戻す
                    //_PLC.SendMessage("FddSpeed", current, null);
                    UpdateSpeed(current, errormes);

                    return false;
                }

                UpdateSpeed(current, errormes);
                return false;
            }

            //速度を戻す
            _PLC.SendMessage("FddSpeed", current, null);
            _PLC.SendMessage("PanelInhibit", false, errormes);
            return true;

        }

        /// <summary>
        /// Index操作の待ち処理
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="bnum"></param>
        /// <param name="timeout">タイムアウト時間</param>
        /// <returns></returns>
        private bool WaitIndexMethod(Action<string> mesbox, TimeSpan timeout, IProgress<CalProgress> prog, CancellationToken ctoken, float Object, float Total)
        {
            //Task.WaitAll(Task.Delay(100));//100msec命令がONになるまで待つ

            List<bool> busymoni = new List<bool>();
            var source = new CancellationTokenSource();
            source.CancelAfter(timeout);
            Task t = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    int tmpPercent = (int)Math.Min(Math.Abs(FddProvider.StsFDD - Object) / (float)Total * 100, 100);

                    prog.Report(new CalProgress { Percent = 0, Status = MechaRes.INDEX_Process + " " + tmpPercent.ToString() + "%" });


                    if (source.IsCancellationRequested)
                    {
                        break;
                    }

                    if (FddProvider.StsFDD == Object)
                    {
                        break;
                    }
                    if (busymoni.Count > 20 && !busymoni.Last())
                    {
                        break;
                    }
                    busymoni.Add(FddProvider.StsBusy);

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
                Task.WaitAll(Task.Delay(500));
                return false;
            }
            catch (AggregateException)
            {   //例外処理
                mesbox?.Invoke($"{MechaRes.EXCEPTION_ERROR}{Environment.NewLine}{MechaRes.CONFIRM_OPERATION}");
                Task.WaitAll(Task.Delay(500));
                return false;
            }

            if (ctoken.IsCancellationRequested)
            {
                Stop(mesbox);
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
                Task.WaitAll(Task.Delay(500));
                return false;
            }

            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
            Task.WaitAll(Task.Delay(500));
            return true;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public bool Init(Action<string> mes)
        {
            if (_PLC.ConnectPLC(mes))
            {

                return true;
            }
            else
            {
                return false;
            }
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
                _PLC.SendMessage("FddSpeed", quer.SPD, mes);
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
            if (selspddisp != FddProvider.StsSpeed)//スピードの設定値が同じなら抜ける
            {
                var quer = Param.Speedlist.ToList().Find(p => p.SPD == selspddisp);
                if (quer != null)
                {
                    _PLC.SendMessage("FddSpeed", quer.SPD, mes);
                }
                else
                {
                    _PLC.SendMessage("FddSpeed", selspddisp, mes);
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

                        if (FddProvider.StsSpeed == selspddisp)
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
                    mes?.Invoke($"{MechaRes.TIMEOUT_ERROR} {TimeSpan.FromSeconds(3).ToString()}{Environment.NewLine}{MechaRes.TIMEOUT_ERROR}");
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// 更新要求
        /// </summary>
        public void UpdateAll()
        {
            FDDChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 有効桁数
        /// </summary>
        /// <returns></returns>
        //public int GetFDDDeip() => FddProvider.FDD_Decip;
        public int GetFDDDeip() => _PLC.ConvertScaleToDecip("stsFDD");
        /// <summary>
        /// 有効桁数
        /// </summary>
        /// <returns></returns>
        public float GetFDD() => FddProvider.StsFDD;
        /// <summary>
        /// テーブルY 原点位置の取得
        /// </summary>
        /// <returns></returns>
        public float GetFDDOrigin() => FddProvider.FDDOrigin;

        /// <summary>
        /// TblY 原点位置の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        //public int GetFDDOriginDeip() => _PLC.ConvertScaleToDecip("FDDOrigin");
        public int GetFDDOriginDeip() => _PLC.ConvertScaleToDecip("FDDOrigin");

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {

        }

        ///もし必要になったら検討する
        /// <summary>
        /// FDDリニアスケール値
        /// </summary>
        //public float stsLinearFDD { get; set; }
    }
}
