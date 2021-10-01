using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CTAPI;

namespace CT30K.Common
{
    public class sp2inf
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.SP2INF Data;

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetSp2inf(out Data) != 0)
            {
                return false;
            }

            return true;
        }


    }
}
