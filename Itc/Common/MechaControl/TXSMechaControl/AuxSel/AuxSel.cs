namespace TXSMechaControl.AuxSel
{
    using PLCController;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
    /// <summary>
    /// その他のオプション設定
    /// </summary>
    public class AuxSel : IAuxSel, IDisposable
    {
        /// <summary>
        /// オプション設定が変更された
        /// </summary>
        public event EventHandler AUXChanged;
        /// <summary>
        /// オプション設定
        /// </summary>
        public AuxSel_Provider AuxProvider { get; }
        /// <summary>
        /// PLC制御のI/F
        /// </summary>
        private readonly IPLCControl _iPLC;
        /// <summary>
        /// その他のオプション設定のコンストラクタ
        /// </summary>
        public AuxSel(IPLCControl pLC)
        {
            _iPLC = pLC;
            AuxProvider = new AuxSel_Provider(pLC);

            //扉インターロックが変更されたとき
            AuxProvider.ObserveProperty(() => AuxProvider.DoorInterlock)
                    .Subscribe(_ =>
                    {
                        AUXChanged?.Invoke(this, new EventArgs());
                    });
            //浜松ホト干渉設定が変更されたとき
            AuxProvider.ObserveProperty(() => AuxProvider.XraySourceCollisionPrevent)
                    .Subscribe(_ =>
                    {
                        AUXChanged?.Invoke(this, new EventArgs());
                    });
            //X線接触センサ取付チェック出力
            AuxProvider.ObserveProperty(() => AuxProvider.XraySourceCollisionSensor)
                    .Subscribe(_ =>
                    {
                        AUXChanged?.Invoke(this, new EventArgs());
                    });
        }
        /// <summary>
        /// ドアインターロック変更
        /// </summary>
        /// <param name="chk"></param>
        public void ChangeDoorInterlock(bool chk, Action<string> mes)
            =>_iPLC.SendMessage("DoorInterlock", chk, mes);
        /// <summary>
        /// X線接触センサ取付チェック入力
        /// </summary>
        /// <param name="chk"></param>
        public void ChangeXrayColiChecker(bool chk, Action<string> mes)
            => _iPLC.SendMessage("XraySourceCollisionSensor", chk, mes);
        /// <summary>
        /// 浜ホト干渉防止設定
        /// </summary>
        /// <param name="chk"></param>
        public void ChangeXrayColiPrevent(bool chk, Action<string> mes)
            => _iPLC.SendMessage("XraySourceCollisionPrevent", chk, mes);

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
