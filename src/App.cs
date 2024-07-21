using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Pingfan.Kit.Inject;

namespace Pingfan.Kit
{
    /// <summary>
    /// 应用程序封装
    /// </summary>
    public class App
    {
        /// <summary>
        /// 未处理的异常
        /// </summary>
        public static Action<Exception> OnError { get; set; } = (e) => { Log.Fatal(e.ToString()); };

        /// <summary>
        /// 根容器
        /// </summary>
        public static IContainer Container { get; private set; } = new Container();

        /// <summary>
        /// App的名字
        /// </summary>
        public static string AppName { get; set; } = "App";


        /// <summary>
        /// 初始化, 捕获全局异常, 如果有异常会记录日志并退出程序, 同时支持命令行参数进行安装, 卸载, 启动, 停止, 重启, 状态
        /// </summary>
        public static void Init(string? title = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls12
                                                   | SecurityProtocolType.Tls13;
            


            if (title != null)
            {
                Console.Title = title;
                // 获取当前启动程序集名字
                var assemblyName = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;
                AppName = assemblyName ?? "App";
            }


            // 捕获当前程序的全局异常
            CatchGlobalException();

            var args = Environment.CommandLine;
            // 卸载服务
            if (args.ContainsIgnoreCase(" uninstall") || args.ContainsIgnoreCase(" remove") ||
                args.ContainsIgnoreCase(" disable"))
            {
                Log.Info(ServiceManager.Remove());
                Environment.Exit(0);
                return;
            }

            // 安装服务
            if (args.Contains(" install") || args.ContainsIgnoreCase(" enable"))
            {
                Log.Info(ServiceManager.Install());
                Environment.Exit(0);
                return;
            }


            if (args.ContainsIgnoreCase(" start"))
            {
                Log.Info(ServiceManager.Start());
                Environment.Exit(0);
                return;
            }

            if (args.ContainsIgnoreCase(" stop"))
            {
                Log.Info(ServiceManager.Stop());
                Environment.Exit(0);
                return;
            }

            if (args.ContainsIgnoreCase(" restart"))
            {
                Log.Info(ServiceManager.ReStart());
                Environment.Exit(0);
                return;
            }

            if (args.ContainsIgnoreCase(" status"))
            {
                Log.Info(ServiceManager.Status());
                Environment.Exit(0);
                return;
            }
        }


