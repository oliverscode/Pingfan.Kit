using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Pingfan.Kit
{
    /// <summary>
    /// 线程安全的Dictionary, ConcurrentDictionary的包装
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ThreadSafeDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {
        public ThreadSafeDictionary()
        {
        }

        public ThreadSafeDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }
    }
}