using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{
    public class ScanCondPar
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.SCANCONDPAR Data;

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <param name="rotate_select"></param>
        /// <returns></returns>
        public bool Load(int rotate_select)
        {
            bool res = Load();

            if (res)
            {
                // 透視画像サイズ
                if (Data.fimage_hsize < 1) Data.fimage_hsize = 640;
                if (Data.fimage_vsize < 1) Data.fimage_vsize = 480;

                // 回転選択有効時はindex値が変更されてもfidオフセットは変更しないための措置
                if (rotate_select == 0)
                {
                    Data.fid_offset[1] = Data.fid_offset[0];
                }
            }

            return res;
        }

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetScancondpar(ref Data) != 0)
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
            if (ComLib.PutScancondpar(ref Data) != 0)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// データ書き込み
        /// </summary>
        /// <returns></returns>
        public bool Put(CTstr.SCANCONDPAR data)
        {
            if (ComLib.PutScancondpar(ref data) != 0)
            {
                return false;
            }

            return true;
        }
    }
}
