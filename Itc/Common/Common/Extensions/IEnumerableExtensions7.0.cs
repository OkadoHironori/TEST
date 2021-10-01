using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>C#7.0以降。Tupleが使える場合</remarks>
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// Index付のアイテムに纏める
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<(T value, int index)> SelectWithIndex<T>(this IEnumerable<T> source) => source.Select((x, i) => (x, i));

        /// <summary>
        /// Index付のアイテムに纏める
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<(T value, int[] indexs)> SelectWithIndex<T>(this T[,] source)
        {
            for (int x = 0; x < source.GetLength(0); ++x)
            {
                for (int y = 0; y < source.GetLength(1); ++y)
                {
                    yield return (source[x, y], new int[] { x, y });
                }
            }
        }

        /// <summary>
        /// ずらして纏める
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public static IEnumerable<(T, T)> Stagger<T>(this IEnumerable<T> source, int shift = 1)
        {
            if (shift <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            int count = source.Count();

            IEnumerable<T> first = source.Take(count - shift);

            IEnumerable<T> second = source.Skip(shift);

            return !first.Any() || !second.Any()
                ? Enumerable.Empty<(T, T)>() 
                : Enumerable.Zip(first, second, (f, s) => (f, s));
        }
    }

}
