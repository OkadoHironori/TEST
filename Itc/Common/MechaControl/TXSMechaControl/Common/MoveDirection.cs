using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXSMechaControl.Common
{
    public class MoveStatus
    {
        /// <summary>
        /// 制限位置？
        /// </summary>
        public Limit Limit { get; private set; } = new Limit();
        /// <summary>
        /// 干渉
        /// </summary>
        public Collision Collision { get; private set; } = new Collision();
        /// <summary>
        /// 動作状態
        /// </summary>
        public Status Status { get; private set; } = new Status();
    }
}
