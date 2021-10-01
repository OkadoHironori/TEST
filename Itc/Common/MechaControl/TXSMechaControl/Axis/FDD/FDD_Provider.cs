

namespace TXSMechaControl.FDD
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
    /// FDDの変更通知
    /// </summary>
    public class FDD_Provider : BindableBase, IDisposable
    {
        /// <summary>
        /// 速度が更新された
        /// </summary>
        public event EventHandler SpdChanged;
        /// <summary>
        /// 速度
        /// </summary>
        private float _StsFDD;
        /// <summary>
        /// FDD
        /// </summary>
        public float StsFDD
        {
            get { return this._StsFDD; }
            set { SetProperty(ref _StsFDD, value); }
        }
        /// <summary>
        /// FDDの有効桁数
        /// </summary>
        //public int FDD_Decip { get; private set; }
        /// <summary>
        /// FDD_原点位置
        /// </summary>
        private float _FDDOrigin;
        /// <summary>
        /// FDD_原点位置
        /// </summary>
        public float FDDOrigin
        {
            get { return this._FDDOrigin; }
            set { SetProperty(ref _FDDOrigin, value); }
        }
        /// <summary>
        /// FDDホームの有効桁数
        /// </summary>
        //public int FDDOrigin_Decip { get; private set; }
        /// <summary>        
        /// 前進限
        /// </summary>
        public bool _stsFLimit;
        /// <summary>        
        /// 前進限
        /// </summary>
        public bool StsFLimit
        {
            get { return this._stsFLimit; }
            set { SetProperty(ref _stsFLimit, value); }
        }
        /// <summary>        
        ///  後退限
        /// </summary>
        public bool _stsBLimit;
        /// <summary>        
        ///  後退限
        /// </summary>
        public bool StsBLimit
        {
            get { return this._stsBLimit; }
            set { SetProperty(ref _stsBLimit, value); }
        }
        /// <summary>
        /// Fcd軸のオーバーヒート
        /// </summary>
        private bool _StsFddDriverHeat;
        /// Fcd軸のオーバーヒート
        /// </summary>
        public bool StsFddDriverHeat
        {
            get { return this._StsFddDriverHeat; }
            set { SetProperty(ref _StsFddDriverHeat, value); }
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
        private readonly IPLCControl _iPLC;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FDD_Provider(IPLCControl pLC)
        {
            _iPLC = pLC;

            _iPLC.StatusChanged += (s, e) =>
            {
                var stsname = (s as PLCmodel).Element;

                switch (stsname)
                {
                    case ("stsFDD"):

                        StsFDD = (s as PLCmodel).FloatStatus;

                        //FDD_Decip = pLC.ConvertScaleToDecip((s as PLCmodel).Scale);

                        break;
                    case ("stsFddSpeed"):

                        StsSpeed = (s as PLCmodel).FloatStatus;

                        break;

                    case ("stsFddMaxSpeed"):

                        //StsMaxSpeed = (s as PLCmodel).FloatStatus;
                        if (!IsMaxSpeedFix)
                        {
                            StsMaxSpeed = (s as PLCmodel).FloatStatus;
                        }

                        IsMaxSpeedFix = true;

                        SpdChanged?.Invoke(this, new EventArgs());

                        break;

                    case ("stsFddMinSpeed"):

                        //StsMinSpeed = (s as PLCmodel).FloatStatus;
                        if (!IsMinSpeedFix)
                        {
                            StsMinSpeed = (s as PLCmodel).FloatStatus;
                        }

                        IsMinSpeedFix = true;

                        SpdChanged?.Invoke(this, new EventArgs());


                        break;

                    case ("FDDOrigin"):

                        FDDOrigin = (-1) * (s as PLCmodel).FloatStatus;

                        //FDDOrigin_Decip = pLC.ConvertScaleToDecip((s as PLCmodel).Scale);

                        break;

                    case ("stsFddOrigin"):

                        StsOrigin = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFddBusy"):

                        StsBusy = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFddFLimit"):
                        StsFLimit = (s as PLCmodel).BoolStatus;
                        break;

                    case ("stsFddBLimit"):
                        StsBLimit = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFddDriverHeat"):
                        StsFddDriverHeat = (s as PLCmodel).BoolStatus;
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
