using System;
using System.IO;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;

namespace Pingfan.Kit
{
    public class Crypt
    {
        /// <summary>
        /// DES密钥
        /// </summary>
        private static byte[] DESKey = Encoding.ASCII.GetBytes("likhhdib");

        /// <summary>
        /// DES偏移量
        /// </summary>
        private static byte[] DESIV = Encoding.ASCII.GetBytes("6j5f1n4g");

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="buffer"></param>
        public static byte[] DesEncrypt(byte[] buffer)
        {
            var des = new DESCryptoServiceProvider();
            des.Key = DESKey;
            des.IV = DESIV;
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(buffer, 0, buffer.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }

                buffer = ms.ToArray();
                ms.Close();
            }

            return buffer;
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] DesDecrypt(byte[] buffer)
        {
            var des = new DESCryptoServiceProvider();
            des.Key = DESKey;
            des.IV = DESIV;
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(buffer, 0, buffer.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }

                buffer = ms.ToArray();
                ms.Close();
            }

            return buffer;
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DesEncrypt(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            buffer = DesEncrypt(buffer);
            var result = System.Convert.ToBase64String(buffer);
            return result;
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DesDecrypt(string str)
        {
            var buffer = System.Convert.FromBase64String(str);
            buffer = DesDecrypt(buffer);
            var result = Encoding.UTF8.GetString(buffer);
            return result;
        }
    }
}