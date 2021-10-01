using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.IO;

namespace Itc.Common
{

    /// <summary>
    /// AESを用いた暗号化・復号化を行う
    /// </summary>
    public class Cipher: ICipher
    {
        RijndaelManaged Rijndael { get; set; }

        const int default_keysize = 128;

        const int default_blocksize = 128;

        const int iterations = 10000;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        public Cipher(string password, byte[] salt)
            : this(password, salt, default_keysize, default_blocksize) { }


        protected Cipher(string password, byte[] salt, int keysize, int blocksize)
        {
            Rijndael = new RijndaelManaged
            {
                KeySize = keysize,
                BlockSize = blocksize
            };

            var deriveBytes = new Rfc2898DeriveBytes(password, salt)
            {
                IterationCount = iterations,
            };

            Rijndael.Key = deriveBytes.GetBytes(Rijndael.KeySize / 8);
            Rijndael.IV = deriveBytes.GetBytes(Rijndael.BlockSize / 8);
        }

        /// <summary>
        /// 暗号化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data)
        {
            using (ICryptoTransform encryptor = Rijndael.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }

        /// <summary>
        /// 復号化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] data)
        {
            using (ICryptoTransform decryptor = Rijndael.CreateDecryptor())
            {
                return decryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }

        /// <summary>
        /// SALT生成
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GenerateSalt(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);

            using(var provider = new SHA256CryptoServiceProvider())
            {
                return provider.ComputeHash(data);
            }
        }
    }
}
