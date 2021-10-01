using System;

using System.IO;
using System.Runtime.InteropServices;

namespace Itc.Common.Utility
{
    /// <summary>
    /// 配列データ読み込み
    /// </summary>
    public static partial class Tools
    {
        /// <summary>
        /// 画像バイナリデータを配列へ読み込み
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filepath">ファイルパス</param>
        /// <param name="srcoffset">オフセット</param>
        /// <param name="dstoffset"></param>
        /// <returns>画像データ</returns>
        public static T[] LoadToArray<T>(string filepath, int srcoffset = 0, int dstoffset = 0) 
            where T : struct
        {
            //Byte型で読み込み
            byte[] srcbuf = File.ReadAllBytes(filepath);

            if (null == srcbuf)
            {
                //空だった場合には例外処理を行う？
                throw new NotImplementedException();
            }

            System.Diagnostics.Debug.Assert(srcoffset >= 0);
            System.Diagnostics.Debug.Assert(dstoffset >= 0);

            int len = (srcbuf.Length - srcoffset) / Marshal.SizeOf(typeof(T));

            if (len <= 0)
            {
                //オフセット値異常
                throw new NotImplementedException();
            }

            T[] dstbuf = new T[len];

            //コピー
            Buffer.BlockCopy(srcbuf, srcoffset, dstbuf, dstoffset, srcbuf.Length - srcoffset);

            return dstbuf;
        }

        /// <summary>
        /// 配列をバイナリデータで保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filepath">ファイルパス</param>
        /// <param name="srcbuf">画像データ</param>
        public static void SaveArray<T>(string filepath, T[] srcbuf) 
            where T : struct
        {
            System.Diagnostics.Debug.Assert(null != srcbuf);

            //byte配列に変換
            int len = srcbuf.Length * Marshal.SizeOf(typeof(T));

            var dstbuf = new byte[len];

            Buffer.BlockCopy(srcbuf, 0, dstbuf, 0, dstbuf.Length);

            //保存
            File.WriteAllBytes(filepath, dstbuf);
        }
    }
}
