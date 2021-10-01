using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{
    public class HscPara
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.HSC_PARA Data;

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetHscpara(out Data) != 0)
            {
                return false;
            }
            return true;
        }

    }
}
