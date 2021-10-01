using Itc.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common
{
    /// <summary>
    /// バイナリ変換
    /// </summary>
    public class BinaryConverter
    {
        /// <summary>
        /// Ptrからushort[]への変換
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ushort[] ConvertPtrtoUS(IntPtr source, int length)
        {
            short[] tmpsourceshort = new short[length];
            Marshal.Copy(source, tmpsourceshort, 0, length);

            ushort[] outtarget = new ushort[length];
            foreach (var idx in Enumerable.Range(0, length))
            {
                FromShort(tmpsourceshort[idx], out byte a, out byte b);
                byte[] tmpd = new byte[2];
                tmpd[0] = b;
                tmpd[1] = a;
                outtarget[idx] = BitConverter.ToUInt16(tmpd, 0);
            }
            return outtarget;
        }
        /// <summary>
        /// short[]からushort[]への変換
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ushort[] ConvertStoUS(short[] source, int width, int height)
        {
            ushort[] outtarget = new ushort[width * height];
            foreach (var idx in Enumerable.Range(0, source.Length))
            {
                FromShort(source[idx], out byte a, out byte b);
                byte[] tmpd = new byte[2];
                tmpd[0] = b;
                tmpd[1] = a;
                outtarget[idx] = BitConverter.ToUInt16(tmpd, 0);
            }
            return outtarget;
        }
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

            Buffer.BlockCopy(source, 0, target, 0, source.Length*2);

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

            foreach(var idx in Enumerable.Range(0,width*height))
            {
                outtarget[idx] = tmptarget[idx].CorrectRange(Min,Max);
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
            foreach (var idx in Enumerable.Range(0, source.Length).Where(p=>p%4==0))
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
        public static void FromShort(short number, out byte byte1, out byte byte2)
        {
            byte1 = (byte)(number >> 8);
            byte2 = (byte)(number & 255);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="byte1"></param>
        /// <param name="byte2"></param>
        public static void FromUShort(ushort number, out byte byte1, out byte byte2)
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
