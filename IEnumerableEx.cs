using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    public static class IEnumerableEx
    {
        /// <summary>
        /// 多线程遍历
        /// </summary>
        public static async Task Each<T>(this IEnumerable<T> list, Action<T> callBack, int threadCount = 0)
        {
            var enumerable = new ConcurrentQueue<T>(list);
            if (threadCount <= 0)
                threadCount = Environment.ProcessorCount * 2;
            if (threadCount > enumerable.Count)
                threadCount = enumerable.Count;
            if (enumerable.Count <= 0)
                return;


            var tasks = new Task[threadCount];

            for (var i = 0; i < threadCount; i++)
            {
                var t = Task.Factory.StartNew(() =>
                {
                    while (enumerable.TryDequeue(out T data))
                    {
                        callBack(data);
                    }
                }, TaskCreationOptions.LongRunning);
                tasks[i] = t;
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 多线程遍历
        /// </summary>
        public static async Task Each<T>(this IEnumerable<T> list, Func<T, Task> callBack, int threadCount = 0)
        {
            var enumerable = new ConcurrentQueue<T>(list);
            if (threadCount <= 0)
                threadCount = Environment.ProcessorCount * 2;
            if (threadCount > enumerable.Count)
                threadCount = enumerable.Count;
            if (enumerable.Count <= 0)
                return;

            var tasks = new Task[threadCount];
            for (var i = 0; i < threadCount; i++)
            {
                var t = Task.Factory.StartNew(() =>
                {
                    while (enumerable.TryDequeue(out T data))
                    {
                        callBack(data).Wait();
                    }
                }, TaskCreationOptions.LongRunning);
                tasks[i] = t;
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 随机取一个元素
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RandomOne<T>(this IEnumerable<T> list)
        {
            var index = RandomEx.Next(0, list.Count());
            return list.ElementAt(index);
        }



        /// <summary>
        /// 返回乱序后的列表
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> RandomSort<T>(this IEnumerable<T> list)
        {
            return list.OrderBy(p => Guid.NewGuid());
        }

        /// <summary>
        /// 忽略大小写的形式判断是否包含
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this IEnumerable<string> list, string value)
        {
            return list.Any(p => p.ContainsIgnoreCase(value));
        }
        
    }
}