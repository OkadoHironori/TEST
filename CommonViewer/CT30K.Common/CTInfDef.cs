using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{
    public class CTInfDef
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.CTINFDEF Data;

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetCtinfdef(out Data) != 0)
            {
                return false;
            }

            // ｽｷｬﾝﾓｰﾄﾞ（FULLﾓｰﾄﾞ）設定
            for (int i = 0; i < full_mode.Length; i++)
            {
                //full_mode[i] = CTstr.StrHelper.ToStr(Data.full_mode, 6, i);
                full_mode[i] = CTAPI.FixedString.GetString(Data.full_mode[i]);
            }

            // 画像マトリックス設定
            for (int i = 0; i < matsiz.Length; i++)
            {
                //matsiz[i] = CTstr.StrHelper.ToStr(Data.matsiz, 4, i);
                matsiz[i] = CTAPI.FixedString.GetString(Data.matsiz[i]);
            
            }

            // 撮影領域設定
            for (int i = 0; i < scan_area.Length; i++)
            {
                //scan_area[i] = CTstr.StrHelper.ToStr(Data.scan_area, 2, i);
                scan_area[i] = CTAPI.FixedString.GetString(Data.scan_area[i]);
            }

            // フィルタ関数設定
            for (int i = 0; i < fc.Length; i++)
            {
                //fc[i] = CTstr.StrHelper.ToStr(Data.fc, 8, i);
                fc[i] = CTAPI.FixedString.GetString(Data.fc[i]);
            }

             return true;
        }

        /// <summary>
        /// ｽｷｬﾝﾓｰﾄﾞ（FULLﾓｰﾄﾞ）
        /// </summary>
        public readonly string[] full_mode = new string[3];

        /// <summary>
        /// 画像ﾏﾄﾘｯｸｽ
        /// </summary>
        public readonly string[] matsiz = new string[6];

        /// <summary>
        /// 撮影領域
        /// </summary>
        public readonly string[] scan_area = new string[3];

        /// <summary>
        /// ﾌｨﾙﾀ関数
        /// </summary>
        public readonly string[] fc = new string[3];

        /// <summary>
        /// ﾌｨﾙﾀ処理
        /// </summary>
        public readonly string[] filter_process = new string[3];

        /// <summary>
        /// I.I.視野
        /// </summary>
        public readonly string[] iifield = new string[3];

    }
}
