

using Itc.Common.Event;

namespace Dio.DioController
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    //using System.ComponentModel;
    //using System.Linq;
    //using System.Linq.Expressions;
    //using System.Reactive.Linq;
    //using System.Collections.Generic;
    using DioLib;

    using ChkChangeEventHandler = System.Action<object, ChkChangeEventArgs>;
    ///// <summary>
    ///// DIOProvider監視用拡張メソッド
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
    /// Dio制御(TXS用)
    /// </summary>
    public class DioControl : IDioControl, INotifyPropertyChanged,IDisposable
    {
        /// <summary>
        /// 変更通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        ///// <summary>
        ///// DIOの変更情報
        ///// </summary>
        ////public DioProvider DioProvider { get; }
        ///// <summary>
        ///// 非常停止イベント
        ///// </summary>
        //public event EventHandler EmergencyStop;
        ///// <summary>
        ///// PLCからの停止イベント
        ///// </summary>
        //public event EventHandler PLCStop;
        ///// <summary>
        ///// 運転準備イベント
        ///// </summary>
        //public event EventHandler Ready;
        ///// <summary>
        ///// 回転CWイベント
        ///// </summary>
        //public event ChkChangeEventHandler RotCW;
        ///// <summary>
        ///// 回転CCWイベント
        ///// </summary>
        //public event ChkChangeEventHandler RotCCW;
        ///// <summary>
        ///// 微調X軸 CWイベント
        ///// </summary>
        //public event ChkChangeEventHandler FSX_CW;
        ///// <summary>
        ///// 微調X軸 CCWイベント
        ///// </summary>
        //public event ChkChangeEventHandler FSX_CCW;
        ///// <summary>
        ///// 微調Y軸 CWイベント
        ///// </summary>
        //public event ChkChangeEventHandler FSY_CW;
        ///// <summary>
        ///// 微調Y軸 CCWイベント
        ///// </summary>
        //public event ChkChangeEventHandler FSY_CCW;
        /// <summary>
        /// 緊急停止
        /// </summary>
        private bool _emergency;
        /// <summary>
        /// 緊急停止
        /// </summary>
        public bool Emergency
        {
            get => _emergency;
            set
            {
                if (_emergency == value)
                    return;
                _emergency = value;
                RaisePropertyChanged(nameof(Emergency));
            }
        }
        /// <summary>
        /// 運転準備
        /// </summary>
        private bool _ready;
        /// <summary>
        /// 運転準備
        /// </summary>
        public bool Ready
        {
            get => _ready;
            set
            {
                if (_ready == value)
                    return;
                _ready = value;
                RaisePropertyChanged(nameof(Ready));
            }
        }
        /// <summary>
        /// メカ動作停止
        /// </summary>
        private bool _plcstop;
        /// <summary>
        /// PLCによる動作停止
        /// </summary>
        public bool PLCStop
        {
            get => _plcstop;
            set
            {
                if (_plcstop == value)
                    return;
                _plcstop = value;
                RaisePropertyChanged(nameof(PLCStop));
            }
        }
        /// <summary>
        /// 回転CW
        /// </summary>
        private bool _rotationCW;
        /// <summary>
        /// 回転CW
        /// </summary>
        public bool RotationCW
        {
            get => _rotationCW;
            set
            {
                if (_rotationCW == value)
                    return;
                _rotationCW = value;
                RaisePropertyChanged(nameof(RotationCW));
            }
        }
        /// <summary>
        /// 回転CW
        /// </summary>
        private bool _rotationCCW;
        /// <summary>
        /// 回転CW
        /// </summary>
        public bool RotationCCW
        {
            get => _rotationCCW;
            set
            {
                if (_rotationCCW == value)
                    return;
                _rotationCCW = value;
                RaisePropertyChanged(nameof(RotationCCW));
            }
        }
        /// <summary>
        /// 微調X CW
        /// </summary>
        private bool _FSX_CW;
        /// <summary>
        /// 微調X CW
        /// </summary>
        public bool FSX_CW
        {
            get => _FSX_CW;
            set
            {
                if (_FSX_CW == value)
                    return;
                _FSX_CW = value;
                RaisePropertyChanged(nameof(FSX_CW));
            }
        }
        /// <summary>
        /// 微調Y CCW
        /// </summary>
        private bool _FSX_CCW;
        /// <summary>
        /// 微調Y CCW
        /// </summary>
        public bool FSX_CCW
        {
            get => _FSX_CCW;
            set
            {
                if (_FSX_CCW == value)
                    return;
                _FSX_CCW = value;
                RaisePropertyChanged(nameof(FSX_CCW));
            }
        }
        /// <summary>
        /// 微調Y CW
        /// </summary>
        private bool _FSY_CW;
        /// <summary>
        /// 微調Y CW
        /// </summary>
        public bool FSY_CW
        {
            get => _FSY_CW;
            set
            {
                if (_FSY_CW == value)
                    return;
                _FSY_CW = value;
                RaisePropertyChanged(nameof(FSY_CW));
            }
        }
        /// <summary>
        /// 微調Y CCW
        /// </summary>
        private bool _FSY_CCW;
        /// <summary>
        /// 微調Y CCW
        /// </summary>
        public bool FSY_CCW
        {
            get => _FSY_CCW;
            set
            {
                if (_FSY_CCW == value)
                    return;
                _FSY_CCW = value;
                RaisePropertyChanged(nameof(_FSY_CCW));
            }
        }
        /// <summary>
        /// DIテーブル
        /// </summary>
        public IEnumerable<DIOTable> DItable { get; private set; }
        /// <summary>
        /// DOテーブル
        /// </summary>
        public IEnumerable<DIOTable> DOtable { get; private set; }
        /// <summary>
        /// DIOからPCへのインターフェイス
        /// </summary>
        private readonly IDioToPC _DioToPC;
        /// <summary>
        /// PCからDIOへのインターフェイス
        /// </summary>
        private readonly IPCToDio _PCToDio;
        /// <summary>
        /// DIOインターフェイス
        /// </summary>
        private readonly IDio _Dio;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dio"></param>
        public DioControl(IDioToPC diotopc, IPCToDio pctodio, IDio dio)
        {
            _Dio = dio;

            _DioToPC = diotopc;
            _DioToPC.EndLoadDITbl += (s, e) =>
            {
                var dtp = s as DioToPC;
                DItable = dtp.DItable;
            };
            _DioToPC.DioMessage += (s, e) =>
            {
                foreach (var idx in DItable)
                {
                    switch (idx.Device_name)
                    {
                        case "Emergency":
                            Emergency = e.Diodata[idx.Dio_num];
                            break;
                        case "Ready":
                            Ready = e.Diodata[idx.Dio_num];
                            break;
                        case "PLCStop":
                            PLCStop = e.Diodata[idx.Dio_num];
                            break;
                        case "RotationCW":
                            RotationCW = e.Diodata[idx.Dio_num];
                            break;
                        case "RotationCCW":
                            RotationCCW = e.Diodata[idx.Dio_num];
                            break;
                        case "FX_CW":
                            FSX_CW = e.Diodata[idx.Dio_num];
                            break;
                        case "FX_CCW":
                            FSX_CCW = e.Diodata[idx.Dio_num];
                            break;
                        case "FY_CW":
                            FSY_CW = e.Diodata[idx.Dio_num];
                            break;
                        case "FY_CCW":
                            FSY_CCW = e.Diodata[idx.Dio_num];
                            break;
                    }
                }
            };

            _PCToDio = pctodio;
            _PCToDio.EndLoadDOTbl += (s, e) =>
            {
                var pd = s as PCToDio;
                DOtable = pd.DOtable;
            };
        }
        /// <summary>
        /// DIOボードの有効・無効を設定する
        /// </summary>
        /// <param name="path"></param>
        public bool DIOInit()
        {
            
            _Dio.Create(DioBoardType.GPC2000, 0, 32);
            _Dio.Open();

            _DioToPC.Create(_Dio);
            _PCToDio.Create(_Dio);

            if (_PCToDio.Load() && _DioToPC.Load())
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// DIO監視中か?
        /// </summary>
        /// <returns></returns>
        public bool IsDioStart() => _DioToPC.IsDioStart();
        /// <summary>
        /// DIOの監視開始
        /// </summary>
        public void DioStart() => _DioToPC.DioStart();
        /// <summary>
        /// DIOへの通知
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sts"></param>
        public void NotifyCommand(string cmd, bool sts) => _PCToDio.Notify(cmd, sts);
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {

        }
    }

    /// <summary>
    /// DIOテーブル
    /// </summary>
    public class DIOTable
    {
        /// <summary>
        /// DIO番号
        /// </summary>
        public int Dio_num { get; set; }
        /// <summary>
        /// デバイス名
        /// </summary>
        public string Device_name { get; set; }
    }

    public interface IDioControl
    {
        /// <summary>
        /// DIOの初期化(テーブル読込)
        /// </summary>
        /// <param name="path"></param>
        bool DIOInit();
        /// <summary>
        /// DIO監視
        /// </summary>
        void DioStart();
        /// <summary>
        /// DIO監視中か?
        /// </summary>
        /// <returns></returns>
        bool IsDioStart();
        /// <summary>
        /// DIOへの通知
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sts"></param>
        void NotifyCommand(string cmd, bool sts);
    }
}
