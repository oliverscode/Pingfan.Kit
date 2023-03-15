using System;

namespace Pingfan.Kit
{
    public static class LockEx
    {
        private static readonly object _locker = new object();

        /// <summary>
        /// 全局单线程执行
        /// </summary>
        /// <param name="action"></param>
        public static void Run(Action action)
        {
            lock (_locker)
            {
                action();
            }
        }

        /// <summary>
        /// 单线程执行
        /// </summary>
        /// <param name="locker"></param>
        /// <param name="action"></param>
        public static void Run(object locker, Action action)
        {
            lock (locker)
            {
                action();
            }
        }
    }
}