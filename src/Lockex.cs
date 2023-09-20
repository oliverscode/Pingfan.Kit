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
        /// 保存锁的并发字典，键为string类型，值为SemaphoreSlim对象。
        /// </summary>
        private static readonly ConcurrentDictionary<string, object> Lockers =
            new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 以key为锁名, 不需区分大小写，执行action
        /// </summary>
        public static void Run(string key, Action action)
        {
            var locker = Lockers.GetOrAdd(key, new object());
            lock (locker)
            {
                action();
            }
        }
        
        /// <summary>
        /// 获取Key为名的锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return Lockers.GetOrAdd(key, new object());
        }
    }
}