using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using CT30K.Common.Properties;

namespace CT30K.Common
{
    /// <summary>
    /// メッセージ文字列取得クラス
    /// </summary>
    public class ResMessage
    {
        /// <summary>
        /// リソース文字列取得
        /// </summary>
        /// <param name="resourceName">リソース名</param>
        /// <returns>文字列</returns>
        public static string Get(string resourceName)
        {
            return Resources.ResourceManager.GetString(resourceName);
        }

        /// <summary>
        /// 番号からリソース文字列取得
        /// </summary>
        /// <param name="number">番号</param>
        /// <returns></returns>
        public static string Get(int number)
        {
            string no = String.Format("{0:00000}", number);
            string resMsg = "Msg_" + no;

            return Resources.ResourceManager.GetString(resMsg);
        }

        /// <summary>
        /// IDS定数からリソース文字列取得
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string Get(IDSNum ids)
        {
            return Get((int)ids);
        }

    }

    /// <summary>
    /// ストリングテーブル定数
    /// </summary>
    public enum IDSNum : int
    {
        IDS_DoubleStart = 9349, // 2重起動

        IDS_NotFound = 9913,    // %1が見つかりません。

        IDS_Axis = 12160,       // %1軸

        IDS_ConflictScanInhibit = 17486,    // %1と%2がともに有効なため、CT30Kを起動できません。
        IDS_XrayRotate = 17487,         // X線管回転
        IDS_HighSpeedCamera = 17488,    // 高速度透視撮影
        IDS_MultiTube = 17489,          // 複数X線管
        IDS_RotateSelect = 17490,       // 回転選択

    }

}
