using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pingfan.Kit
{
    /// <summary>
    /// 命令行
    /// </summary>
    public static class Cmd
    {
        /// <summary>
        /// 执行命令行, 返回进程, 方便实时读取
        /// </summary>
        /// <returns></returns>
        public static Process Run(string cmd)
        {
            ProcessStartInfo startInfo;
            FileEx.OnError += (eee) => { Console.WriteLine("写文件出错:" + eee); };


            if (ProcessEx.IsWindows)
            {
                startInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + cmd,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.Default, // 设置标准输出编码
                    StandardErrorEncoding = Encoding.Default, // 设置标准错误编码
                };
            }
            else
            {
                startInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + cmd.Replace("\"", "\\\"") + "\"",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.Default, // 设置标准输出编码
                    StandardErrorEncoding = Encoding.Default, // 设置标准错误编码
                };
            }

            var process = new Process
            {
                StartInfo = startInfo
            };
            process.Start();
            process.StandardInput.AutoFlush = true;
            return process;
        }

        /// <summary>
        /// 执行命令行,  返回输出
        /// </summary>
        /// <returns></returns>
        public static string RunWithOutput(string cmd)
        {
            Process? process = null;
            try
            {
                process = Run(cmd);

                try
                {
                    process.StandardInput.WriteLine("exit");
                    process.StandardInput.WriteLine((char)3);
                }
                catch
                {
                    // ignored
                }


                process.WaitForExit();

                var sb = new StringBuilder();
                sb.AppendLine(process.StandardOutput.ReadToEnd());
                sb.AppendLine(process.StandardError.ReadToEnd());
                var output = sb.ToString();
                return output;
            }
            finally
            {
                process?.Kill();
                process?.Dispose();
            }
        }
    }
}