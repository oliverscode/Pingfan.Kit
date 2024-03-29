using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// IEnumerable扩展方法
    /// </summary>
    public static class EnumerableEx
    {
        /// <summary>
        /// 并行处理集合中的每一个元素。
        /// </summary>
        public static async Task Each<T>(this IEnumerable<T> list, Action<T> callBack, int threadCount = 0)
        {
            var queue = new ConcurrentQueue<T>(list);
            threadCount = threadCount <= 0 ? Environment.ProcessorCount * 2 : threadCount;
            threadCount = threadCount > queue.Count ? queue.Count : threadCount;

            var tasks = Enumerable.Range(0, threadCount)
                .Select(_ => Task.Run(() =>
                {
                    while (queue.TryDequeue(out var data))
                    {
                        callBack(data);
                    }
                }));

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 并行处理集合中的每一个元素。
        /// </summary>
        public static async Task EachAsync<T>(this IEnumerable<T> list, Func<T, Task> callBack, int threadCount = 0)
        {
            var queue = new ConcurrentQueue<T>(list);
            threadCount = threadCount <= 0 ? Environment.ProcessorCount * 2 : threadCount;
            threadCount = threadCount > queue.Count ? queue.Count : threadCount;

            var tasks = Enumerable.Range(0, threadCount)
                .Select(_ => Task.Run(async () =>
                {
                    while (queue.TryDequeue(out var data))
                    {
                        await callBack(data);
                    }
                }));

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 并行处理集合中的每一个元素。
        /// </summary>
        public static async Task Each<T>(this IEnumerable<T> list, Action<T, Progress> callBack, int threadCount = 0)
        {
            var queue = new ConcurrentQueue<T>(list);
            threadCount = threadCount <= 0 ? Environment.ProcessorCount * 2 : threadCount;
            threadCount = threadCount > queue.Count ? queue.Count : threadCount;

            var pe = new Progress();
            pe.Total = list.Count();
            var tasks = Enumerable.Range(0, threadCount)
                .Select(_ => Task.Run(() =>
                {
                    while (queue.TryDequeue(out var data))
                    {
                        callBack(data, pe);
                        pe.Next();
                    }
                }));

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 并行处理集合中的每一个元素。
        /// </summary>
        public static async Task EachAsync<T>(this IEnumerable<T> list, Func<T, Progress, Task> callBack,
            int threadCount = 0)
        {
            var queue = new ConcurrentQueue<T>(list);
            threadCount = threadCount <= 0 ? Environment.ProcessorCount * 2 : threadCount;
            threadCount = threadCount > queue.Count ? queue.Count : threadCount;

            var pe = new Progress();
            pe.Total = list.Count();
            var tasks = Enumerable.Range(0, threadCount)
                .Select(_ => Task.Run(async () =>
                {
                    while (queue.TryDequeue(out var data))
                    {
                        await callBack(data, pe);
                        pe.Next();
                    }
                }));

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 从集合中随机返回一个元素。
        /// </summary>
        public static T? RandomOne<T>(this IEnumerable<T> collection)
        {
            var list = collection as IList<T> ?? collection.ToList();
            if (list.Count == 0)
                return default;
            var index = RandomEx.Next(0, list.Count);
            return list[index];
        }

        /// <summary>
        /// 对集合进行乱序处理。
        /// </summary>
        public static IEnumerable<T> RandomSort<T>(this IEnumerable<T> list)
        {
            return list.OrderBy(_ => Guid.NewGuid());
        }

        /// <summary>
        /// 判断集合中是否包含指定字符串（忽略大小写）。
        /// </summary>
        public static bool ContainsIgnoreCase(this IEnumerable<string> list, string value)
        {
            return list.Any(p => p.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 克隆一个集合
        /// </summary>
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> list)
        {
            return list.Select(p => p);
        }
    }
}