using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Pingfan.Kit
{
    /// <summary>
    /// 服务管理
    /// </summary>
    public static class ServiceManager
    {
        static ServiceManager()
        {
            // 判断是否是管理员
            if (!ProcessEx.IsAdmin)
            {
                Log.Fatal("Please run as administrator");
            }
        }

        /// <summary>
        /// 安装服务
        /// </summary>
        public static string Install()
        {
            GetServerInfo(out var serverPath, out var serverName);

            // 排除install
            var args = Environment.CommandLine;
            args = args.Replace("install", "");
            // 排除开头的前面的自身路径, 后面中的参数不排除
            args = args.Substring(serverPath.Length).TrimStart();


            if (ProcessEx.IsWindows)
            {
                return RunCommand($"nssm.exe install {serverName} {serverPath} {args}");
            }
            else
            {
                var cmd =
                    $"[Unit]\\nDescription={serverName} Auto Start Service\\nAfter=network.target\\n\\n[Service]\\nExecStart={serverPath}\\nUser=root\\nRestart=on-failure\\nRestartSec=3s\\n\\n[Install]\\nWantedBy=multi-user.target && sudo systemctl daemon-reload && sudo systemctl enable {serverName}";
                return Cmd.RunWithOutput($"echo -e \"{cmd}\" > /etc/systemd/system/{serverName}.service");
            }
        }


        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <returns></returns>
        public static string Remove()
        {
            GetServerInfo(out var serverPath, out var serverName);

            if (ProcessEx.IsWindows)
            {
                return RunCommand($"nssm.exe remove {serverName} confirm");
            }
            else
            {
                return Cmd.RunWithOutput($"systemctl stop {serverName} && rm /etc/systemd/system/{serverName}.service");
            }
        }

        public static string Start()
        {
            GetServerInfo(out var serverPath, out var serverName);

            if (ProcessEx.IsWindows)
            {
                return RunCommand($"nssm.exe start {serverName}");
            }
            else
            {
                return Cmd.RunWithOutput($"systemctl start {serverName}");
            }
        }

        public static string Stop()
        {
            GetServerInfo(out var serverPath, out var serverName);

            if (ProcessEx.IsWindows)
            {
                return RunCommand($"nssm.exe stop {serverName}");
            }
            else
            {
                return Cmd.RunWithOutput($"systemctl stop {serverName}");
            }
        }

        public static string ReStart()
        {
            GetServerInfo(out var serverPath, out var serverName);

            if (ProcessEx.IsWindows)
            {
                return RunCommand($"nssm.exe restart {serverName}");
            }
            else
            {
                return Cmd.RunWithOutput($"systemctl restart {serverName}");
            }
        }

        public static string Status()
        {
            GetServerInfo(out var serverPath, out var serverName);

            if (ProcessEx.IsWindows)
            {
                return RunCommand($"nssm.exe status {serverName}");
            }
            else
            {
                return Cmd.RunWithOutput($"systemctl status {serverName}");
            }
        }

        private static void GetServerInfo(out string serverPath, out string serverName)
        {
            serverPath = Process.GetCurrentProcess().MainModule!.FileName!;
            if (serverPath.IsNullOrWhiteSpace())
            {
                throw new Exception("Can not get server path");
            }

            serverName = AppDomain.CurrentDomain.FriendlyName;
            if (serverName.IsNullOrWhiteSpace())
            {
                throw new Exception("Can not get server name");
            }
        }

        private static string RunCommand(string command)
        {
            // 判断当前目录是否有文件nssm.exe
            if (!File.Exists("nssm.exe"))
            {
                throw new Exception("Can not find nssm.exe");
            }

            var process = Cmd.Run(command);
            var sr = new StreamReader(process.StandardOutput.BaseStream, Encoding.Unicode);
            var result = sr.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            return result;
        }
    }
}