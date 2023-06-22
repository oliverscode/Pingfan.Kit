﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Pingfan.Kit
{
    public static class PathEx
    {
        private static string _currentDirectory;

        /// <summary>
        /// 当前程序运行的目录
        /// </summary>
        public static string CurrentDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_currentDirectory))
                {
                    _currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                                        + Path.DirectorySeparatorChar;
                }

                return _currentDirectory;
            }
        }

        /// <summary>
        /// 从当前目录拼凑绝对路径
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string CombineFromCurrentDirectory(params string[] paths)
        {
            // 把当前目录加入到路径中
            var ps = new string[paths.Length + 1];
            ps[0] = CurrentDirectory;
            Array.Copy(paths, 0, ps, 1, paths.Length);
            return Path.Combine(ps);
        }

        /// <summary>
        /// 按照当前系统, 拼接绝对路径, 不会以分隔符结尾
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
            // 当前系统的目录分隔符
            var separator = Path.DirectorySeparatorChar;

            var ps = new List<string>();
            foreach (var path in paths)
            {
                var p = path.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                ps.AddRange(p);
            }

            var result = string.Join(separator.ToString(), ps);

            // 不是windows系统, 路径必须是当前系统分隔符开头
#if NETCOREAPP
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
            {
                result = separator + result;
            }
#endif

            return result;
        }

        /// <summary>
        /// 按windows系统整理路径, 也就是\分隔符
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FormatWindows(string path)
        {
            path = Regex.Replace(path, @"(\\+)", @"\");
            path = Regex.Replace(path, @"(/+)", @"/");

            return string.Join("\\", path.Split(new char[] { '\\', '/' }, StringSplitOptions.None));
        }

        /// <summary>
        /// 按unix系统整理路径, 也就是/分隔符
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FormatUnix(string path)
        {
            path = Regex.Replace(path, @"(\\+)", @"\");
            path = Regex.Replace(path, @"(/+)", @"/");

            return string.Join("/", path.Split(new char[] { '\\', '/' }, StringSplitOptions.None));
        }


        /// <summary>
        /// 如果目录不存在就创建这个目录
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectoryIfNotExists(string path)
        {
            var dir = Path.GetDirectoryName(path);
            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}