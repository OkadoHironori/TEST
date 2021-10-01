using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{
    public class XTable
    {
        private const int COND_MAX = 3;
        private const int II_MAX = 3;

        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.X_TABLE Data;
        
        /// <summary>
        /// データ取得
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetXtable(out Data) != 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// I.I.インデックス範囲内
        /// </summary>
        /// <param name="iiIndex"></param>
        /// <returns></returns>
        public bool InRangeII(int iiIndex)
        {
            if ((iiIndex >= 0) && (iiIndex < II_MAX))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 条件インデックス範囲内
        /// </summary>
        /// <param name="condIndex"></param>
        /// <returns></returns>
        public bool InRangeCond(int condIndex)
        {
            if ((condIndex >= 0) && (condIndex < COND_MAX))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 管電圧取得
        /// </summary>
        /// <param name="iiIndex"></param>
        /// <param name="condIndex"></param>
        /// <returns></returns>
        public float GetVolt(int iiIndex, int condIndex)
        {
            float volt = 0;
            if (InRangeII(iiIndex) && InRangeCond(condIndex))
            {
                int tableIndex = (iiIndex * 6) + (condIndex * 2);
                volt = Data.xtable[tableIndex];
                //volt = Data.xtable[iiIndex, iiIndex, condIndex];
            }
            return volt;
        }

        /// <summary>
        /// 管電流取得
        /// </summary>
        /// <param name="iiIndex"></param>
        /// <param name="condIndex"></param>
        /// <returns></returns>
        public float GetCurrent(int iiIndex, int condIndex)
        {
            float current = 0;
            if (InRangeII(iiIndex) && InRangeCond(condIndex))
            {
                int tableIndex = (iiIndex * 6) + (condIndex * 2) + 1;
                current = Data.xtable[tableIndex];
                //current = Data.xtable[iiIndex, iiIndex, condIndex];
            }
            return current;
        }
    }
}
