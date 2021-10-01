
using Board.BoardControl;
using Dio.DioController;
using Itc.Common.TXEnum;
using PLCController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TXSMechaControl.FStage
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
    /// 微調クラス
    /// </summary>
    public class FStage : IFStage, IDisposable
    {
        /// <summary>
        /// 微調のFixパラメータ
        /// </summary>
        public FStage_Param Param { get; }
        /// <summary>
        /// 微調の変更通知
        /// </summary>
        public FStage_Provider FStage_Provider { get; }
        /// <summary>
        /// 微調に関する何かしらが変更された
        /// </summary>
        public event EventHandler FStageChanged;
        /// <summary>
        /// ボードの微調X軸制御
        /// </summary>
        private readonly IBoardXStage _BoardFX;
        /// <summary>
        /// ボードの微調X軸制御
        /// </summary>
        private readonly IBoardYStage _BoardFY;
        /// <summary>
        /// PCL制御
        /// </summary>
        private readonly IPLCControl _PLC;
        /// <summary>
        /// PCL制御
        /// </summary>
        private readonly IDioControl _DIO;
        /// <summary>
        /// コンストラクタ 
        /// </summary>
        public FStage(IDioControl dio, IPLCControl plc, IBoardXStage fx, IBoardYStage fy)
        {
            _PLC = plc;

            _DIO = dio;

            _BoardFX = fx;

            _BoardFY = fy;

            FStage_Provider = new FStage_Provider(plc, fx, fy);


            //微調の速度が変更されたとき
            FStage_Provider.ObserveProperty(() => FStage_Provider.StsSpeed)
                    .Subscribe(_ =>
                    {
                        var quer = Param.Speedlist.ToList().Find(p => p.SPD == FStage_Provider.StsSpeed);
                        if (quer != null)
                        {
                            Param.CurrentSpd = quer;
                            FStageChanged?.Invoke(this, new EventArgs());
                        }
                    });

            //微調のX軸が変更された
            FStage_Provider.ObserveProperty(() => FStage_Provider.StsFXPosi)
                    .Subscribe(_ =>
                    {
                        FStageChanged?.Invoke(this, new EventArgs());
                    });
            //微調のX軸が変更された
            FStage_Provider.ObserveProperty(() => FStage_Provider.StsFYPosi)
                    .Subscribe(_ =>
                    {
                        FStageChanged?.Invoke(this, new EventArgs());
                    });

            //微調X軸 タッチパネルからCCW操作
            _DIO.FSX_CCW += (s, e) => _BoardFX.Manual(null, RevMode.CCW, FStage_Provider.StsSpeed);
            //微調X軸 タッチパネルからCW操作
            _DIO.FSX_CW += (s, e) => _BoardFX.Manual(null, RevMode.CW, FStage_Provider.StsSpeed);
            //微調Y軸 タッチパネルからCCW操作
            _DIO.FSY_CCW += (s, e) => _BoardFY.Manual(null, RevMode.CCW, FStage_Provider.StsSpeed);
            //微調Y軸 タッチパネルからCW操作
            _DIO.FSY_CW += (s, e) => _BoardFY.Manual(null, RevMode.CW, FStage_Provider.StsSpeed);


            Param = new FStage_Param();
            Param.SetParam();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="mes"></param>
        public bool Init(Action<string> mes)
        {
            if (_PLC.ConnectPLC(mes))
            {
                if (!_DIO.IsDioStart())
                {
                    _DIO.DIOInit(mes);

                    _DIO.DioStart();
                }

                if (!_BoardFX.Init(mes)) return false;

                if (!_BoardFY.Init(mes)) return false;

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 微調 原点復帰
        /// </summary>
        public bool Origin(Action<string> errormes, IProgress<CalProgress> prog, CancellationTokenSource ctoken)
        {
            try
            {
                _PLC.SendMessage("PanelInhibit", true, errormes);

                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.RESET_Process });


                if (!_BoardFX.Origin(errormes, ctoken))
                {
                    prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
                    return false;

                }
                else
                {
                    prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
                }

                if (!_BoardFY.Origin(errormes, ctoken))
                {
                    prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
                    return false;
                }
                else
                {
                    prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
                }
            }
            finally
            {
                FStageChanged?.Invoke(this, new EventArgs());
                
                _PLC.SendMessage("PanelInhibit", false, errormes);
            }

            return true;
        }
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public void UpdateSpeed(string selspddisp, Action<string> errormes)
        {
            var quer = Param.Speedlist.ToList().Find(p => p.DispName == selspddisp);
            if (quer != null)
            {
                FStage_Provider.StsSpeed = quer.SPD;
            }
            else
            {
                throw new Exception($"{nameof(Param.Speedlist)} is failed");
            }
        }
        /// <summary>
        /// 微調X軸のマニュアル動作
        /// </summary>
        public bool Manual_FX(RevMode mode, Action<string> errormes)
            =>_BoardFX.Manual(errormes, mode, FStage_Provider.StsSpeed);
        /// <summary>
        /// 微調Y軸のマニュアル動作
        /// </summary>
        public bool Manual_FY(RevMode mode, Action<string> errormes)
            =>_BoardFY.Manual(errormes, mode, FStage_Provider.StsSpeed);
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="errormes"></param>
        /// <returns></returns>
        public bool Stop(Action<string> errormes)
        {
            _BoardFX.Stop(errormes, StopMode.Fast);
            _BoardFY.Stop(errormes, StopMode.Fast);

            return true;
        }
        /// <summary>
        /// インデックス移動
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="errormes"></param>
        /// <param name="prog"></param>
        /// <param name="ctoken"></param>
        /// <returns></returns>
        public bool Index_FX(float posi, Action<string> errormes, IProgress<CalProgress> prog, CancellationTokenSource ctoken)
        {
            return _BoardFX.Index(errormes, PosiMode.Abso, posi);
        }
        public bool Index_FY(float posi, Action<string> errormes, IProgress<CalProgress> prog, CancellationTokenSource ctoken)
        {
            return _BoardFY.Index(errormes, PosiMode.Abso, posi);
        }
        /// <summary>
        /// 更新要求
        /// </summary>
        public void UpdateAll() => FStageChanged?.Invoke(this, new EventArgs());
        /// <summary>
        /// 微調X軸位置を要求
        /// </summary>
        /// <returns></returns>
        public float GetFXPosi() => FStage_Provider.StsFXPosi;
        /// <summary>
        /// 微調Y軸位置を要求
        /// </summary>
        /// <returns></returns>
        public float GetFYPosi() => FStage_Provider.StsFYPosi;
        /// <summary>
        /// 微調X軸の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFXDeip() => _BoardFX.GetScale();
        /// <summary>
        /// 微調Y軸の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFYDeip() => _BoardFY.GetScale();
        /// <summary>
        /// 微調X軸の最大値
        /// </summary>
        /// <returns></returns>
        public float GetFXMax() => _BoardFX.GetMax();
        /// <summary>
        /// 微調X軸の最小値
        /// </summary>
        /// <returns></returns>
        public float GetFXMin() => _BoardFX.GetMin();
        /// <summary>
        /// 微調X軸の最大値
        /// </summary>
        /// <returns></returns>
        public float GetFYMax() => _BoardFY.GetMax();
        /// <summary>
        /// 微調X軸の最小値
        /// </summary>
        /// <returns></returns>
        public float GetFYMin() => _BoardFY.GetMin();
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
