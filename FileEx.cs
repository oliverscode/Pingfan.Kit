﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pingfan.Kit
{
    /// <summary>
    /// 文件操作类, 失败时会重试多次
    /// </summary>
    public class FileEx
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        public static int RetryCount { get; set; } = 100;

        /// <summary>
        /// 重试间隔, 单位毫秒
        /// </summary>
        public static int RetryInterval { get; set; } = 50;

        /// <summary>
        /// 读取一个文件, 如果文件不存在会返回null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadAllText(string path)
        {
            return Read(path, () => File.ReadAllText(path));
        }

        /// <summary>
        /// 读取一个文件, 如果文件不存在会返回null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string path)
        {
            return Read(path, () => File.ReadAllBytes(path));
        }

        /// <summary>
        /// 读取一个文件, 如果文件不存在会返回null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] ReadAllLines(string path)
        {
            return Read(path, () => File.ReadAllLines(path));
        }


        /// <summary>
        /// 读取一个文件, 如果文件不存在会返回null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<string> ReadLines(string path)
        {
            return Read(path, () => File.ReadLines(path));
        }


        /// <summary>
        /// 写入文件, 如果文件不存在则创建
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void AppendAllText(string path, string contents)
        {
            Write(path, () => { File.AppendAllText(path, contents, Encoding.UTF8); });
        }

        /// <summary>
        /// 写入文件, 如果文件不存在则创建
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void WriteAllText(string path, string contents)
        {
            Write(path, () => { File.WriteAllText(path, contents, Encoding.UTF8); });
        }


        /// <summary>
        /// 写入文件, 如果文件不存在则创建
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void WriteAllBytes(string path, byte[] contents)
        {
            Write(path, () => { File.WriteAllBytes(path, contents); });
        }

        private static void Write(string path, Action fn)
        {
            // 获取Path的目录
            lock (path)
            {
                Retry.Run(RetryCount, RetryInterval, () =>
                {
                    var dir = Path.GetDirectoryName(path);
                    if (Directory.Exists(dir) == false)
                    {
                        Directory.CreateDirectory(dir);
                    }
                });

                Retry.Run(RetryCount, RetryInterval, fn);
            }
        }

        private static T Read<T>(string path, Func<T> fn) where T : class
        {
            if (File.Exists(path) == false)
            {
                return null;
            }

            lock (path)
            {
                return fn();
            }
        }
    }
}