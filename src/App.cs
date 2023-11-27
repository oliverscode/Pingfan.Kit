// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Text;
// using System.Threading.Tasks;
//
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