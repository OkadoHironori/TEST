using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CTAPI;

namespace CT30K.Common
{
    public class zoomtbl
    { 
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.ZOOMTBL Data;

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetZoomtbl(out Data) != 0)
            {
                return false;
            }

            return true;
        }


    }
}
