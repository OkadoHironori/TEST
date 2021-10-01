using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using CTAPI;

namespace CT30K.Common
{
    
    public class DispInf
    {
        /// <summary>
        /// 構造体データ
        /// </summary>
        public CTstr.DISPINF Data;


        #region 読み込み/書き込みメソッド
        /// <summary>
        /// 読み込みメソッド
        /// </summary>
        public bool Load()
        {
            //// 戻り値用変数
            //bool result = false;

            //try
            //{
            //    // COMLIB.DLLの関数をコール
            //    result = (ComLib.GetDispinf(out Data) == 0);
            //}
            //catch (Exception ex)
            //{
            //    // エラーメッセージ
            //    string s = ex.Message;
            //}

            //return result;

            if (ComLib.GetDispinf(out Data) != 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 書き込みメソッド
        /// </summary>
        public bool Put() 
        {
            //// 戻り値用変数
            //bool result = false;

            //try
            //{
            //    // COMLIB.DLLの関数をコール
            //    result = (ComLib.PutDispinf(ref Data) == 0);
            //}
            //catch (Exception ex)
            //{
            //    // エラーメッセージ
            //    string s = ex.Message;
            //}
            //return result;

            if (ComLib.PutDispinf(ref Data) != 0)
            {
                return false;
            }
            return true;


        }

        /// <summary>
        /// データ書き込み
        /// </summary>
        /// <returns></returns>
        public bool Put(CTstr.DISPINF data)
        {
            if (ComLib.PutDispinf(ref data) != 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
