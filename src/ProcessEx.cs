using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Pingfan.Kit
{
    /// <summary>
    /// 进程扩展
    /// </summary>
    public static class ProcessEx
    {
        private static readonly string MutexName = $"{Assembly.GetEntryAssembly()!.FullName}_{Environment.UserInteractive}";
        private static Mutex? _mutex;

        /// <summary>
        /// 本程序是否运行过
        /// </summary>
        public static bool HasRun
        {
            get
            {
                _mutex = new Mutex(true, MutexName, out var createdNew);
                if (createdNew)
                {
                    // Don't release the mutex until the application is closed
                    return false;
                }

                // Close the mutex if it's already been created
                _mutex.Close();
                return true;
            }
        }

#if net48 || NETCOREAPP
        
        /// <summary>
        /// 是否是管理员运行, 如果是linux则判断是否是root用户
        /// </summary>
        public static bool IsAdmin
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "cmd.exe",
                        Arguments = "/C net session",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    try
                    {
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();
                        return process.ExitCode == 0;
                    }
                    catch
                    {
                        return false;
                    }
                    finally
                    {
                        process.Kill();
                    }
                }
                else
                {
                    return Environment.UserName == "root";
                }
            }
        }
#endif
    }
}