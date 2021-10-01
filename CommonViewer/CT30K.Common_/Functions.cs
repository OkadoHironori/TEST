using System;
using System.IO;
using System.Runtime.InteropServices;

using CTAPI;

namespace CT30K.Common
{
    /// <summary>
    /// 汎用関数クラス
    /// </summary>
    public static class Functions
    {        
        /// <summary>
        /// 文字列配列に変換（拡張メソッド）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] ToStrArray<T>(this T[] array)
        {
            // 文字列配列に変換
            return Array.ConvertAll<T, string>(array, i => i.ToString());
        }
        
        /// <summary>
        /// 構造体データ読み込みメソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="rec"></param>
        /// <returns></returns>
        public static bool ReadStructure<T>(string fileName, ref T rec) 
        {
            bool result = false;

            try
            {
                using (Stream s = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] buf = new byte[Marshal.SizeOf(typeof(T))];
                    s.Read(buf, 0, buf.Length);

                    IntPtr p = Marshal.AllocHGlobal(buf.Length);
                    try
                    {
                        Marshal.Copy(buf, 0, p, buf.Length);
                        rec = (T)Marshal.PtrToStructure(p, typeof(T));
                        result = true; 
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(p);
                    }
                };
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                string s = ex.Message;
            }

            return result;
        }
        
        /// <summary>
        /// 構造体データ書き込みメソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="rec"></param>
        /// <returns></returns>
        public static bool WriteStructure<T>(string fileName, T rec)
        {
            // 戻り値用変数
            bool result = false;

            try
            {
                using (Stream s = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    // 書き込み用バッファ用意
                    byte[] buf = new byte[Marshal.SizeOf(typeof(T))];

                    // メモリ確保
                    IntPtr p = Marshal.AllocHGlobal(buf.Length);

                    try
                    {
                        // マーシャリング
                        Marshal.StructureToPtr(rec, p, true);

                        // コピー
                        Marshal.Copy(p, buf, 0, buf.Length);

                        // 書き込み
                        s.Write(buf, 0, buf.Length);

                        // 成功！
                        result = true;
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(p);
                    }
                };
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                string s = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 構造体データ読み込みメソッド（ダミー）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="rec"></param>
        /// <returns></returns>
        public static bool ReadStructureDummy<T>(string fileName, ref T rec)
        {
            bool result = false;

            try
            {
                //using (Stream s = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                //{
                    byte[] buf = new byte[Marshal.SizeOf(typeof(T))];
                //    s.Read(buf, 0, buf.Length);

                    IntPtr p = Marshal.AllocHGlobal(buf.Length);
                    try
                    {
                        Marshal.Copy(buf, 0, p, buf.Length);
                        rec = (T)Marshal.PtrToStructure(p, typeof(T));
                        result = true;
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(p);
                    }
                //};
            }
            catch (Exception ex)
            {
                // エラーメッセージ
                string s = ex.Message;
            }

            return result;
        }
        
        /// <summary>
        /// 文字列の指定した位置から指定した長さを取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="start">開始位置</param>
        /// <param name="len">長さ</param>
        /// <returns>取得した文字列</returns>
        public static string Mid(string str, int start, int len)
        {
            if (start <= 0)
            {
                throw new ArgumentException("引数'start'は1以上でなければなりません。");
            }
            if (len < 0)
            {
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            }
            if (str == null || str.Length < start)
            {
                return "";
            }
            if (str.Length < (start + len))
            {
                return str.Substring(start - 1);
            }
            return str.Substring(start - 1, len);
        }

        /// <summary>
        /// 文字列の指定した位置から末尾までを取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="start">開始位置</param>
        /// <returns>取得した文字列</returns>
        public static string Mid(string str, int start)
        {
            return Mid(str, start, str.Length);
        }

        /// <summary>
        /// 文字列の先頭から指定した長さの文字列を取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="len">長さ</param>
        /// <returns>取得した文字列</returns>
        public static string Left(string str, int len)
        {
            if (len < 0)
            {
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            }
            if (str == null)
            {
                return "";
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(0, len);
        }

        /// <summary>
        /// 文字列の末尾から指定した長さの文字列を取得する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="len">長さ</param>
        /// <returns>取得した文字列</returns>
        public static string Right(string str, int len)
        {
            if (len < 0)
            {
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            }
            if (str == null)
            {
                return "";
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(str.Length - len, len);
        }
    }  
}