        private static void CatchGlobalException()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                var ex = (Exception)eventArgs.ExceptionObject;
                OnError(ex);
                Environment.Exit(1);
            };
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                var ex = args.Exception;
                OnError(ex);
                Environment.Exit(1);
            };
        }

        /// <summary>
        /// 启动指定类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Run<T>() where T : class
        {
            Container.New<T>();
            Run();
        }


        /// <summary>
        /// 启动APP, 并且加载plugins目录下的所有dll, dll可以是标准的dotnet程序集, 也可以是类库, 会自动调用Main或者PluginStart方法
        /// 但必须是静态且公开的方法
        /// </summary>
        public static void Run()
        {
            // 判断当前目录下是否有plugins目录, 如果有则加载
            var pluginsDir = PathEx.CombineCurrentDirectory("plugins");
            if (Directory.Exists(pluginsDir))
            {
                var files = Directory.GetFiles(pluginsDir, "*.dll");
                foreach (var file in files)
                {
                    try
                    {
                        var assembly = Assembly.LoadFile(file);
                        // 获取assembly的入口并且调用
                        var types = assembly.GetTypes();
                        foreach (var type in types)
                        {
                            var mainMethod = type.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);
                            if (mainMethod != null)
                            {
                                Task.Run(() => { mainMethod.Invoke(null, null); });
                                goto Start;
                            }

                            // 如果没有Main方法, 则调用PluginStart方法
                            var pluginStartMethod =
                                type.GetMethod("PluginStart", BindingFlags.Public | BindingFlags.Static);
                            if (pluginStartMethod != null)
                            {
                                Task.Run(() => { pluginStartMethod.Invoke(null, null); });
                                goto Start;
                            }
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            Start:

            Log.Debug($"{AppName} is Running...");
            Loop.Wait();
        }
    }
}

//
// namespace Pingfan.Kit
// {
//     /// <summary>
//     /// 应用程序封装
//     /// </summary>
//     public class App
//     {
//         private AppInfo _appInfo;
//
//
//         /// <summary>
//         /// 需要更新的文件列表
//         /// </summary>
//         public List<string> UpdateManifest { get; set; } = new List<string>();
//
//         public App(AppInfo appInfo)
//         {
//             this._appInfo = appInfo;
//             if (this._appInfo.CheckUpdateInterval > 0)
//             {
//                 Timer.SetIntervalWithTry(this._appInfo.CheckUpdateInterval * 1000, this.CheckUpdate);
//             }
//         }
//
//         /// <summary>
//         /// 检测更新
//         /// </summary>
//         public bool CheckUpdate()
//         {
//             var url = $"{this._appInfo.ServerUrl}{this._appInfo.UId}/manifest.txt";
//             var manifest = Http.Get(url);
//             if (manifest.IsNullOrWhiteSpace())
//             {
//                 Log.Error("无法获取到更新信息");
//                 return false;
//             }
//
//             var manifestLines = manifest.Split("\r", "\n");
//             var dict = new Dictionary<string, string>();
//             foreach (var line in manifestLines)
//             {
//                 var kv = line.Split("=");
//                 if (kv.Length == 2)
//                 {
//                     dict[kv[0]] = kv[1];
//                 }
//             }
//
//
//             UpdateManifest.Clear();
//             // 分析本地文件是否一致
//             foreach (var key in dict.Keys)
//             {
//                 var remoteCrc32 = dict[key];
//
//                 var localFile = PathEx.CombineFromCurrentDirectory(key);
//                 using var fs = File.Open(localFile, FileMode.Open, FileAccess.Read, FileShare.Read);
//                 var localCrc32 = HashEx.Crc32(fs);
//                 if (localCrc32 != remoteCrc32)
//                     UpdateManifest.Add(key);
//             }
//
//
//             return UpdateManifest.Count > 0;
//         }
//
//         /// <summary>
//         /// 更新执行更新
//         /// </summary>
//         public void DoUpdate()
//         {
//             if (UpdateManifest.Count <= 0)
//                 return;
//
//             StringBuilder cmdSb = new StringBuilder();
//             foreach (var key in UpdateManifest)
//             {
//                 
//             }
//         }
//
//
//         /// <summary>
//         /// 捕获全局异常
//         /// </summary>
//         public void CatchGlobalException()
//         {
//             AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
//             {
//                 var ex = (Exception)args.ExceptionObject;
//                 UnHandledException(ex);
//             };
//             TaskScheduler.UnobservedTaskException += (sender, args) =>
//             {
//                 var ex = args.Exception;
//                 UnHandledException(ex);
//             };
//         }
//
//         /// <summary>
//         /// 未处理的异常实际调用的方法
//         /// </summary>
//         /// <param name="exception"></param>
//         public void UnHandledException(Exception exception)
//         {
//         }
//     }
//
//     /// <summary>
//     /// 应用程序信息
//     /// </summary>
//     public class AppInfo
//     {
//         /// <summary>
//         /// 唯一标识
//         /// </summary>
//         public string UId { get; set; }
//
//         /// <summary>
//         /// 作者名称
//         /// </summary>
//         public string Author { get; set; }
//
//         /// <summary>
//         /// 服务器地址
//         /// </summary>
//         public string ServerUrl { get; set; } = "https://update.yplpf.com/";
//
//         /// <summary>
//         /// 检测更新的间隔时间, 单位: 秒
//         /// </summary>
//         public int CheckUpdateInterval { get; set; } = 60;
//
//         /// <summary>
//         /// 更新前执行的脚本
//         /// <code>sc stop xxxx</code>
//         /// <code>systemctl stop xxxx</code>
//         /// </summary>
//         public string UpdateBeforeShell { get; set; }
//
//         /// <summary>
//         /// 更新后执行的脚本
//         /// <code>sc start xxxx</code>
//         /// <code>systemctl start xxxx</code>
//         /// </summary>
//         public string UpdateAfterShell { get; set; }
//     }
// }