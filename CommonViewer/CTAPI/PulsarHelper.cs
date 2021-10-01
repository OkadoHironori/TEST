using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CTAPI
{
    /// <summary>
    /// PulsarHelper.dll
    /// </summary>
    public class PulsarHelper
    {
        /// <summary>
        /// 透視画像表示用共有メモリハンドル
        /// </summary>
        public static IntPtr hMap = IntPtr.Zero;
        // CreateUserStopMapで設定されるハンドル値
        public static IntPtr hStopMap = IntPtr.Zero;      

        //-----------------------------------------------------------------------------
        // API関数
        //-----------------------------------------------------------------------------
        #region API
        /// <summary>
        /// 透視画像表示用共有メモリの生成
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [DllImport("PulsarHelper.dll")]
        ///public static extern IntPtr CreateTransImageMap(ushort width, ushort height);
        public static extern IntPtr CreateTransImageMap(int width, int height);

        /// <summary>
        /// 透視画像表示用共有メモリの破棄
        /// </summary>
        /// <param name="hMap"></param>
        [DllImport("PulsarHelper.dll")]
        public static extern void DestroyTransImageMap(IntPtr hMap);

        /// <summary>
        /// 透視画像表示用共有メモリから透視画像を取得する
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [DllImport("PulsarHelper.dll")]
        public static extern int GetTransImage(ushort[] image);

        /// <summary>
        /// キャプチャ停止用共有メモリにフラグを書き込む
        /// </summary>
        /// <param name="flag"></param>
        [DllImport("PulsarHelper.dll")]
        public static extern void SetCancel(int flag);

        /// <summary>
        /// キャプチャ停止用共有メモリにフラグを読み込む //Rev20.00 追加 by長野 2014/09/18
        /// </summary>
        /// <param name="flag"></param>
        [DllImport("PulsarHelper.dll")]
        public static extern bool CheckCancel();

        /// <summary>
        /// キャプチャ停止用共有メモリの生成
        /// </summary>
        /// <returns></returns>
        [DllImport("PulsarHelper.dll")]
        public static extern IntPtr CreateUserStopMap();

        /// <summary>
        /// キャプチャ停止用共有メモリの破棄
        /// </summary>
        /// <param name="hMap"></param>
        [DllImport("PulsarHelper.dll")]
        public static extern void DestroyUserStopMap(IntPtr hMap);


        #endregion API
    }
}
