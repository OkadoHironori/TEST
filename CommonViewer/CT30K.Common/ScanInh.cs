using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{
    /// <summary>
    /// データ収集初期化情報
    /// </summary>
    public class ScanInh
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.SCANINH Data;

        /// <summary>
        /// データ読込み
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ComLib.GetScaninh(out Data) != 0)
            {
                return false;
            }

            // GPGPU設定
            AppValue.GPGPU = (Data.gpgpu == 0 ? true : false);

            return true;
        }


    }
}
