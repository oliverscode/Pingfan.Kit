using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Pingfan.Kit
{
    /// <summary>
    /// 服务管理
    /// </summary>
    static class ServiceManager
    {
        /// <summary>
        /// 安装服务
        /// </summary>
        public static string Install()
        {
            // 获取自身文件路径
            var serverPath = Process.GetCurrentProcess().MainModule!.FileName;
            if (serverPath.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server path");
                return string.Empty;
            }

            // 获取自身文件名, windows下不包含.exe
            var serverName = Path.GetFileNameWithoutExtension(serverPath);
            if (serverName.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server name");
                return string.Empty;
            }

            // 排除install
            var args = Environment.CommandLine;
            args = args.Replace("install", "");
            // 排除开头的前面的自身路径, 后面中的参数不排除
            args = args.Substring(serverPath.Length).TrimStart();


            if (ProcessEx.IsWindows)
            {
                var process = Cmd.Run($"nssm.exe install {serverName} {serverPath} {args}");
                var sr = new StreamReader(process.StandardOutput.BaseStream, Encoding.Unicode);
                var result = sr.ReadToEnd();
                process.WaitForExit();
                process.Dispose();


                Start();
                return result;
            }
            else
            {
                var cmd =
                    $"[Unit]\\nDescription={serverName} Auto Start Service\\nAfter=network.target\\n\\n[Service]\\nExecStart={serverPath}\\nUser=root\\nRestart=on-failure\\nRestartSec=3s\\n\\n[Install]\\nWantedBy=multi-user.target && sudo systemctl daemon-reload && sudo systemctl enable {serverName}";

                var result = Cmd.RunWithOutput($"echo -e \"{cmd}\" > /etc/systemd/system/{serverName}.service");
                Start();
                return result;
            }
        }


        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <returns></returns>
        public static string Remove()
        {
            // 获取自身文件路径
            var serverPath = Process.GetCurrentProcess().MainModule!.FileName;
            if (serverPath.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server path");
                return string.Empty;
            }

            // 获取自身文件名, windows下不包含.exe
            var serverName = Path.GetFileNameWithoutExtension(serverPath);
            if (serverName.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server name");
                return string.Empty;
            }
            
            if (ProcessEx.IsWindows)
            {
                var process = Cmd.Run($"nssm.exe remove {serverName} confirm");
                var sr = new StreamReader(process.StandardOutput.BaseStream, Encoding.Unicode);
                var result = sr.ReadToEnd();
                process.WaitForExit();
                process.Dispose();
                return result;
            }
            else
            {
                return Cmd.RunWithOutput($"systemctl stop {serverName} && rm /etc/systemd/system/{serverName}.service");
            }
        }

        public static string Start()
        {
            // 获取自身文件路径
            var serverPath = Process.GetCurrentProcess().MainModule!.FileName;
            if (serverPath.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server path");
                return string.Empty;
            }

            // 获取自身文件名, windows下不包含.exe
            var serverName = Path.GetFileNameWithoutExtension(serverPath);
            if (serverName.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server name");
                return string.Empty;
            }

            if (ProcessEx.IsWindows)
            {
                var process = Cmd.Run($"nssm.exe start {serverName}");
                var sr = new StreamReader(process.StandardOutput.BaseStream, Encoding.Unicode);
                var result = sr.ReadToEnd();
                process.WaitForExit();
                process.Dispose();
                return result;
            }
            else
            {
                return Cmd.RunWithOutput($"systemctl start {serverName}");
            }
        }

        public static string Stop()
        {
            // 获取自身文件路径
            var serverPath = Process.GetCurrentProcess().MainModule!.FileName;
            if (serverPath.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server path");
                return string.Empty;
            }

            // 获取自身文件名, windows下不包含.exe
            var serverName = Path.GetFileNameWithoutExtension(serverPath);
            if (serverName.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server name");
                return string.Empty;
            }

            if (ProcessEx.IsWindows)
            {
                var process = Cmd.Run($"nssm.exe stop {serverName}");
                var sr = new StreamReader(process.StandardOutput.BaseStream, Encoding.Unicode);
                var result = sr.ReadToEnd();
                process.WaitForExit();
                process.Dispose();
                return result;
            }
            else
            {
                return Cmd.RunWithOutput($"systemctl stop {serverName}");
            }
        }

        public static string ReStart()
        {
            // 获取自身文件路径
            var serverPath = Process.GetCurrentProcess().MainModule!.FileName;
            if (serverPath.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server path");
                return string.Empty;
            }

            // 获取自身文件名, windows下不包含.exe
            var serverName = Path.GetFileNameWithoutExtension(serverPath);
            if (serverName.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server name");
                return string.Empty;
            }

            if (ProcessEx.IsWindows)
            {
                var process = Cmd.Run($"nssm.exe restart {serverName}");
                var sr = new StreamReader(process.StandardOutput.BaseStream, Encoding.Unicode);
                var result = sr.ReadToEnd();
                process.WaitForExit();
                process.Dispose();
                return result;
            }
            else
            {
                return Cmd.RunWithOutput($"systemctl restart {serverName}");
            }
        }

        public static string Status()
        {
            // 获取自身文件路径
            var serverPath = Process.GetCurrentProcess().MainModule!.FileName;
            if (serverPath.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server path");
                return string.Empty;
            }

            // 获取自身文件名, windows下不包含.exe
            var serverName = Path.GetFileNameWithoutExtension(serverPath);
            if (serverName.IsNullOrWhiteSpace())
            {
                Log.Error("Can not get server name");
                return string.Empty;
            }

            if (ProcessEx.IsWindows)
            {
                var process = Cmd.Run($"nssm.exe status {serverName}");
                var sr = new StreamReader(process.StandardOutput.BaseStream, Encoding.Unicode);
                var result = sr.ReadToEnd();
                process.WaitForExit();
                process.Dispose();
                return result;
            }
            else
            {
                return Cmd.RunWithOutput($"systemctl status {serverName}");
            }
        }
    }
}