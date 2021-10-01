using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{

     public class ReconInf
    {
        /// <summary>
        /// Reconinf構造体
        /// </summary>
        public CTstr.RECONINF Data;
         
         
         #region 読み込み/書き込みメソッド
        /// <summary>
        /// 読み込みメソッド
        /// </summary>
        public bool Load()
        {
            // 戻り値用変数
            bool result = false;

            try
            {
                // COMLIB.DLLの関数をコール
                result = (ComLib.GetReconinf(out Data) == 0);
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
                result = (ComLib.PutReconinf(ref Data) == 0);
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
