using System.Reflection;
using System.Threading;

namespace Pingfan.Kit
{
    public static class ProcessEx
    {
        private static string MutexName = Assembly.GetExecutingAssembly().GetType().GUID.ToString();

        /// <summary>
        /// 判断是否运行过
        /// </summary>
        public static bool HasRun
        {
            get
            {
                try
                {
                    // 尝试获取 Mutex
                    Mutex.OpenExisting(MutexName);
                    // 如果没有抛出异常，说明 Mutex 已经存在，即程序已经运行过
                    return true;
                }
                catch (WaitHandleCannotBeOpenedException)
                {
                    // WaitHandleCannotBeOpenedException 说明 Mutex 不存在，即程序没有运行过
                    // 创建 Mutex
                    new Mutex(false, MutexName);
                    return false;
                }
                catch (AbandonedMutexException)
                {
                    // AbandonedMutexException 说明 Mutex 被遗弃，即上一个程序实例没有正确释放 Mutex
                    // 但是操作系统已经帮我们接管并释放了这个 Mutex，所以我们可以安全地忽略这个异常
                    // 并创建一个新的 Mutex
                    new Mutex(false, MutexName);
                    return false;
                }
            }
        }
    }
}