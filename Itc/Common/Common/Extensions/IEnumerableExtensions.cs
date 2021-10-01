using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.Extensions
{
    /// <summary>
    /// IEnumrable<T>関係の拡張メソッド
    /// </summary>
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// IEnumerable<T>でForEachを使用する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// CastしてからSelectを１メソッドで実行する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static IEnumerable<T> Convert<T>(this System.Collections.IEnumerable source, Func<object, T> converter)
        {
            foreach (object item in source)
            {
                yield return converter(item);
            }
        }

        /// <summary>
        /// 複数のシーケンスを連結する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, params T[] second) => Enumerable.Concat(first, second); //first.Concat(second)と書くと、StackOverflowExceptionとなる

        /// <summary>
        /// 要素を規定数で纏める
        /// </summary>
        /// <remarks> = Chunk </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Buffer<T>(this IEnumerable<T> source, int count)
        {
            var result = new List<T>(count);

            foreach (T item in source)
            {
                //規定数Listでまとめる
                result.Add(item);

                if (result.Count == count)
                {
                    yield return result;

                    result = new List<T>(count);
                }
            }

            //端数を返す
            if (result.Count != 0)
            {
                yield return result.ToArray();
            }
        }

        /// <summary>
        /// Index付のアイテムに纏める
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [Obsolete] //C#7.0以降でTupleが使えるため
        public static IEnumerable<IndexedItem<T>> WithIndex<T>(this IEnumerable<T> source) => source.Select((x, i) => new IndexedItem<T>(x, i));

        /// <summary>
        /// Index付のアイテムに纏める
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [Obsolete] //C#7.0以降でTupleが使えるため
        public static IEnumerable<IndexedItem2<T>> WithIndex<T>(this T[,] source)
        {
            for (int x = 0; x < source.GetLength(0); ++x)
            {
                for (int y = 0; y < source.GetLength(1); ++y)
                {
                    yield return new IndexedItem2<T>(source[x, y], new int[] { x, y });
                }
            }
        }

        /// <summary>
        /// ずらしてまとめる
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="shift">ずらす要素数。１以上であること</param>
        /// <returns></returns>
        [Obsolete] //C#7.0以降でTupleが使えるため
        public static IEnumerable<Tuple<T, T>> ShiftZip<T>(this IEnumerable<T> m, int shift = 1)
        {
            if (shift <= 0) throw new ArgumentOutOfRangeException();

            int count = m.Count();

            IEnumerable<T> first = m.Take(count - shift);

            IEnumerable<T> second = m.Skip(shift);

            return !first.Any() || !second.Any()
                ? Enumerable.Empty<Tuple<T, T>>()
                : Enumerable.Zip(first, second, (f, s) => new Tuple<T, T>(f, s));
        }
    }
   
    /// <summary>
    /// Index付のアイテム
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IndexedItem<T>
    {
        public T Element { get; private set; }

        public int Index { get; private set; }

        public IndexedItem(T element, int index)
        {
            this.Element = element;

            this.Index = index;
        }
    }

    //多次元用
    public class IndexedItem2<T>
    {
        public T Element { get; private set; }

        public int[] Index { get; private set; }

        public IndexedItem2(T element, int[] index)
        {
            this.Element = element;
            this.Index = index;
        }
    }
}
