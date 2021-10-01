
using Board.BoardControl;
using Dio.DioController;
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

namespace TXSMechaControl.UpDown
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
    public class UdStage : IUdStage, IDisposable
    {
        /// <summary>
        /// 昇降のFixパラメータ
        /// </summary>
        public Ud_Param Param { get; }
        /// <summary>
        /// 昇降の変更通知
        /// </summary>
        public Ud_Provider Ud_Provider { get; }
        /// <summary>
        /// 昇降に関する何かしらが変更された
        /// </summary>
        public event EventHandler UdChanged;
        /// <summary>
        /// ボードの昇降制御
        /// </summary>
        private readonly IBoardUpDown _BoardUD;
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
        public UdStage(IDioControl dio, IPLCControl plc, IBoardUpDown boardUd)
        {
            _PLC = plc;

            _DIO = dio;

            _BoardUD = boardUd;

            Ud_Provider = new Ud_Provider(plc);

            //昇降位置が変更されたとき
            Ud_Provider.ObserveProperty(() => Ud_Provider.StsUDPosi)
                    .Subscribe(_ =>
                    {
                        UdChanged?.Invoke(this, new EventArgs());
                    });

            //ボード変更
            _BoardUD.NotifyCount += (s, e) =>
            {
                _PLC.SendMessage("stsUDPosition", e.NumValue, null);//位置を通知
                Ud_Provider.StsUDPosi = e.NumValue;
            };


            Param = new Ud_Param();
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

                _BoardUD.Init(mes);

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 回転 原点復帰
        /// </summary>
        public bool Origin(Action<string> errormes, IProgress<CalProgress> prog, CancellationTokenSource ctoken)
        {
            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.RESET_Process });

            if (_BoardUD.Origin(errormes, ctoken))
            {
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
                return true;
            }
            else
            {
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
                return true;
            }
        }
        /// <summary>
        /// マニュアル動作
        /// </summary>
        public bool Manual(RevMode mode, Action<string> errormes) => _BoardUD.Manual(errormes,mode,Param.CurrentSpd.SPD);
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
                Param.CurrentSpd = quer;
                Param.ChangedSaveSpd(quer.SPD);
            }
            else
            {
                throw new Exception($"{nameof(Param.Speedlist)} is failed");
            }

            UdChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public void AlamReset()
        {
            _BoardUD.Reset(null);
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

            UdChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="errormes"></param>
        /// <returns></returns>
        public bool Stop(Action<string> errormes) => _BoardUD.Stop(errormes, StopMode.Slow);
        /// <summary>
        /// インデックス移動
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="errormes"></param>
        /// <param name="prog"></param>
        /// <param name="ctoken"></param>
        /// <returns></returns>
        public bool Index(float posi, Action<string> errormes, ComProgress com)
        {
            com.prog.Report(new CalProgress { Percent = 0, Status = MechaRes.INDEX_Process });
            com.Total = (float)Math.Abs(posi - Ud_Provider.StsUDPosi);
            com.Target = posi;

            if (_BoardUD.Index(errormes, PosiMode.Abso, posi, com))
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
        /// 更新要求
        /// </summary>
        public void UpdateAll() => UdChanged?.Invoke(this, new EventArgs());
        /// <summary>
        /// 昇降位置を要求
        /// </summary>
        /// <returns></returns>
        public float GetUPPosi() => Ud_Provider.StsUDPosi;
        /// <summary>
        /// 昇降テーブルの有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetUDDeip() => _BoardUD.GetUDScale();
        /// <summary>
        /// 昇降位置の最大値
        /// </summary>
        /// <returns></returns>
        public float GetUDMax() => _BoardUD.GetUDMax();
        /// <summary>
        /// 昇降位置の最小値
        /// </summary>
        /// <returns></returns>
        public float GetUDMin() => _BoardUD.GetUDMin();
        /// <summary>
        /// 昇降最高速度
        /// </summary>
        /// <returns></returns>
        public float GetUDSpeedMax()=> _BoardUD.GetUDSpeedMax();
        /// <summary>
        /// 昇降最低速度
        /// </summary>
        /// <returns></returns>
        public float GetUDSpeedMin()=> _BoardUD.GetUDSpeedMin();
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
