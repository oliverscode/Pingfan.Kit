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
        /// 遍历文件夹, 同时不卡顿, 自动忽略权限不足的文件夹
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


                // 便利子目录
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
    }
}