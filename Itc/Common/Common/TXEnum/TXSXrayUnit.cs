using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.TXEnum
{
    /// <summary>
    /// 管電流の単位
    /// </summary>
    public enum Current:int
    {
        /// <summary>
        /// mA
        /// </summary>
        mA,
        /// <summary>
        /// µA
        /// </summary>
        uA,
    }
    /// <summary>
    /// 管電圧の単位
    /// </summary>
    public enum Voltage:int
    {
        /// <summary>
        /// kV
        /// </summary>
        kV=1,
    }
}
