using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TXSMechaControl.AuxSel;
using TXSMechaControl.FCD;
using TXSMechaControl.FDD;
using TXSMechaControl.FStage;
using TXSMechaControl.Rotation;
using TXSMechaControl.TblY;
using TXSMechaControl.UpDown;

namespace TXSMechaControl.MechaIntegrate
{
    public interface IMechaIntegrate
    {
        /// <summary>
        /// FCD
        /// </summary>
        IFCD _FCD { get; }
        /// <summary>
        /// FDD
        /// </summary>
        IFDD _FDD { get; }
        /// <summary>
        /// TableY
        /// </summary>
        ITableY _TableY { get; }
        /// <summary>
        /// 昇降
        /// </summary>
        IUdStage _Ud { get; }
        /// <summary>
        /// 回転
        /// </summary>
        IRotation _Rot { get; }
        /// <summary>
        /// 微調
        /// </summary>
        IFStage _FStage { get; }
        /// <summary>
        /// その他オプション
        /// </summary>
        IAuxSel _Aux { get; }
    }
}
