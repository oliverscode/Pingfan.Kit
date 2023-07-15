using System;


namespace Pingfan.Kit
{
#if net48 || NETCOREAPP
    using LightInject;

    public class App
    {
        private static IServiceContainer _container = new ServiceContainer();

        public static void ConfigServices(Action<IServiceContainer> action)
        {
            action(_container);
        }
    }
#endif
}