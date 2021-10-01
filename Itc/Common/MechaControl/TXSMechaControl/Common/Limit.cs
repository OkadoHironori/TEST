using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXSMechaControl.Common
{
    public class Limit
    {
        /// <summary>        
        /// 前進限
        /// </summary>
        public bool stsFLimit { get; set; }
        /// <summary>
        /// 後退限
        /// </summary>
        public bool stsBLimit { get; set; }
    }
}
