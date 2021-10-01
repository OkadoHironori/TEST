using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
    public class modRoikey
    {
        /// <summary>
        /// ROIｷｰ情報
        /// </summary>
        public struct roikeyType
        {
            /// <summary>
            /// ﾛｲ種別ﾓｰﾄﾞ
            /// </summary>
            public int imgroi;

            /// <summary>
            /// ﾛｲ動作ﾓｰﾄﾞ
            /// </summary>
            public int roi_mode;

            /// <summary>
            /// ﾛｲ X座標
            /// </summary>
            public int roi_x;

            /// <summary>
            /// ﾛｲ Y座標
            /// </summary>
            public int roi_y;

            /// <summary>
            /// ﾛｲ Xｻｲｽﾞ
            /// </summary>
            public int roi_xsize;

            /// <summary>
            /// ﾛｲ Yｻｲｽﾞ
            /// </summary>
            public int roi_ysize;

            /// <summary>
            /// 不規則ROI座標
            /// </summary>
            public int[,] trace_pos;

            /// <summary>
            /// 不規則ROI座標
            /// </summary>
            public void Initialize()
            {
                trace_pos = new int[2, 256];
            }
        }

        /// <summary>
        /// ROIｷｰ情報
        /// </summary>
        public static roikeyType roikey;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public modRoikey()
        {
            // ROIｷｰ情報
            roikey = new roikeyType();

            // 構造体内の配列初期化
            roikey.Initialize();
        }

        //roikey（コモン）取得関数
        [DllImport("comlib.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int GetRoikey(ref roikeyType theRoikey);

        [DllImport("comlib.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int PutRoikey(ref roikeyType theRoikey);
    }
}
