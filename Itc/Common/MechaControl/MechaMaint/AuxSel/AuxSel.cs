using Itc.Common.Event;
using PLCController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
//using System.Reactive.Linq;
//using System.Text;
//using System.Threading.Tasks;
namespace MechaMaintCnt.AuxSel
{
    using MessageEventHandler = Action<object, MessageEventArgs>;

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
    /// その他のオプション設定
    /// </summary>
    public class AuxSel : IAuxSel, IDisposable
    {
        /// <summary>
        /// PLCからの要求
        /// </summary>
        public event EventHandler AUXChanged;
        /// <summary>
        /// オプション設定
        /// </summary>
        public AuxSel_Provider AuxProvider { get; }
        /// <summary>
        /// PLC制御のI/F
        /// </summary>
        private readonly IPLCMonitor _PLC;
        /// <summary>
        /// その他のオプション設定のコンストラクタ
        /// </summary>
        public AuxSel(IPLCMonitor monitor)
        {
            _PLC = monitor;

            AuxProvider = new AuxSel_Provider(monitor);

            ////扉インターロックが変更されたとき
            //AuxProvider.ObserveProperty(() => AuxProvider.DoorInterlock)
            //        .Subscribe(_ =>
            //        {
            //            AUXChanged?.Invoke(this, new EventArgs());
            //        });

            ////X線接触センサ取付チェック出力
            //AuxProvider.ObserveProperty(() => AuxProvider.DoorEMLock)
            //        .Subscribe(_ =>
            //        {
            //            AUXChanged?.Invoke(this, new EventArgs());
            //        });


            ////浜松ホト干渉設定が変更されたとき
            //AuxProvider.ObserveProperty(() => AuxProvider.XraySourceCollisionPrevent)
            //        .Subscribe(_ =>
            //        {
            //            AUXChanged?.Invoke(this, new EventArgs());
            //        });
            ////X線接触センサ取付チェック出力
            //AuxProvider.ObserveProperty(() => AuxProvider.XraySourceCollisionSensor)
            //        .Subscribe(_ =>
            //        {
            //            AUXChanged?.Invoke(this, new EventArgs());
            //        });
        }
        /// <summary>
        /// 電磁ロック
        /// </summary>
        /// <param name="emlock"></param>
        public void SetEleMagLock(bool emlock)
        {
            AuxProvider.DoorEMLock = emlock;
        }
        /// <summary>
        /// ドアインターロック変更
        /// </summary>
        /// <param name="chk"></param>
        public void ChangeDoorInterlock(bool chk, Action<string> mes)
            => _PLC.SendMessage("DoorInterlock", chk, mes);
        /// <summary>
        /// X線接触センサ取付チェック入力
        /// </summary>
        /// <param name="chk"></param>
        public void ChangeXrayColiChecker(bool chk, Action<string> mes)
            => _PLC.SendMessage("XraySourceCollisionSensor", chk, mes);
        /// <summary>
        /// 浜ホト干渉防止設定
        /// </summary>
        /// <param name="chk"></param>
        public void ChangeXrayColiPrevent(bool chk, Action<string> mes)
            => _PLC.SendMessage("XraySourceCollisionPrevent", chk, mes);
        /// <summary>
        /// 更新
        /// </summary>
        public void UpdateAll()
            => AUXChanged?.Invoke(this, new EventArgs());

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
