using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Itc.Common.Extensions
{
    /// <summary>
    /// 列挙子用拡張メソッド
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 属性毎にキャッシュを作るためのジェネリッククラス
        /// </summary>
        /// <remarks>クラス内でジェネリッククラスを定義しているのは、拡張メソッドをジェネリッククラスで定義できないから。</remarks>
        /// <typeparam name="TAttribute"></typeparam>
        internal class EnumAttibuteCache<TAttribute>
            where TAttribute : Attribute
        {
            private static ConcurrentDictionary<Enum, TAttribute> body = new ConcurrentDictionary<Enum, TAttribute>();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <param name="valueFactory"></param>
            /// <returns></returns>
            internal static TAttribute GetOrAdd(Enum key, Func<Enum, TAttribute> valueFactory) => body.GetOrAdd(key, valueFactory);
        }

        /// <summary>
        /// 特定の属性を取得する
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum key)
            where TAttribute : Attribute => EnumAttibuteCache<TAttribute>.GetOrAdd(key, k => k.GetAttributeCore<TAttribute>());

        /// <summary>
        /// リフレクションを利用して特定の属性を取得する
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="enumKey"></param>
        /// <returns></returns>
        internal static TAttribute GetAttributeCore<TAttribute>(this Enum enumKey) 
            where TAttribute : Attribute
        {
            //リフレクションを用いて列挙体の型から情報を取得
            System.Reflection.FieldInfo fieldInfo = enumKey.GetType().GetField(enumKey.ToString());

            //指定した属性のリスト
            IEnumerable<TAttribute> attributes
                = fieldInfo.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();

            //属性がなかった場合、nullを返す
            //同じ属性が複数含まれていても、最初のみ返す
            return attributes?.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToEnumerable<T>(this Type type)
        {
            foreach (T value in Enum.GetValues(type))
            {
                yield return value;
            }
        }
    }
}
