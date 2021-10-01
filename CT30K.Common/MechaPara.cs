using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{
    public class MechaPara
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.MECHAPARA Data;

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetMechapara(out Data) != 0)
            {
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// データ書き込み
        /// </summary>
        /// <returns></returns>
        public bool Put(CTstr.MECHAPARA data)
        {
            if (ComLib.PutMechapara(ref data) != 0)
            {
                return false;
            }

            return true;
        }


    }
}
