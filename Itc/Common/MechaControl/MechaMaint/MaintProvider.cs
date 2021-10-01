using Itc.Common.Extensions;
using PLCController;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaMaintCnt
{
    public class MaintProvider : BindableBase, IDisposable
    {
        /// <summary>        
        /// 原点位置
        /// </summary>
        public bool _StsFCDOrigin;
        /// <summary>        
        /// 原点位置
        /// </summary>
        public bool StsFCDOrigin
        {
            get { return this._StsFCDOrigin; }
            set { SetProperty(ref _StsFCDOrigin, value); }
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
        /// 扉インターロック
        /// </summary>
        private bool _DoorInterlock;
        /// <summary>
        /// 扉インターロック
        /// </summary>
        public bool DoorInterlock
        {
            get { return this._DoorInterlock; }
            set { SetProperty(ref _DoorInterlock, value); }
        }
        /// <summary>
        /// X線接触センサ取付チェック出力
        /// </summary>
        private bool _XraySourceCollisionSensor;
        /// <summary>
        /// X線接触センサ取付チェック出力
        /// </summary>
        public bool XraySourceCollisionSensor
        {
            get { return this._XraySourceCollisionSensor; }
            set { SetProperty(ref _XraySourceCollisionSensor, value); }
        }
        /// <summary>
        /// 浜松ホト干渉防止設定
        /// </summary>
        private bool _XraySourceCollisionPrevent;
        /// <summary>
        /// 浜松ホト干渉防止設定
        /// </summary>
        public bool XraySourceCollisionPrevent
        {
            get { return this._XraySourceCollisionPrevent; }
            set { SetProperty(ref _XraySourceCollisionPrevent, value); }
        }
        /// <summary>
        /// PLC
        /// </summary>
        private readonly IPLCMonitor _PLCMonitor;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="monitor"></param>
        public MaintProvider(IPLCMonitor monitor)
        {
            _PLCMonitor = monitor;

            _PLCMonitor.StatusChanged += (s, e) =>
            {
                var stsname = (s as PLCmodel).Element;

                Debug.WriteLine(stsname);

                switch (stsname)
                {
                    case ("DoorInterlock"):

                        DoorInterlock = (s as PLCmodel).BoolStatus;

                        break;

                    case ("XraySourceCollisionSensor"):

                        XraySourceCollisionSensor = (s as PLCmodel).BoolStatus;

                        break;

                    case ("XraySourceCollisionPrevent"):

                        XraySourceCollisionPrevent = (s as PLCmodel).BoolStatus;

                        break;

                    case ("FCDOrigin"):

                        FCDOrigin = (s as PLCmodel).FloatStatus;

                        break;

                    case ("FCDHome"):

                        FCDHome = (s as PLCmodel).FloatStatus;

                        break;

                    case ("FCD_Coli1"):

                        FCD_Hama1 = (s as PLCmodel).FloatStatus;

                        break;

                    case ("FCD_Coli2"):

                        FCD_Hama2 = (s as PLCmodel).FloatStatus;

                        break;

                    case ("stsFcdOrigin"):

                        StsFCDOrigin = (s as PLCmodel).BoolStatus;

                        break;
                };
            };

        }

        public void Dispose()
        {

        }
    }
}
