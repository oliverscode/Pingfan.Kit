using System.Collections.Generic;
using System.IO;

namespace Pingfan.Kit
{
    /// <summary>
    /// 目录操作
    /// </summary>
    public static class DirectoryEx
    {
        /// <summary>
        /// 遍历文件夹不卡顿, 自动忽略权限不足的文件夹, 返回相对路径
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetFiles(
            string path,
            string searchPattern = "*"
        )
        {
            var queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                var dir = queue.Dequeue();

                string[]? files = null;
                try
                {
                    files = Directory.GetFiles(dir, searchPattern);
                }
                catch
                {
                    //
                }

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        yield return file;
                    }
                }


                // 遍历子目录
                string[]? subDirs = null;
                try
                {
                    subDirs = Directory.GetDirectories(dir);
                }
                catch
                {
                    //
                }

                if (subDirs == null) continue;
                foreach (var subDir in subDirs)
                {
                    queue.Enqueue(subDir);
                }
            }
        }
        
        
        /// <summary>
        /// 判断当前目录是否存在
        /// </summary>
        public static bool CurrentExists(string path)
        {
            var dir = PathEx.CombineCurrentDirectory(path);
            return Directory.Exists(dir);
        }
        
        
    }
}