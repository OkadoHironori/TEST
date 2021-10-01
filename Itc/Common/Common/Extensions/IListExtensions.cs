using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.Extensions
{
    /// <summary>
    /// IList<T>関係の拡張メソッド
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// 指定されたIndexに要素が存在するかどうか
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsDefinedAt<T>(this IList<T> source, int index) => index >= 0 && index < source.Count;
    }
}
