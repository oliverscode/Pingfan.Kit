// using System;
//
//
// namespace Pingfan.Kit
// {
// #if net48 || NETCOREAPP
//     using LightInject;
//
//     /// <summary>
//     /// 默认单例注入
//     /// </summary>
//     public class App
//     {
//         private static readonly IServiceContainer Container = new ServiceContainer();
//
//         static App()
//         {
//             Container.SetDefaultLifetime<PerContainerLifetime>();
//
//             ConfigDefaultService();
//         }
//         
//         private static void ConfigDefaultService()
//         {
//             // 注册日志服务
//             Container.Register<ILog, Log>();
//
//             // 注册这个容器
//             Container.RegisterInstance(Container);
//
//
//         } 
//         
//         
//         /// <summary>
//         /// 配置默认注入
//         /// </summary>
//         /// <param name="action"></param>
//         public static void ConfigServices(Action<IServiceContainer> action)
//         {
//             action(Container);
//         }
//
//         /// <summary>
//         /// 运行
//         /// </summary>
//         public static void Run(Action action)
//         {
//             action();
//             Loop.Wait();
//         }
//         
//         /// <summary>
//         /// 运行
//         /// </summary>
//         public static void Run(Action<IServiceContainer> action)
//         {
//             action(Container);
//             Loop.Wait();
//         }
//     }
// #endif
// }