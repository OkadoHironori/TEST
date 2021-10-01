using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace CTAPI

{
    #region 固定長文字列構造体用インターフェース
    /// <summary>
    /// 固定長文字列構造体用インターフェース
    /// </summary>
    public interface IFixedString
    {
        byte[] Buf { get; set; }
    }
    #endregion

    #region FixedString2
    /// <summary>
    /// FixedString2 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString2 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }
    }
    #endregion

    #region FixedString4
    /// <summary>
    /// FixedString4 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString4 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }
    }
    #endregion

    #region FixedString6
    /// <summary>
    /// FixedString6 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString6 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }
    }
    #endregion

    #region FixedString8
    /// <summary>
    /// FixedString8 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString8 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }
    }
    #endregion

    #region FixedString10
    /// <summary>
    /// FixedString10 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString10 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        { 
            get { return buf; } 
            set { buf = value; } 
        }
    }
    #endregion

    #region FixedString16
    /// <summary>
    /// FixedString16 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString16 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }
    }
    #endregion

    #region FixedString18
    /// <summary>
    /// FixedString18 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString18 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }
    }
    #endregion

    #region FixedString20
    /// <summary>
    /// FixedString20 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString20 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }
    }
    #endregion

    #region FixedString32
    /// <summary>
    /// FixedString32 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString32 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }
    }
    #endregion

    #region FixedString64
    /// <summary>
    /// FixedString64 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString64 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }
    }
    #endregion

    #region FixedString128
    /// <summary>
    /// FixedString128 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString128 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }
    }
    #endregion

    #region FixedString256
    /// <summary>
    /// FixedString256 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FixedString256 : IFixedString
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        private byte[] buf;

        /// <summary>
        /// バイト配列
        /// </summary>
        public byte[] Buf
        {
            get { return buf; }
            set { buf = value; }
        }
    }
    #endregion

    #region 固定長文字列用スタティックメソッド用クラス
    /// <summary>
    /// 固定長文字列用スタティックメソッド用クラス
    /// </summary>
    public static class FixedString
    {
        /// <summary>
        /// 指定された固定長文字列を初期化する（拡張メソッド）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixedString"></param>
        /// <returns></returns>
        public static void Initialize<T>(this T fixedString) where T : IFixedString
        {
            fixedString.Buf = new byte[Marshal.SizeOf(typeof(T))];
        }

        /// <summary>
        ///  固定長文字列からstring型で文字列を取得する（拡張メソッド）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixedString"></param>
        /// <returns></returns>
        public static string GetString<T>(this T fixedString) where T : IFixedString
        {
            string rName = null;
            int tmp = 0;
            if (fixedString.Buf == null)
            {
                return string.Empty;
            }
            else
            {
                //return Encoding.Default.GetString(fixedString.Buf).TrimEnd('\0');
                //Rev99.99 変更 by長野 2014/12/04
                //文字列が"○○○\0「\0以外の任意の文字列」"の場合、TrimEndが"\0"を消してくれない。
                //その場合は、\0までの文字列を抜き出すようにした。
                //System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("shift-jis");
                //return Encoding.Default.GetString(fixedString.Buf).TrimEnd('\0');
                Encoding encoding = Encoding.GetEncoding("shift-jis");
                rName = encoding.GetString(fixedString.Buf).TrimEnd('\0');
                tmp = rName.IndexOf("\0");
                if (tmp == -1)
                {
                    return (rName);
                }
                else
                {
                    return(rName.Substring(0,tmp));
                }
            }
        }
        
        /// <summary>
        /// string型の文字列を固定長文字列型変数（byte[]）にセットする（拡張メソッド）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixedString"></param>
        /// <returns></returns>
        public static void SetString<T>(this T fixedString, string s) where T : IFixedString
        {
            if (fixedString.Buf == null) return;

            // バイト配列初期化
            Array.Clear(fixedString.Buf, 0, fixedString.Buf.Length);

            // エンコード
            var work = Encoding.Default.GetBytes(s);

            // コピー
            Array.Copy(work, fixedString.Buf, Math.Min(work.Length, fixedString.Buf.Length));
        }

        /// <summary>
        /// 指定されたサイズで固定長文字列の配列を作成する
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static T[] CreateArray<T>(int size) where T : IFixedString
        {
            var array = new T[size];

            for (int i = 0; i < array.Length; i++)
            {
                array[i].Initialize();
            }

            return array;
        }

        /// <summary>
        /// 通常の文字列配列に変換する（拡張メソッド）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixedStringArray"></param>
        /// <returns></returns>
        public static string[] ToStringArray<T>(this T[] fixedStringArray) where T : IFixedString
        {
            // 通常の文字列配列に変換
            return Array.ConvertAll<T, string>(fixedStringArray, target => target.GetString());
        }

        /// <summary>
        /// 指定された文字とマッチする固定型文字列配列のインデクス値を求める
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixedStringArray"></param>
        /// <param name="target"></param>
        /// <param name="MissingValue"></param>
        /// <returns></returns>
        public static int GetIndexByStr<T>(this T[] fixedStringArray, string target, int MissingValue) where T : IFixedString
        {
            for (int i = 0; i < fixedStringArray.Length; i++)
            {
                if (target.Trim().ToUpper().Equals(fixedStringArray[i].GetString().Trim().ToUpper()))
                {
                    return i;
                }
            }

            // 見つからなかった場合
            return MissingValue;
        }
    }
    #endregion

}
