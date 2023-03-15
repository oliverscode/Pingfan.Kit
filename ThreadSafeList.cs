using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Pingfan.Kit
{
    /// <summary>
    /// 线程安全的List, ConcurrentBag的包装
    /// </summary>
    public class ThreadSafeList<T> : ConcurrentBag<T>
    {
        public ThreadSafeList()
        {
        }

        public ThreadSafeList(IEnumerable<T> collection) : base(collection)
        {
        }

        // 其他方法可以根据需要添加
    }
}