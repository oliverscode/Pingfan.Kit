using System;
using System.Reflection;
using System.Threading;

namespace Pingfan.Kit
{
    public static class ProcessEx
    {
        private static string _mutexName = $"{Assembly.GetEntryAssembly().FullName}_{Environment.UserInteractive}";
        private static Mutex _mutex;

        public static bool HasRun
        {
            get
            {
                bool createdNew;
                _mutex = new Mutex(true, _mutexName, out createdNew);
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
    }
}