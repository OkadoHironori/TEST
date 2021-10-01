using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.Extensions
{
    /// <summary>
    /// IComparable<T> 関係の拡張メソッド
    /// </summary>
    public static class IComparableExtensions
    {
        /// <summary>
        /// 指定した値より大きいか判断します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsGreaterThan<T>(this T a, T b)
            where T : IComparable<T> => a.CompareTo(b) > 0;

        /// <summary>
        /// 指定した値と等しいか判断します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsEqual<T>(this T a, T b)
            where T : IComparable<T> => a.CompareTo(b) == 0;

        /// <summary>
        /// 指定した値より小さいか判断します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsLessThan<T>(this T a, T b)
            where T : IComparable<T> => a.CompareTo(b) < 0;

        /// <summary>
        /// 指定した値以上か判断します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsNotLessThan<T>(this T a, T b)
            where T : IComparable<T> => !a.IsLessThan(b);

        /// <summary>
        /// 指定した値以下か判断します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsNotGreaterThan<T>(this T a, T b)
            where T : IComparable<T> => !a.IsGreaterThan(b);

        /// <summary>
        /// lower～higherの範囲内か判断します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="lower"></param>
        /// <param name="higher"></param>
        /// <param name="inclusive"></param>
        /// <returns></returns>
        public static bool InRange<T>(this T target, T lower, T higher, bool inclusive = true)
            where T : IComparable<T>
        {
            if (lower.CompareTo(higher) > 0)
            {
                throw new ArgumentException();
            }

            return inclusive
                ? target.IsNotLessThan(lower) && target.IsNotGreaterThan(higher)
                : target.IsGreaterThan(lower) && target.IsLessThan(higher);
        }

        /// <summary>
        /// lower～higherの範囲内に修正します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="lower"></param>
        /// <param name="higher"></param>
        /// <returns></returns>
        public static T CorrectRange<T>(this T target, T lower, T higher)
            where T : IComparable<T>
        {
            if (lower.CompareTo(higher) > 0)
            {
                throw new ArgumentException();
            }

            if (target.IsLessThan(lower))
                return lower;
            else if (target.IsGreaterThan(higher))
                return higher;
            else
                return target;
        }
    }
}
