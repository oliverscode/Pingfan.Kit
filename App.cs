using System;


namespace Pingfan.Kit
{
#if net48 || NETCOREAPP
    using LightInject;

    public class App
    {
        private static readonly IServiceContainer Container = new ServiceContainer();

        static App()
        {
            Container.SetDefaultLifetime<PerContainerLifetime>();
        }
        
        public static void ConfigDefaultService()
        {
            // 注册日志服务
            Container.Register<ILog, Log>();


        } 
        
        
        /// <summary>
        /// 配置默认注入
        /// </summary>
        /// <param name="action"></param>
        public static void ConfigServices(Action<IServiceContainer> action)
        {
            action(Container);
        }

        /// <summary>
        /// 运行
        /// </summary>
        public static void Run()
        {
            Loop.Wait();
        }
    }
#endif
}