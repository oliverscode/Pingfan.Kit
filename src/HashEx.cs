using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Pingfan.Kit
{
    /// <summary>
    /// 常用的Hash算法
    /// </summary>
    public class HashEx
    {
        static HashEx()
        {
            GetCrc32Table();
        }

        private static ulong[] _crc32Table = null!;

        /// <summary>
        /// 生成CRC32码表
        /// </summary>
        private static void GetCrc32Table()
        {
            ulong crc;
            _crc32Table = new ulong[256];
            int i, j;
            for (i = 0; i < 256; i++)
            {
                crc = (ulong)i;
                for (j = 8; j > 0; j--)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ 0xEDB88320;
                    else
                        crc >>= 1;
                }

                _crc32Table[i] = crc;
            }
        }

        /// <summary>
        /// 计算CRC32哈希值
        /// </summary>
        /// <param name="buffer">要计算哈希的字节数组</param>
        /// <returns>返回CRC32哈希值</returns>
        public static string Crc32(byte[] buffer)
        {
            int len = buffer.Length;
            ulong value = 0xffffffff;
            for (int i = 0; i < len; i++)
            {
                value = (value >> 8) ^ _crc32Table[(value & 0xFF) ^ buffer[i]];
            }

            return (value ^ 0xffffffff).ToString("x2");
        }

        /// <summary>
        /// 计算CRC32哈希值
        /// </summary>
        /// <param name="stream">要计算哈希的数据流</param>
        /// <returns>返回CRC32哈希值</returns>
        public static string Crc32(Stream stream)
        {
            var len = stream.Length;
            ulong value = 0xffffffff;
            for (var i = 0; i < len; i++)
            {
                value = (value >> 8) ^ _crc32Table[(value & 0xFF) ^ (byte)stream.ReadByte()];
            }

            return (value ^ 0xffffffff).ToString("x2");
        }

        /// <summary>
        /// 计算CRC32哈希值
        /// </summary>
        /// <param name="data">要计算哈希的字符串</param>
        /// <returns>返回CRC32哈希值</returns>
        public static string Crc32(string data)
        {
            var buffer = Encoding.ASCII.GetBytes(data);
            int len = buffer.Length;
            ulong value = 0xffffffff;
            for (int i = 0; i < len; i++)
            {
                value = (value >> 8) ^ _crc32Table[(value & 0xFF) ^ buffer[i]];
            }

            return (value ^ 0xffffffff).ToString("x2");
        }

        /// <summary>
        /// 计算SHA256哈希值
        /// </summary>
        /// <param name="data">要计算哈希的字符串</param>
        /// <returns>返回SHA256哈希值</returns>
        public static string Sha256(string data)
        {
            using (var sha256 = SHA256.Create())
            {
                return Sha256(Encoding.ASCII.GetBytes(data), sha256);
            }
        }

        /// <summary>
        /// 计算SHA256哈希值
        /// </summary>
        /// <param name="stream">要计算哈希的数据流</param>
        /// <returns>返回SHA256哈希值</returns>
        public static string Sha256(Stream stream)
        {
            using (var sha256 = SHA256.Create())
            {
                return Sha256(stream, sha256);
            }
        }

        /// <summary>
        /// 计算SHA256哈希值
        /// </summary>
        /// <param name="data">要计算哈希的字节数组</param>
        /// <returns>返回SHA256哈希值</returns>
        public static string Sha256(byte[] data)
        {
            using (var sha256 = SHA256.Create())
            {
                return Sha256(data, sha256);
            }
        }

        private static string Sha256(byte[] data, HashAlgorithm sha256)
        {
            var hash = sha256.ComputeHash(data);
            return ToHexString(hash);
        }

        private static string Sha256(Stream stream, HashAlgorithm sha256)
        {
            var hash = sha256.ComputeHash(stream);
            return ToHexString(hash);
        }

        /// <summary>
        /// 计算MD5哈希值
        /// </summary>
        /// <param name="data">要计算哈希的字符串</param>
        /// <returns>返回MD5哈希值</returns>
        public static string Md5(string data)
        {
            using (var md5 = MD5.Create())
            {
                return Md5(Encoding.ASCII.GetBytes(data), md5);
            }
        }

        /// <summary>
        /// 计算MD5哈希值
        /// </summary>
        /// <param name="stream">要计算哈希的数据流</param>
        /// <returns>返回MD5哈希值</returns>
        public static string Md5(Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                return Md5(stream, md5);
            }
        }

        /// <summary>
        /// 计算MD5哈希值
        /// </summary>
        /// <param name="data">要计算哈希的字节数组</param>
        /// <returns>返回MD5哈希值</returns>
        public static string Md5(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                return Md5(data, md5);
            }
        }

        private static string Md5(byte[] data, HashAlgorithm md5)
        {
            var hash = md5.ComputeHash(data);
            return ToHexString(hash);
        }

        private static string Md5(Stream stream, HashAlgorithm md5)
        {
            var hash = md5.ComputeHash(stream);
            return ToHexString(hash);
        }

        private static string ToHexString(byte[] hash)
        {
            var sb = new StringBuilder();
            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 对参数进行签名
        /// </summary>
        /// <param name="args">要签名的参数</param>
        /// <returns>签名结果</returns>
        public static string Signature(params object[] args)
        {
            var toEncryptArray = Encoding.ASCII.GetBytes(string.Join("", args));
            using (var sha256 = SHA256.Create())
            {
                var resultArray = sha256.ComputeHash(toEncryptArray);
                return ToHexString(resultArray);
            }
        }
    }
}