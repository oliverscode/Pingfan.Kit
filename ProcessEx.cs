using System;
using System.Reflection;
using System.Threading;

namespace Pingfan.Kit
{
    public static class ProcessEx
    {
        private static string mutexName = $"{Assembly.GetEntryAssembly().FullName}_{Environment.UserInteractive}";
        private static Mutex mutex = null;

        public static bool HasRun
        {
            get
            {
                bool createdNew;
                mutex = new Mutex(true, mutexName, out createdNew);
                if (createdNew)
                {
                    // Don't release the mutex until the application is closed
                    return false;
                }

                // Close the mutex if it's already been created
                mutex.Close();
                return true;
            }
        }
    }
}