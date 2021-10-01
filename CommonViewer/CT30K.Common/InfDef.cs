using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{
    public class InfDef
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.INFDEF Data;

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetInfdef(out Data) != 0)
            {
                return false;
            }

            //CT30Kに戻す_20140/11/07hata
            //// 軸名称設定
            //for (int i = 0; i < AxisName.Length; i++)
            //{
            //    //string axis = CTstr.StrHelper.ToStr(Data.m_axis_name, 8, i);
            //    AxisName[i] = CTAPI.FixedString.GetString(Data.m_axis_name[i]);
            //}
            
            // フィルタ処理設定
            for (int i = 0; i < filter_process.Length; i++)
            {
                //filter_process[i] = CTstr.StrHelper.ToStr(Data.filter_process, 16, i);
                filter_process[i] = CTAPI.FixedString.GetString(Data.filter_process[i]);
            }

            // I.I.視野設定
            for (int i = 0; i < iifield.Length; i++)
            {
                //iifield[i] = CTstr.StrHelper.ToStr(Data.iifield, 16, i);
                iifield[i] = CTAPI.FixedString.GetString(Data.iifield[i]);
            }

            return true;
        }

        //CT30Kに戻す_20140/11/07hata
        ///// <summary>
        ///// 軸名称
        ///// </summary>
        //public readonly string[] AxisName = new string[2];

        /// <summary>
        /// ﾌｨﾙﾀ処理
        /// </summary>
        public readonly string[] filter_process = new string[2];

        /// <summary>
        /// I.I.視野
        /// </summary>
        public readonly string[] iifield = new string[3];


    }
}
