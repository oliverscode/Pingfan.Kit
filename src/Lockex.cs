using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 锁管理器
    /// </summary>
    public static class Locker
    {
        /// <summary>
        /// 保存锁的并发字典，键为string类型，值为SemaphoreSlim类型
        /// </summary>
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> Lockers =
            new ConcurrentDictionary<string, SemaphoreSlim>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 以key为锁名, 不需区分大小写，执行action, 其实就是lock的语法糖
        /// </summary>
        public static void Run(string key, Action action)
        {
            var locker = Get(key);
            locker.Wait();
            try
            {
                action();
            }
            finally
            {
                locker.Release();
            }
        }

        /// <summary>
        /// 以key为锁名, 不需区分大小写，执行action, 其实就是lock的语法糖
        /// </summary>
        public static async Task RunAsync(string key, Func<Task> action)
        {
            var locker = Get(key);
            await locker.WaitAsync();
            try
            {
                await action();
            }
            finally
            {
                locker.Release();
            }
        }

        /// <summary>
        /// 获取Key为名的锁, 不区分大小写, 如果不存在则创建
        /// </summary>
        public static SemaphoreSlim Get(string key)
        {
            return Lockers.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
        }
    }
}