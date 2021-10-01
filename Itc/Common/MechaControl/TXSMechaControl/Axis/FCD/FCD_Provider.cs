

namespace TXSMechaControl.FCD
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
    /// FCDの変更通知
    /// </summary>
    public class FCD_Provider : BindableBase, IDisposable
    {
        /// <summary>
        /// FCDのSPDが更新された
        /// </summary>
        public event EventHandler SpdChanged;
        /// <summary>
        /// FCD
        /// </summary>
        private float _StsFCD;
        /// <summary>
        /// FCD
        /// </summary>
        public float StsFCD
        {
            get { return this._StsFCD; }
            set { SetProperty(ref _StsFCD, value); }
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
        /// 回転大ﾃｰﾌﾞﾙ有無    '追加 by 稲葉 14-02-26    //2014/10/06_v1951反映
        /// </summary>
        private bool _StsRotLargeTable;
        /// <summary>
        ///  回転大ﾃｰﾌﾞﾙ有無    '追加 by 稲葉 14-02-26    //2014/10/06_v1951反映
        /// </summary>
        public bool StsRotLargeTable
        {
            get { return this._StsRotLargeTable; }
            set { SetProperty(ref _StsRotLargeTable, value); }
        }
        /// <summary>
        /// Fcd軸のオーバーヒート
        /// </summary>
        private bool _StsFcdDriverHeat;
        /// <summary>
        /// Fcd軸のオーバーヒート
        /// </summary>
        public bool StsFcdDriverHeat
        {
            get { return this._StsFcdDriverHeat; }
            set { SetProperty(ref _StsFcdDriverHeat, value); }
        }
        /// <summary>
        /// FcdのHome位置
        /// </summary>
        private float _FCDHome;
        /// Fcd軸のHome位置
        /// </summary>
        public float FCDHome
        {
            get { return this._FCDHome; }
            set { SetProperty(ref _FCDHome, value); }
        }
        /// <summary>
        /// Fcdの原点位置
        /// </summary>
        private float _FCDOrigin;
        /// Fcdの原点位置
        /// </summary>
        public float FCDOrigin
        {
            get { return this._FCDOrigin; }
            set { SetProperty(ref _FCDOrigin, value); }
        }
        /// <summary>
        /// 浜松Fcd1の位置
        /// </summary>
        private float _FCD_Hama1;
        /// <summary>
        /// 浜松Fcd1の位置
        /// </summary>
        public float FCD_Hama1
        {
            get { return this._FCD_Hama1; }
            set { SetProperty(ref _FCD_Hama1, value); }
        }
        /// <summary>
        /// 浜松Fcd2の位置
        /// </summary>
        private float _FCD_Hama2;
        /// <summary>
        /// 浜松Fcd2の位置
        /// </summary>
        public float FCD_Hama2
        {
            get { return this._FCD_Hama2; }
            set { SetProperty(ref _FCD_Hama2, value); }
        }
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
        /// 前進時に接触
        /// </summary>
        public bool _stsFTouch;
        /// <summary>        
        /// 前進時に接触
        /// </summary>
        public bool StsFTouch
        {
            get { return this._stsFTouch; }
            set { SetProperty(ref _stsFTouch, value); }
        }
        /// <summary>        
        ///  後退時に接触
        /// </summary>
        public bool _stsBTouch;
        /// <summary>        
        ///  後退時に接触
        /// </summary>
        public bool StsBTouch
        {
            get { return this._stsBTouch; }
            set { SetProperty(ref _stsBTouch, value); }
        }
        /// <summary>
        /// 接触
        /// </summary>
        public bool IsTouch => StsBTouch | StsFTouch;
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
        /// ｲﾝﾃﾞｯｸｽ減速設定状態  '追加 by 稲葉 10-10-19 何に使ってるの？
        /// </summary>
        public bool StsIndexSlow { get; private set; }
        /// <summary>        
        /// 原点位置
        /// </summary>
        public bool _stsOrigin;
        /// <summary>        
        /// 原点位置
        /// </summary>
        public bool StsOrigin
        {
            get { return this._stsOrigin; }
            set { SetProperty(ref _stsOrigin, value); }
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
        /// PLC制御
        /// </summary>
        private readonly IPLCControl _iPLC;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FCD_Provider(IPLCControl pLC)
        {

            _iPLC = pLC;

            _iPLC.StatusChanged += (s, e) =>
            {
                var stsname = (s as PLCmodel).Element;

                switch (stsname)
                {
                    case ("stsFCD"):

                        StsFCD = (s as PLCmodel).FloatStatus;

                        break;

                    case ("stsRotLargeTable"):

                        StsRotLargeTable = (s as PLCmodel).BoolStatus;

                        break;

                    case ("FCDOrigin"):

                        FCDOrigin = (s as PLCmodel).FloatStatus;

                        break;

                    case ("FCDHome"):

                        FCDHome = (s as PLCmodel).FloatStatus;

                        break;

                    case ("stsFcdSpeed"):

                        StsSpeed = (s as PLCmodel).FloatStatus;

                        break;

                    case ("stsFcdMaxSpeed"):

                        if (!IsMaxSpeedFix)
                        {
                            StsMaxSpeed = (s as PLCmodel).FloatStatus;
                        }

                        IsMaxSpeedFix = true;
                        SpdChanged?.Invoke(this, new EventArgs());

                        break;

                    case ("stsFcdMinSpeed"):

                        if (!IsMinSpeedFix)
                        {
                            StsMinSpeed = (s as PLCmodel).FloatStatus;
                        }

                        IsMinSpeedFix = true;
                        SpdChanged?.Invoke(this, new EventArgs());

                        break;

                    case ("stsFcdIndexSlow"):

                        StsIndexSlow = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdFLimit"):

                        StsFLimit = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdBLimit"):

                        StsBLimit = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdFTouch"):

                        StsFTouch = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdBTouch"):

                        StsBTouch = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdDriverHeat"):

                        StsFcdDriverHeat = (s as PLCmodel).BoolStatus;

                        break;

                    case ("FCD_Coli1"): 

                        FCD_Hama1 = (s as PLCmodel).FloatStatus;

                        break;

                    case ("FCD_Coli2"):

                        FCD_Hama2 = (s as PLCmodel).FloatStatus;

                        break;
                    case ("stsFcdOrigin"):

                        StsOrigin = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdBusy"):

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

        }
    }
}
