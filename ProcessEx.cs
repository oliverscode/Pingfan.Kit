using System;
using System.Reflection;
using System.Threading;

namespace Pingfan.Kit
{
    public static class ProcessEx
    {
        private static string MutexName = $"{Assembly.GetEntryAssembly().FullName}_{Environment.UserInteractive}";

        public static bool HasRun
        {
            get
            {
                // 尝试打开Mutex
                bool createdNew;
                using (var mutex = new Mutex(true, MutexName, out createdNew))
                {
                    if (createdNew)
                    {
                        // Mutex不存在，程序之前没有运行过
                        mutex.ReleaseMutex();
                        return false;
                    }
                }

                // Mutex已存在，程序已经运行过
                return true;
            }
        }
    }

}