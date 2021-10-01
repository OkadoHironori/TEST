using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace Itc.Common.Extensions
{
    /// <summary>
    /// BitArray関係の拡張メソッド
    /// </summary>
    public static class BitArrayExtensions
    {
        /// <summary>
        /// 全ビットがfalseか判断します
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static bool Any(this BitArray bits)
        {
            foreach (bool bit in bits)
            {
                if(bit) return true;
            }

            return false;
        }

        /// <summary>
        /// 全ビットがtrueか判断します
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static bool All(this BitArray bits)
        {
            foreach (bool bit in bits)
            {
                if (!bit) return false;
            }

            return true;
        }

        /// <summary>
        /// byte配列に変換します
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this BitArray bits)
        {
            byte[] data = new byte[bits.Length / 8];

            bits.CopyTo(data, 0);

            return data;
        }
    }
}
