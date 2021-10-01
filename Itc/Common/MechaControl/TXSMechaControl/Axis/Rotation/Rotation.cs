namespace TXSMechaControl.Rotation
{
    using Board.BoardControl;
    using Dio.DioController;
    using Itc.Common.TXEnum;
    using PLCController;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Provider監視用拡張メソッド
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
    public class Rotation : IRotation, IDisposable
    {
        /// <summary>
        /// 回転のFixパラメータ
        /// </summary>
        public Rotation_Param Param { get; private set; }
        /// <summary>
        /// 回転の変更通知
        /// </summary>
        public Rot_Provider RotProvider { get; }
        /// <summary>
        /// Rotationに関する何かしらが変更された
        /// </summary>
        public event EventHandler RotChanged;
        /// <summary>
        /// PLCから回転要求があった
        /// </summary>
        public event EventHandler PLCRotRequest;
        /// <summary>
        /// ボードのテーブル回転制御
        /// </summary>
        private readonly IBoardRotation _BoardRot;
        /// <summary>
        /// PLC制御
        /// </summary>
        private readonly IPLCControl _PLC;
        /// <summary>
        /// DIO制御
        /// </summary>
        private readonly IDioControl _DIO;
        /// <summary>
        /// コンストラクタ 
        /// </summary>
        public Rotation(IDioControl dio, IPLCControl plc, IBoardRotation boardRot)
        {
            _BoardRot = boardRot;//ボード回転
            _PLC = plc;//PLC
            _DIO = dio;//DIO

            RotProvider = new Rot_Provider(plc, boardRot);
            //回転の位置が変更されたとき
            RotProvider.ObserveProperty(() => RotProvider.StsRot)
                    .Subscribe(_ =>
                    {

                        _PLC.SendMessage("stsRotPosition", RotProvider.StsRot, null);//角度を通知

                        _DIO.NotifyCommand("RotOrgPosi", RotProvider.StsRot == -2.00F);//原点にいるとき

                        RotChanged?.Invoke(this, new EventArgs());
                    });

            //PLCからIndex位置決要求があった
            RotProvider.ObserveProperty(() => RotProvider.RotIndex)
                    .Subscribe(_ =>
                    {
                        if (RotProvider.RotIndex)
                        {
                            Task.WaitAll(Task.Delay(100));//「RotProvider.StsRotIndexPos」が更新されるのを待つ(100msec)
                            PLCRotRequest?.Invoke(this, new EventArgs());
                        }
                    });

            //ボードのReady状態
            RotProvider.ObserveProperty(() => RotProvider.StsReady)
                    .Subscribe(_ =>
                    {
                        RotChanged?.Invoke(this, new EventArgs());
                    });

            //ボードのAlarm状態
            RotProvider.ObserveProperty(() => RotProvider.StsAlarm)
                    .Subscribe(_ =>
                    {
                        RotChanged?.Invoke(this, new EventArgs());
                    });

            //ボード変更
            _BoardRot.NotifyCount += (s, e) =>
            {
                RotProvider.StsRot = e.NumValue;
            };

            //ボード原点復帰中をDIOに通知
            _BoardRot.NotifyRestOperation += (s, e) =>
            {
                _DIO.NotifyCommand("RotResetFlg", e.Chk);
            };

            //非常停止
            _DIO.EmergencyStop += (s, e) =>
            {
                _BoardRot.Stop(null, StopMode.Fast);
            };

            //PLC動作停止
            _DIO.PLCStop += (s, e) =>
            {
                _BoardRot.Stop(null, StopMode.Fast);
            };

            //CW回転
            _DIO.RotCW += (s, e) =>
            {
                if (e.Chk)
                {
                    _BoardRot.Manual(null, RevMode.CW, RotProvider.StsRotSpeed);
                }
                else
                {
                    _BoardRot.Stop(null, StopMode.Fast);
                }
            };

            //CCW回転
            _DIO.RotCCW += (s, e) =>
            {
                if (e.Chk)
                {
                    _BoardRot.Manual(null, RevMode.CW, RotProvider.StsRotSpeed);
                }
                else
                {
                    _BoardRot.Stop(null, StopMode.Fast);
                }
            };

            Param = new Rotation_Param();
            Param.SetParam();
        }
        /// <summary>
        /// 回転 更新要求
        /// </summary>
        public void UpdateAll()=> RotChanged?.Invoke(this, new EventArgs());       
        /// <summary>
        /// 回転 原点復帰
        /// </summary>
        public bool Origin(Action<string> errormes, IProgress<CalProgress> prog, CancellationTokenSource ctoken)
        {
            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.RESET_Process });

            if (_BoardRot.Origin(errormes, ctoken))
            {
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
                return true;
            }
            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
            return false;
        }
        /// <summary>
        /// 回転　初期化
        /// </summary>
        public bool Init(Action<string> mes)
        {
            if (_PLC.ConnectPLC(mes))
            {
                if (!_DIO.IsDioStart())
                {
                    _DIO.DIOInit(mes);

                    _DIO.DioStart();

                    _BoardRot.Init(mes);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// インデックス操作
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="errormes"></param>
        /// <returns></returns>
        public bool Index(float posi, Action<string> errormes, ComProgress com)
        {
            com.prog.Report(new CalProgress { Percent = 0, Status = MechaRes.INDEX_Process });
            com.Total = (float)Math.Abs(posi - RotProvider.StsRot);
            com.Target = posi;

            if (_BoardRot.Index(errormes, PosiMode.Abso, posi, com))
            {
                //停止表示
                com.prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
                return true;
            }

            //完了報告
            com.prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
            return false;
        }
        /// <summary>
        /// マニュアル動作
        /// </summary>
        public bool Manual(RevMode mode, Action<string> errormes)
        {        
            switch (mode)
            {
                case (RevMode.CW):
                        _BoardRot.Manual(errormes, mode, Param.CurrentSpd.SPD);
                    break;
                case (RevMode.CCW):
                        _BoardRot.Manual(errormes, mode, Param.CurrentSpd.SPD);
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
            return _BoardRot.Stop(errormes, StopMode.Fast);
        }
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name="speadmes"></param>
        /// <param name="errormes"></param>
        public void UpdateSpeed(string selspddisp, Action<string> errormes)
        {
            var quer = Param.Speedlist.ToList().Find(p => p.DispName == selspddisp);
            if (quer != null)
            {
                Param.CurrentSpd = quer;
                Param.ChangedSaveSpd(quer.SPD);
            }
            else
            {
                throw new Exception($"{nameof(Param.Speedlist)} is failed");
            }
            RotChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name="speadmes"></param>
        /// <param name="errormes"></param>
        public void UpdateSpeed(float selspddisp, Action<string> errormes)
        {
            var quer = Param.Speedlist.ToList().Find(p => p.SPD == selspddisp);
            if (quer != null)
            {
                Param.CurrentSpd = quer;
                Param.ChangedSaveSpd(quer.SPD);
            }
            else
            {
                Param.CurrentSpd.SPD = selspddisp;
                Param.ChangedSaveSpd(selspddisp);
            }

            RotChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 回転角度を要求
        /// </summary>
        /// <returns></returns>
        public float GetRot() => RotProvider.StsRot;
        /// <summary>
        /// 回転角度の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetRotDeip() => _BoardRot.GetRotScale();
        /// <summary>
        /// 最大回転角度
        /// </summary>
        /// <returns></returns>
        public float GetRotMax() => _BoardRot.GetRotMax();
        /// <summary>
        /// 最小回転角度
        /// </summary>
        /// <returns></returns>
        public float GetRotMin() => _BoardRot.GetRotMin();
        /// <summary>
        /// 最大回転角度
        /// </summary>
        /// <returns></returns>
        public float GetRotSpeedMax() => _BoardRot.GetRotSpeedMax();
        /// <summary>
        /// 最小回転角度
        /// </summary>
        /// <returns></returns>
        public float GetRotSpeedMin() => _BoardRot.GetRotSpeedMin();
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
