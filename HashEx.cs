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

        protected static ulong[] Crc32Table;

        //生成CRC32码表
        private static void GetCrc32Table()
        {
            ulong crc;
            Crc32Table = new ulong[256];
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

                Crc32Table[i] = crc;
            }
        }

        public static string Crc32(byte[] buffer)
        {
            int len = buffer.Length;
            ulong value = 0xffffffff;
            for (int i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ buffer[i]];
            }

            return (value ^ 0xffffffff).ToString("x2");
        }

        public static string Crc32(Stream stream)
        {
            var len = stream.Length;
            ulong value = 0xffffffff;
            for (var i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ (byte)stream.ReadByte()];
            }

            return (value ^ 0xffffffff).ToString("x2");
        }

        public static string Crc32(string data)
        {
            var buffer = Encoding.ASCII.GetBytes(data);
            int len = buffer.Length;
            ulong value = 0xffffffff;
            for (int i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ buffer[i]];
            }

            return (value ^ 0xffffffff).ToString("x2");
        }

        public static string Sha256(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            var hash = SHA256.Create().ComputeHash(bytes);

            var sb = new StringBuilder();
            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static string Sha256(Stream stream)
        {
            var hash = SHA256.Create().ComputeHash(stream);
            var sb = new StringBuilder();
            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
        public static string Sha256(byte[] data)
        {
            var hash = SHA256.Create().ComputeHash(data);

            var sb = new StringBuilder();
            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static string Md5(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            return Md5(bytes);
        }

        public static string Md5(Stream stream)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(stream);
            var sb = new StringBuilder();
            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static string Md5(byte[] data)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(data);
            var sb = new StringBuilder();
            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static string Signature(params object[] args)
        {
            var sb = new StringBuilder();
            foreach (var data in args)
            {
                sb.Append("[");
                var key = data.GetType().ToString();
                sb.Append(key);
                sb.Append("]");
                sb.Append("=");
                sb.Append(data);
            }

            return Sha256(sb.ToString());
        }
    }
}