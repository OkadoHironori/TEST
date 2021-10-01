using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{

    public class Roikey
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.ROIKEY Data;
        
        
        #region 読み込み/書き込みメソッド
        /// <summary>
        /// 読み込みメソッド
        /// </summary>
        public bool Read()
        {
            // 戻り値用変数
            bool result = false;

            try
            {
                // COMLIB.DLLの関数をコール
                result = (ComLib.GetRoikey(out Data) == 0);
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                string s = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 書き込みメソッド
        /// </summary>
        public bool Write()
        {
            // 戻り値用変数
            bool result = false;

            try
            {
                // COMLIB.DLLの関数をコール
                result = (ComLib.PutRoikey(ref Data) == 0);
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                string s = ex.Message;
            }

            return result;
        }
        #endregion

    }
}
