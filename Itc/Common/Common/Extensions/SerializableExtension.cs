using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace Itc.Common.Extensions
{
    public static class SerializableExtension
    {
        /// <summary>
        /// シリアライズして比較する //※要Serializable属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool ValueEquals<T>(this T self, T other)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new System.ArgumentException();
            }

            var bf = new BinaryFormatter();

            using (var ms_self = new MemoryStream())
            {
                bf.Serialize(ms_self, self);

                using (var ms_other = new MemoryStream())
                {
                    bf.Serialize(ms_other, other);

                    byte[] bytes_self = GetBytes(ms_self);

                    byte[] bytes_other = GetBytes(ms_other);

                    return bytes_self.SequenceEqual(bytes_other);
                }
            }
        }

        /// <summary>
        /// MemoryStreamの内容をbyte[]に変換
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] GetBytes(MemoryStream ms)
        {
            ms.Position = 0;

            byte[] bytes_self = new byte[ms.Length];

            ms.Read(bytes_self, 0, bytes_self.Length);

            return bytes_self;
        }
    }
}
