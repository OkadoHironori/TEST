using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXSMechaControl.Common
{
    /// <summary>
    /// 干渉
    /// </summary>
    public class Collision
    {
        /// <summary>
        /// インクリメント時に接触
        /// </summary>
        public bool stsFTouch { get; set; }
        /// <summary>
        /// デクリメント時に接触
        /// </summary>
        public bool stsBTouch { get; set; }

        /// <summary>
        /// FCD テーブルＹ左移動中接触 //利用していないと思う
        /// </summary>
        ///public bool stsXLTouch { get; set; }
    }
}
