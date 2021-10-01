using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{
    public class MecaInf
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.MECAINF Data;

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetMecainf(out Data) != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// データ書き込み
        /// </summary>
        /// <returns></returns>
        public bool Write()
        {
            if (ComLib.PutMecainf(ref Data) != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// データを指定して書き込み
        /// </summary>
        /// <returns></returns>
        public bool Put(CTstr.MECAINF data)
        {
            if (ComLib.PutMecainf(ref data) != 0)
            {
                return false;
            }

            return true;
        }
    }
}
