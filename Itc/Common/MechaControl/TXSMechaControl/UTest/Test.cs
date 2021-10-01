namespace TXSMechaControl.UTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using TXSMechaControl.UTest;

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
    public class Somthing_TEST : ISomthing_TEST, IDisposable
    {
        public  List<float> List { get; set; } = new List<float>();
        public  List<float> RList { get; set; } = new List<float>();
        /// <summary>
        /// Dioの定期監視用
        /// </summary>
        public System.Timers.Timer _Timer = null;
        /// <summary>
        /// 回転の変更通知
        /// </summary>
        public Somthing_Provider SomeProvider { get; }
        /// <summary>
        /// Rotationに関する何かしらが変更された
        /// </summary>
        public event EventHandler RotChanged;
        /// <summary>
        /// コンストラクタ 
        /// </summary>
        public Somthing_TEST()
        {

            SomeProvider = new Somthing_Provider();

            //回転の位置が変更されたとき
            SomeProvider.ObserveProperty(() => SomeProvider.StsRot).Buffer(TimeSpan.FromSeconds(2))
                    .Subscribe(_ =>
                    {
                        Debug.WriteLine($"時間{DateTime.Now:HHmmss},{SomeProvider.StsRot.ToString()}");

                        List.Add(SomeProvider.StsRot);
                        RotChanged?.Invoke(this, new EventArgs());
                    });


        }
        public void Start()
        {
            _Timer = new System.Timers.Timer(10);//タイマーインターバル設定

            _Timer.Elapsed += (s, e) =>
            {
                Random r1 = new System.Random();
                float a = r1.Next(1, 300);
                //uC_BindSlider1.SetValue(r1.Next(1, 300));
                RList.Add(a);
                SomeProvider.StsRot = a;
            };

            _Timer.Enabled = true;//タイマー開始

        }
            

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            _Timer.Enabled = false;//タイマー開始

            //throw new NotImplementedException();
        }
    }
}
