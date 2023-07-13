using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 锁管理器
    /// </summary>
    public class Locker
    {
        /// <summary>
        /// 保存锁的并发字典，键为string类型，值为SemaphoreSlim对象。
        /// </summary>
        private static ConcurrentDictionary<string, SemaphoreSlim> _lockers =
            new ConcurrentDictionary<string, SemaphoreSlim>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 以key为锁名, 不需区分大小写，执行action
        /// </summary>
        public static void Run(string key, Action action)
        {
            var locker = _lockers.GetOrAdd(key, new SemaphoreSlim(1,1));
            locker.Wait();
            try
            {
                action.Invoke();
            }
            finally
            {
                locker.Release();
            }
        }

        /// <summary>
        /// 以key为锁名, 不需区分大小写，异步执行action
        /// </summary>
        public static async Task RunAsync(string key, Func<Task> action)
        {
            var locker = _lockers.GetOrAdd(key, new SemaphoreSlim(1,1));
            await locker.WaitAsync();
            try
            {
                await action.Invoke();
            }
            finally
            {
                locker.Release();
            }
        }
    }

}