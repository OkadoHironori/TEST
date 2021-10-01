using Board.BoardControl;
using Dio.DioController;
using Itc.Common.Extensions;
using Itc.Common.TXEnum;
using PLCController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TXSMechaControl.UpDown
{
    public class Ud_Provider : BindableBase, IDisposable
    {
        /// <summary>
        /// 昇降位置
        /// </summary>
        private float _StsUDPosi;
        /// <summary>
        /// 昇降位置
        /// </summary>
        public float StsUDPosi
        {
            get { return this._StsUDPosi; }
            set { SetProperty(ref _StsUDPosi, value); }
        }
        /// <summary>
        /// 運転速度
        /// </summary>
        private float _stsSpeed;
        ///運転速度
        /// </summary>
        public float StsSpeed
        {
            get { return this._stsSpeed; }
            set { SetProperty(ref _stsSpeed, value); }
        }
        /// <summary>
        /// 原点位置
        /// </summary>
        private bool _UdOrigin;
        /// <summary>
        /// 原点位置
        /// </summary>
        public bool UdOrigin
        {
            get { return this._UdOrigin; }
            set { SetProperty(ref _UdOrigin, value); }
        }
        /// <summary>
        /// 位置決め要求
        /// </summary>
        private bool _UdIndex;
        /// <summary>
        /// 位置決め要求
        /// </summary>
        public bool UdIndex
        {
            get { return this._UdIndex; }
            set { SetProperty(ref _UdIndex, value); }
        }
        /// <summary>
        /// PLCからの指定位置動作時の指定位置
        /// </summary>
        public float StsUdIndexPos { get; set; }
        /// <summary>
        /// PLC制御
        /// </summary>
        private readonly IPLCControl _PLC;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pLC"></param>
        /// <param name="diocnt"></param>
        /// <param name="boardud"></param>
        public Ud_Provider(IPLCControl plc)
        {
            _PLC = plc;

            //PLC変更
            _PLC.StatusChanged += (s, e) =>
            {
                var stsname = (s as PLCmodel).Element;

                switch (stsname)
                {
                    case ("stsUDIndexPos"):
                        StsUdIndexPos = (s as PLCmodel).FloatStatus;
                        break;
                    case ("stsUDSpeed"):
                        StsSpeed = (s as PLCmodel).FloatStatus;
                        break;
                    case ("UpDownIndex"):
                        UdIndex = (s as PLCmodel).BoolStatus;
                        break;
                }
            };
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {

        }
    }
}
