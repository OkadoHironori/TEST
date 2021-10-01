using Itc.Common.Extensions;
using PLCController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXSMechaControl.AuxSel
{
    /// <summary>
    /// PLCのオプション設定を監視
    /// </summary>
    public class AuxSel_Provider: BindableBase, IDisposable
    {
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
        /// PLC制御
        /// </summary>
        private readonly IPLCControl _iPLC;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pLC"></param>
        public AuxSel_Provider(IPLCControl pLC)
        {
            _iPLC = pLC;
            _iPLC.StatusChanged += (s, e) =>
            {
                var stsname = (s as PLCmodel).Element;

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
