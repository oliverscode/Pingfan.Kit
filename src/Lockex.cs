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
            new(StringComparer.OrdinalIgnoreCase);

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
        public static T Run<T>(string key, Func<T> action)
        {
            var locker = Get(key);
            locker.Wait();
            try
            {
                return action();
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
        /// 以key为锁名, 不需区分大小写，执行action, 其实就是lock的语法糖
        /// </summary>
        public static async Task<T> RunAsync<T>(string key, Func<Task<T>> action)
        {
            var locker = Get(key);
            await locker.WaitAsync();
            try
            {
                return await action();
            }
            finally
            {
                locker.Release();
            }
        }


        /// <summary>
        /// 本程序域为锁名, 不需区分大小写，执行action, 其实就是lock的语法糖
        /// </summary>
        public static void Run(Action action)
        {
            var locker = Get(AppDomain.CurrentDomain.FriendlyName);
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
        /// 本程序域为锁名, 不需区分大小写，执行action, 其实就是lock的语法糖
        /// </summary>
        public static async Task RunAsync(Func<Task> action)
        {
            var locker = Get(AppDomain.CurrentDomain.FriendlyName);
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