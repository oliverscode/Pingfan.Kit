using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pingfan.Kit
{
    /// <summary>
    /// 文件操作类, 失败时会重试多次
    /// </summary>
    public static class FileEx
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        public static int RetryCount { get; set; } = 20;

        /// <summary>
        /// 重试间隔, 单位毫秒
        /// </summary>
        public static int RetryInterval { get; set; } = 100;

        /// <summary>
        /// 读取一个文件, 如果文件不存在会返回null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadAllText(string path, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            return Read(path, () => File.ReadAllText(path));
        }

        /// <summary>
        /// 读取一个文件, 如果文件不存在会返回byte[0]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string path)
        {
            return Read(path, () => File.ReadAllBytes(path)) ?? Array.Empty<byte>();
        }

        /// <summary>
        /// 读取一个文件, 如果文件不存在会返回string[]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] ReadAllLines(string path, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            return Read(path, () => File.ReadAllLines(path, encoding)) ?? Array.Empty<string>();
        }

        /// <summary>
        /// 读取一个文件, 如果文件不存在会返回string[]
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<string> ReadLines(string path, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            return Read(path, () => File.ReadLines(path, encoding)) ?? Array.Empty<string>();
        }

        /// <summary>
        /// 写入文件, 如果文件不存在则创建
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void AppendAllText(string path, string contents, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            Write(path, () => File.AppendAllText(path, contents, encoding));
        }

        /// <summary>
        /// 写入文件, 如果文件不存在则创建
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void WriteAllText(string path, string contents, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            Write(path, () => File.WriteAllText(path, contents, encoding));
        }

        /// <summary>
        /// 写入文件, 如果文件不存在则创建
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void WriteAllBytes(string path, byte[] contents)
        {
            Write(path, () => File.WriteAllBytes(path, contents));
        }

        private static void Write(string path, Action fn)
        {
            // 获取Path的目录
            lock (path)
            {
                var dir = Path.GetDirectoryName(path);
                Retry.Run(RetryCount, RetryInterval, () =>
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                });

                Retry.Run(RetryCount, RetryInterval, fn);
            }
        }

        /// <summary>
        /// 删除一个文件, 或者目录
        /// </summary>
        /// <param name="path"></param>
        public static bool Delete(string path)
        {
            lock (path)
            {
                return Retry.Run(RetryCount, RetryInterval, () =>
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        return true;
                    }

                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                        return true;
                    }

                    return false;
                });
            }
        }

        private static T Read<T>(string path, Func<T> fn) where T : class
        {
            if (!File.Exists(path))
            {
                return null;
            }

            lock (path)
            {
                return Retry.Run(RetryCount, RetryInterval, fn);
            }
        }
    }
}