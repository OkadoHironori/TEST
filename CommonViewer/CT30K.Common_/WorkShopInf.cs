using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{
    public class WorkShopInf
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.WORKSHOPINF Data;

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetWorkshopinf(out Data) != 0)
            {
                return false;
            }

            // 事業所名設定
            //workshop = CTstr.StrHelper.ToStr(Data.workshop, 16);
            workshop = CTAPI.FixedString.GetString(Data.workshop);

            return true;
        }

        /// <summary>
        /// 事業所名
        /// </summary>
        public string workshop { get; set; }
    }
}
