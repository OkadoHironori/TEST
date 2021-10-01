

namespace TXSMechaControl.Rotation
{
    using Board.BoardControl;
    using Dio.DioController;
    using Itc.Common.Extensions;
    using Itc.Common.TXEnum;
    using PLCController;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TXSMechaControl.Common;

    /// <summary>
    /// PLCからの指令
    /// </summary>
    public class Rot_Provider : BindableBase, IDisposable
    {
        /// <summary>
        /// PLCからの指定位置動作時の指定位置
        /// </summary>
        public float StsRotIndexPos { get; set; }
        /// <summary>
        /// 回転スピード
        /// </summary>
        private float _StsRotSpeed;
        /// <summary>
        /// 回転スピード
        /// </summary>
        public float StsRotSpeed
        {
            get { return this._StsRotSpeed; }
            set { SetProperty(ref _StsRotSpeed, value); }
        }
        /// <summary>
        /// 回転位置決め要求
        /// </summary>
        private bool _RotIndex;
        /// <summary>
        /// 回転位置決め要求
        /// </summary>
        public bool RotIndex
        {
            get { return this._RotIndex; }
            set { SetProperty(ref _RotIndex, value); }
        }
        /// <summary>
        /// 回転位置
        /// </summary>
        private float _StsRot;
        /// <summary>
        /// 回転位置
        /// </summary>
        public float StsRot
        {
            get { return this._StsRot; }
            set { SetProperty(ref _StsRot, value); }
        }
        /// <summary>
        /// サーボアラーム
        /// </summary>
        private bool _StsAlarm;
        /// <summary>
        /// サーボアラーム
        /// </summary>
        public bool StsAlarm
        {
            get { return this._StsAlarm; }
            set { SetProperty(ref _StsAlarm, value); }
        }
        /// <summary>
        /// 準備
        /// </summary>
        private bool _StsReady;
        /// <summary>
        /// 準備
        /// </summary>
        public bool StsReady
        {
            get { return this._StsReady; }
            set { SetProperty(ref _StsReady, value); }
        }
        /// <summary>
        /// ボードのテーブル回転制御
        /// </summary>
        private readonly IBoardRotation _BoardRot;
        /// <summary>
        /// PLC制御
        /// </summary>
        private readonly IPLCControl _PLC;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Rot_Provider(IPLCControl pLC, IBoardRotation board)
        {
            _PLC = pLC;
            _BoardRot = board;
            //PLC変更
            _PLC.StatusChanged += (s, e) =>
            {
                var stsname = (s as PLCmodel).Element;

                switch (stsname)
                {
                    case ("stsRotIndexPos"):
                        StsRotIndexPos = (s as PLCmodel).FloatStatus;
                        break;
                    case ("stsRotSpeed"):
                        StsRotSpeed = (s as PLCmodel).FloatStatus;
                        break;
                    case ("RotIndex"):
                        RotIndex = (s as PLCmodel).BoolStatus;
                        break;
                }
            };

            _BoardRot.StsChanged += (s, e) =>
            {
                StsAlarm = (s as BoardRotation).RotAlm;
                StsReady = (s as BoardRotation).RotReady;
            };

        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
