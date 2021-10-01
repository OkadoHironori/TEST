using Itc.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXSMechaControl.Common
{
    /// <summary>
    /// 速度
    /// </summary>
    public class Speed
    {
        /// <summary>
        /// 最高速度
        /// </summary>
        public float stsMaxSpeed { get; set; }
        /// <summary>
        /// 最低速度
        /// </summary>
        public float stsMinSpeed { get; set; }
        ///// <summary>
        ///// 減速速度 追加 by 稲葉 18-01-15
        ///// </summary
        //public float stsDecelerationSpeed { get; set; }
        /// <summary>
        /// ｲﾝﾃﾞｯｸｽ減速設定状態  '追加 by 稲葉 10-10-19
        /// </summary>
        public bool stsIndexSlow { get; set; }

        public void Dispose()
        {
            
        }
    }
}
