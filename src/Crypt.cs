using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
#pragma warning disable SYSLIB0021

namespace Pingfan.Kit
{
    /// <summary>
    /// 可能不够安全的加密解密, 建议使用AesCrypt
    /// </summary>
    public static class DesCrypt
    {
        /// <summary>
        /// DES密钥, 长度必须8位
        /// </summary>
        public static readonly byte[] Key = new byte[] { 0x6C, 0x69, 0x6B, 0x68, 0x68, 0x64, 0x69, 0x62 };

        /// <summary>
        /// DES偏移量 6j5f1n4g, 长度必须8位
        /// </summary>
        public static readonly byte[] Iv = new byte[] { 54, 106, 53, 102, 49, 110, 52, 103 };


        /// <summary>
        /// DES加密
        /// </summary>
        public static byte[] Encrypt(byte[] buffer)
        {
            var des = new DESCryptoServiceProvider
            {
                Key = Key,
                IV = Iv,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
                cs.FlushFinalBlock();
            }

            buffer = ms.ToArray();

            return buffer;
        }

        /// <summary>
        /// DES解密
        /// </summary>
        public static byte[] Decrypt(byte[] buffer)
        {
            var des = new DESCryptoServiceProvider
            {
                Key = Key,
                IV = Iv,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
                cs.FlushFinalBlock();
            }

            buffer = ms.ToArray();

            return buffer;
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        public static string Encrypt(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            buffer = Encrypt(buffer);
            var result = Convert.ToBase64String(buffer);
            return result;
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        public static string Decrypt(string str)
        {
            var buffer = Convert.FromBase64String(str);
            buffer = Decrypt(buffer);
            var result = Encoding.UTF8.GetString(buffer);
            return result;
        }
    }

    /// <summary>
    /// 比较安全的加密解密
    /// </summary>
    public static class AesCrypt
    {
        /// <summary>
        /// 密钥长度必须是 16、24 或 32 位
        /// </summary>
        public static readonly byte[] Key = new byte[]
            { 0x8B, 0x4A, 0x2E, 0x3D, 0x78, 0x4F, 0x2C, 0x67, 0x1A, 0xAB, 0x19, 0x59, 0xE0, 0x7F, 0x1F, 0x88 };

        /// <summary>
        /// 偏移量长度必须是 16 位
        /// </summary>
        public static readonly byte[] Iv = new byte[]
            { 0xA3, 0x6B, 0x7D, 0x45, 0x50, 0xC4, 0x8A, 0x90, 0xC6, 0x1D, 0x04, 0x9D, 0x3B, 0x13, 0x16, 0x4C };

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] buffer)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = Iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
                cs.FlushFinalBlock();
            }

            buffer = ms.ToArray();

            return buffer;
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] buffer)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = Iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
                cs.FlushFinalBlock();
            }

            buffer = ms.ToArray();

            return buffer;
        }

        /// <summary>
        /// AES 加密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encrypt(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            buffer = Encrypt(buffer);
            var result = Convert.ToBase64String(buffer);
            return result;
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decrypt(string str)
        {
            var buffer = System.Convert.FromBase64String(str);
            buffer = Decrypt(buffer);
            var result = Encoding.UTF8.GetString(buffer);
            return result;
        }
    }
}