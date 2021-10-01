

namespace TXSMechaControl.TblY
{
    using Itc.Common.Extensions;
    using PLCController;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TXSMechaControl.Common;

    /// <summary>
    /// TblYの変更通知
    /// </summary>
    public class TblY_Provider : BindableBase, IDisposable
    {
        /// <summary>
        /// SPDが更新された
        /// </summary>
        public event EventHandler SpdChanged;
        /// <summary>
        /// テーブルY軸の位置
        /// </summary>
        private float _StsTblY;
        /// <summary>
        /// テーブルY軸の位置
        /// </summary>
        public float StsTblY
        {
            get { return this._StsTblY; }
            set { SetProperty(ref _StsTblY, value); }
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
        /// 最高速度
        /// </summary>
        public float StsMaxSpeed { get; private set; }
        public bool IsMaxSpeedFix = false;
        /// <summary>
        /// 最低速度
        /// </summary>
        public float StsMinSpeed { get; private set; }
        public bool IsMinSpeedFix = false;
        /// <summary>
        /// TblY_原点位置
        /// </summary>
        private float _TblYOrigin;
        /// <summary>
        /// TblY_原点位置
        /// </summary>
        public float TblYOrigin
        {
            get { return this._TblYOrigin; }
            set { SetProperty(ref _TblYOrigin, value); }
        }
        /// <summary>        
        /// 壁限
        /// </summary>
        public bool _stsLLimit;
        /// <summary>        
        /// 壁限
        /// </summary>
        public bool StsLLimit
        {
            get { return this._stsLLimit; }
            set { SetProperty(ref _stsLLimit, value); }
        }
        /// <summary>        
        ///  窓側限
        /// </summary>
        public bool _stsRLimit;
        /// <summary>        
        ///  窓側限
        /// </summary>
        public bool StsRLimit
        {
            get { return this._stsRLimit; }
            set { SetProperty(ref _stsRLimit, value); }
        }

        /// <summary>
        /// 浜松Fcd1の位置
        /// </summary>
        private float _TblY_Hama1;
        /// <summary>
        /// 浜松Fcd1の位置
        /// </summary>
        public float TblY_Hama1
        {
            get { return this._TblY_Hama1; }
            set { SetProperty(ref _TblY_Hama1, value); }
        }
        ///// <summary>
        /// 浜松Fcd2の位置
        /// </summary>
        private float _TblY_Hama2;
        /// <summary>
        /// 浜松Fcd2の位置
        /// </summary>
        public float TblY_Hama2
        {
            get { return this._TblY_Hama2; }
            set { SetProperty(ref _TblY_Hama2, value); }
        }
        /// <summary>        
        /// ビジー
        /// </summary>
        public bool _stsBusy;
        /// <summary>        
        /// ビジー
        /// </summary>
        public bool StsBusy
        {
            get { return this._stsBusy; }
            set { SetProperty(ref _stsBusy, value); }
        }
        /// <summary>
        /// FDDが原点位置にいるか？
        /// </summary>
        private bool _StsOrigin;
        /// <summary>
        /// FDDが原点位置にいるか？
        /// </summary>
        public bool StsOrigin
        {
            get { return this._StsOrigin; }
            set { SetProperty(ref _StsOrigin, value); }
        }
        /// <summary>
        /// PLC制御
        /// </summary>
        private readonly IPLCControl _PLC;
        /// <summary>
        /// FCDコンストラクタ
        /// </summary>
        public TblY_Provider(IPLCControl pLC)
        {
            _PLC = pLC;

            _PLC.StatusChanged += (s, e) =>
            {
                var stsname = (s as PLCmodel).Element;

                switch (stsname)
                {
                    case ("stsTblYPosition"):

                        StsTblY = (s as PLCmodel).FloatStatus;
                        break;

                    case ("stsTblYSpeed"):

                        StsSpeed = (s as PLCmodel).FloatStatus;
                        break;

                    case ("stsTblYMaxSpeed"):

                        if (!IsMaxSpeedFix)
                        {
                            StsMaxSpeed = (s as PLCmodel).FloatStatus;
                        }

                        IsMaxSpeedFix = true;

                        SpdChanged?.Invoke(this, new EventArgs());

                        break;

                    case ("stsTblYMinSpeed"):

                        if (!IsMinSpeedFix)
                        {
                            StsMinSpeed = (s as PLCmodel).FloatStatus;
                        }

                        IsMinSpeedFix = true;

                        SpdChanged?.Invoke(this, new EventArgs());

                        break;


                    case ("stsTblYLLimit"):

                        StsLLimit = (s as PLCmodel).BoolStatus;

                        break;
                    case ("stsTblYRLimit"):

                        StsRLimit = (s as PLCmodel).BoolStatus;

                        break;

                    case ("TblYOrigin"):
                        //テーブルY軸の原点位置
                        TblYOrigin = (-1) * (s as PLCmodel).FloatStatus;
                        break;

                    case ("TblY_Coli1"):
                        TblY_Hama1 = (s as PLCmodel).FloatStatus;
                        break;
                    case ("TblY_Coli2"):
                        TblY_Hama2 = (s as PLCmodel).FloatStatus;
                        break;
                    case ("stsTblYOrigin"):
                        StsOrigin = (s as PLCmodel).BoolStatus;
                        break;
                    case ("stsTblYBusy"):
                        StsBusy = (s as PLCmodel).BoolStatus;
                        break;
                }
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
