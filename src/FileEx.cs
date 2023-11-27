using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Pingfan.Kit
{
    /// <summary>
    /// 文件操作类, 失败时会重试多次
    /// </summary>
    public static class FileEx
    {
        private static readonly ConcurrentDictionary<string, object> Locks = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 错误事件
        /// </summary>
        public static event Action<Exception>? OnError;

        /// <summary>
        /// 重试次数
        /// </summary>
        public static int RetryCount { get; set; } = 10;

        /// <summary>
        /// 重试间隔, 单位毫秒
        /// </summary>
        public static int RetryInterval { get; set; } = 100;

        private static void RunWithRetry(Action action)
        {
            var attempts = 0;

            while (true)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception e)
                {
                    if (++attempts == RetryCount)
                        OnError?.Invoke(e);

                    Thread.Sleep(RetryInterval);
                }
            }
        }

        private static T RunWithRetry<T>(Func<T> action)
        {
            var attempts = 0;

            while (true)
            {
                try
                {
                    return action();
                }
                catch (Exception e)
                {
                    if (++attempts == RetryCount)
                        OnError?.Invoke(e);

                    Thread.Sleep(RetryInterval);
                }
            }
        }

        /// <summary>
        /// 读取一个文件
        /// </summary>
        public static string ReadAllText(string path, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var lockObject = Locks.GetOrAdd(path, _ => new object());

            lock (lockObject)
            {
                return RunWithRetry(() => File.ReadAllText(path, encoding));
            }
        }

        /// <summary>
        /// 读取一个文件的所有行
        /// </summary>
        public static string[] ReadAllLines(string path, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var lockObject = Locks.GetOrAdd(path, _ => new object());

            lock (lockObject)
            {
                return RunWithRetry(() => File.ReadAllLines(path, encoding));
            }
        }

        /// <summary>
        /// 读取一个文件的所有行
        /// </summary>
        public static IEnumerable<string> ReadLines(string path, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var lockObject = Locks.GetOrAdd(path, _ => new object());

            lock (lockObject)
            {
                return RunWithRetry(() => File.ReadLines(path, encoding));
            }
        }

        /// <summary>
        /// 写入文本到一个文件
        /// </summary>
        public static void WriteAllText(string path, string contents, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var lockObject = Locks.GetOrAdd(path, _ => new object());

            lock (lockObject)
            {
                RunWithRetry(() =>
                {
                    PathEx.CreateDirectoryIfNotExists(path);
                    File.WriteAllText(path, contents, encoding);
                });
            }
        }

        /// <summary>
        /// 写入多行文本到一个文件
        /// </summary>
        public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var lockObject = Locks.GetOrAdd(path, _ => new object());

            lock (lockObject)
            {
                RunWithRetry(() =>
                {
                    PathEx.CreateDirectoryIfNotExists(path);
                    File.WriteAllLines(path, contents, encoding);
                });
            }
        }

        /// <summary>
        /// 读取一个文件的所有字节
        /// </summary>
        public static byte[] ReadAllBytes(string path)
        {
            var lockObject = Locks.GetOrAdd(path, _ => new object());

            lock (lockObject)
            {
                return RunWithRetry(() => File.ReadAllBytes(path));
            }
        }

        /// <summary>
        /// 写入字节数组到一个文件
        /// </summary>
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            var lockObject = Locks.GetOrAdd(path, _ => new object());

            lock (lockObject)
            {
                RunWithRetry(() =>
                {
                    PathEx.CreateDirectoryIfNotExists(path);
                    File.WriteAllBytes(path, bytes);
                });
            }
        }

        /// <summary>
        /// 追加文本到一个文件
        /// </summary>
        public static void AppendAllText(string path, string contents, Encoding? encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            var lockObject = Locks.GetOrAdd(path, _ => new object());

            lock (lockObject)
            {
                RunWithRetry(() =>
                {
                    PathEx.CreateDirectoryIfNotExists(path);
                    File.AppendAllText(path, contents, encoding);
                });
            }
        }

        /// <summary>
        /// 删除一个文件
        /// </summary>
        /// <param name="path"></param>
        public static bool Delete(string path)
        {
            var lockObject = Locks.GetOrAdd(path, _ => new object());
            lock (lockObject)
            {
                RunWithRetry(() =>
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                });
            }

            return File.Exists(path);
        }
    }
}