using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Pingfan.Kit
{
    public class PathEx
    {
        private static string _CurrentDirectory;

        /// <summary>
        /// 当前程序运行的目录
        /// </summary>
        public static string CurrentDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CurrentDirectory))
                {
                    _CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                                        + Path.DirectorySeparatorChar;
                }

                return _CurrentDirectory;
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
            // 其他系统分隔符
            var otherSeparator = separator == '/' ? '\\' : '/';

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
    }
}