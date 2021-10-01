using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDetector
{
    /// <summary>
    /// バイナリ変換
    /// </summary>
    public class BinaryConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] ConvertStoB(short[] source, int width, int height)
        {
            byte[] target = new byte[source.Length * 2];

            Buffer.BlockCopy(source, 0, target, 0, source.Length * 2);

            return target;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] ConvertUStoB(ushort[] source)
        {
            byte[] target = new byte[source.Length * 2];

            Buffer.BlockCopy(source, 0, target, 0, source.Length * 2);

            return target;
        }
        /// <summary>
        /// Float →　byte[] BigEndian
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] ConvertFtoB_BigEndian(float[] source)
        {
            foreach (var idx in Enumerable.Range(0, source.Length))
            {
                source[idx] = Reverse(source[idx]);
            }

            byte[] target = new byte[source.Length * 2];

            Buffer.BlockCopy(source, 0, target, 0, source.Length * 2);

            return target;
        }
        /// <summary>
        /// Float →　byte[] littleEndian
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] ConvertFtoB_littleEndian(float[] source)
        {
            // create a byte array and copy the floats into it...
            byte[] byteArray = new byte[source.Length * 4];
            Buffer.BlockCopy(source, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }
        /// <summary>
        /// short →　byte[] BigEndian
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] ConvertStoB_BigEndian(short[] source)
        {
            foreach (var idx in Enumerable.Range(0, source.Length))
            {
                source[idx] = Reverse(source[idx]);
            }

            byte[] target = new byte[source.Length * 2];

            Buffer.BlockCopy(source, 0, target, 0, source.Length * 2);

            return target;
        }
        /// <summary>
        /// byte →　short
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static short[] ConvertBtoS(byte[] source, int width, int height, short Max, short Min)
        {
            short[] tmptarget = new short[source.Length / 2];

            Buffer.BlockCopy(source, 0, tmptarget, 0, width * height * 2);

            short[] outtarget = new short[width * height];

            foreach (var idx in Enumerable.Range(0, width * height))
            {
                outtarget[idx] = tmptarget[idx];
            }

            return outtarget;
        }

        /// <summary>
        /// byte →　short
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static short[] ConvertBtoS_BigEndian(byte[] source, int width, int height)
        {
            short[] tmptarget = new short[source.Length / 2];

            Buffer.BlockCopy(source, 0, tmptarget, 0, width * height * 2);

            short[] outtarget = new short[width * height];

            foreach (var idx in Enumerable.Range(0, width * height))
            {
                outtarget[idx] = Reverse(tmptarget[idx]);
            }
            return outtarget;
        }
        /// <summary>
        /// byte →　Float
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static float[] ConvertBtoF_BigEndian(byte[] source, int width, int height)
        {
            float[] tmptarget = new float[source.Length / 4];
            int cnt = 0;
            foreach (var idx in Enumerable.Range(0, source.Length).Where(p => p % 4 == 0))
            {
                tmptarget[cnt] = BitConverter.ToSingle(source, idx);
                cnt++;
            }
            return tmptarget;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="byte1"></param>
        /// <param name="byte2"></param>
        static void FromShort(short number, out byte byte1, out byte byte2)
        {
            byte1 = (byte)(number >> 8);
            byte2 = (byte)(number & 255);
        }
        /// <summary>
        /// 伝統的な16ビット入れ替え処理(符号なし)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static ushort Reverse(ushort value)
        {
            return (ushort)((value & 0xFF) << 8 | (value >> 8) & 0xFF);
        }
        /// <summary>
        /// 伝統的な16ビット入れ替え処理(符号あり)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static short Reverse(short value)
        {
            return (short)((value & 0xFF) << 8 | (value >> 8) & 0xFF);
        }
        /// <summary>
        /// 伝統的な32ビット入れ替え処理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static uint Reverse(uint value)
        {
            return (value & 0xFF) << 24 |
                    ((value >> 8) & 0xFF) << 16 |
                    ((value >> 16) & 0xFF) << 8 |
                    ((value >> 24) & 0xFF);
        }
        /// <summary>
        /// 浮動小数点はちょっと効率悪いけどライブラリでできる操作でカバーする
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Reverse(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value); // これ以上いい処理が思いつかない
            Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }

    }
}
