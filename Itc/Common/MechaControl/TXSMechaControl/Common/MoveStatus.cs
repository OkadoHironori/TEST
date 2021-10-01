using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXSMechaControl.Common
{
    /// <summary>
    /// 動作状態
    /// </summary>
    public class Status
    {
        /// <summary>
        /// 前進中 インクリメント方向
        /// </summary>
        public bool stsForward { get; set; }
        /// <summary>
        /// 後退中　デクリメント方向
        /// </summary>
        public bool stsBackward { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy => !stsForward & !stsBackward;
        /// <summary>
        /// 進行方向
        /// </summary>
        public MDirMode DirMode { get; set; }
    }
}
