using System;
using TXSMechaControl.AuxSel;
using TXSMechaControl.FCD;
using TXSMechaControl.FDD;
using TXSMechaControl.FStage;
using TXSMechaControl.Rotation;
using TXSMechaControl.TblY;
using TXSMechaControl.UpDown;

namespace TXSMechaControl.MechaIntegrate
{
    public class MechaIntegrate : IMechaIntegrate, IDisposable
    {
        /// <summary>
        /// FCD
        /// </summary>
        public IFCD _FCD { get; }
        /// <summary>
        /// FDD
        /// </summary>
        public IFDD _FDD { get; }
        /// <summary>
        /// TableY
        /// </summary>
        public ITableY _TableY { get; }
        /// <summary>
        /// 昇降
        /// </summary>
        public IUdStage _Ud { get; }
        /// <summary>
        /// 回転
        /// </summary>
        public IRotation _Rot { get; }
        /// <summary>
        /// 微調
        /// </summary>
        public IFStage _FStage { get; }
        /// <summary>
        /// その他オプション
        /// </summary>
        public IAuxSel _Aux { get; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MechaIntegrate(IFCD fCD, IFDD fdd, ITableY tableY, IUdStage ud, IRotation rot,IFStage fs, IAuxSel aux)
        {
            _FCD = fCD;
            _FDD = fdd;
            _TableY = tableY;
            _Ud = ud;
            _Rot = rot;
            _FStage = fs;
            _Aux = aux;
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}
