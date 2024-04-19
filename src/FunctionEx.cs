using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 封装了防抖器和节流器
    /// </summary>
    public static class FunctionEx
    {
        private enum FnKind
        {
            /// <summary>
            /// 防抖器, 一定时间内只执行最后一次
            /// </summary>
            Debounce,

            /// <summary>
            /// 节流器, 一定时间内只执行一次
            /// </summary>
            Throttle,
        }


        private static readonly ConcurrentDictionary<string, CancellationTokenSource> List =
            new(StringComparer.OrdinalIgnoreCase);

        private static void Fn(string key, int delay, FnKind fnKind, Delegate action, params object?[]? args)
        {
            if (List.TryGetValue(key, out var item))
            {
                if (fnKind == FnKind.Throttle)
                {
                    return;
                }

                if (fnKind == FnKind.Debounce)
                {
                    if (List.TryRemove(key, out _))
                    {
                        item.Cancel();
                    }
                }
            }

            var tokenSource = Ticker.Once(delay, () =>
            {
                action.DynamicInvoke(args);
                List.TryRemove(key, out _);
            });
            List[key] = tokenSource;
        }

        #region 防抖器部分

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce(string key, int delay, Action action)
        {
            Fn(key, delay, FnKind.Debounce, action);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1>(string key, int delay, Action<T1> action, T1 p1)
        {
            Fn(key, delay, FnKind.Debounce, action, p1);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2>(string key, int delay, Action<T1, T2> action, T1 p1, T2 p2)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3>(string key, int delay, Action<T1, T2, T3> action, T1 p1, T2 p2, T3 p3)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4>(string key, int delay,
            Action<T1, T2, T3, T4> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5>(string key, int delay,
            Action<T1, T2, T3, T4, T5> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4, p5);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6>(string key, int delay,
            Action<T1, T2, T3, T4, T5, T6> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7>(string key, int delay,
            Action<T1, T2, T3, T4, T5, T6, T7> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7, T8>(string key, int delay,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7,
            T8 p8)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key, int delay,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7,
            T8 p8,
            T9 p9)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce(string key, int delay, Func<Task> action)
        {
            Fn(key, delay, FnKind.Debounce, action);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1>(string key, int delay, Func<T1, Task> action, T1 p1)
        {
            Fn(key, delay, FnKind.Debounce, action, p1);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2>(string key, int delay, Func<T1, T2, Task> action, T1 p1, T2 p2)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3>(string key, int delay, Func<T1, T2, T3, Task> action, T1 p1, T2 p2,
            T3 p3)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4>(string key, int delay,
            Func<T1, T2, T3, T4, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5>(string key, int delay,
            Func<T1, T2, T3, T4, T5, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4, p5);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6>(string key, int delay,
            Func<T1, T2, T3, T4, T5, T6, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7>(string key, int delay,
            Func<T1, T2, T3, T4, T5, T6, T7, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7, T8>(string key, int delay,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7,
            T8 p8)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key, int delay,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7,
            T8 p8,
            T9 p9)
        {
            Fn(key, delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }

        #endregion

        #region 节流器部分

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle(string key, int delay, Action action)
        {
            Fn(key, delay, FnKind.Throttle, action);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1>(string key, int delay, Action<T1> action, T1 p1)
        {
            Fn(key, delay, FnKind.Throttle, action, p1);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2>(string key, int delay, Action<T1, T2> action, T1 p1, T2 p2)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3>(string key, int delay, Action<T1, T2, T3> action, T1 p1, T2 p2, T3 p3)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4>(string key, int delay,
            Action<T1, T2, T3, T4> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5>(string key, int delay,
            Action<T1, T2, T3, T4, T5> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4, p5);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6>(string key, int delay,
            Action<T1, T2, T3, T4, T5, T6> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7>(string key, int delay,
            Action<T1, T2, T3, T4, T5, T6, T7> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7, T8>(string key, int delay,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7,
            T8 p8)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key, int delay,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7,
            T8 p8,
            T9 p9)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle(string key, int delay, Func<Task> action)
        {
            Fn(key, delay, FnKind.Throttle, action);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1>(string key, int delay, Func<T1, Task> action, T1 p1)
        {
            Fn(key, delay, FnKind.Throttle, action, p1);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2>(string key, int delay, Func<T1, T2, Task> action, T1 p1, T2 p2)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3>(string key, int delay, Func<T1, T2, T3, Task> action, T1 p1, T2 p2,
            T3 p3)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4>(string key, int delay,
            Func<T1, T2, T3, T4, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5>(string key, int delay,
            Func<T1, T2, T3, T4, T5, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4, p5);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6>(string key, int delay,
            Func<T1, T2, T3, T4, T5, T6, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7>(string key, int delay,
            Func<T1, T2, T3, T4, T5, T6, T7, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7, T8>(string key, int delay,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7,
            T8 p8)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key, int delay,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7,
            T8 p8,
            T9 p9)
        {
            Fn(key, delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }

        #endregion

        #region 限流部分

        private static ThreadSafeDictionary<string, List<DateTime>> _limitList = new();

        /// <summary>
        /// 如果处于限流状态, 就返回true, 否者返回false
        /// </summary>
        /// <param name="key">标识</param>
        /// <param name="count">最大次数, 包含此次数</param>
        /// <param name="delay">毫秒时间内, 最大不能超过60秒</param>
        /// <returns></returns>
        public static bool Limit(string key, int count, int delay)
        {
            if (delay > 60000)
            {
                throw new ArgumentException("delay不能超过10秒");
            }

            if (_limitList.TryGetValue(key, out var list) == false)
            {
                _limitList.TryAdd(key, new List<DateTime>()
                {
                    DateTime.Now
                });

                return false;
            }

            list.Add(DateTime.Now);
            // 判断是否超过了限制
            if (list.Count(x => x > DateTime.Now.AddMilliseconds(-delay)) > count)
            {
                return true;
            }

            return false;
        }

        #endregion

        static FunctionEx()
        {
            Ticker.LoopWithTry(1000, () =>
            {
                // 倒着循环_limitList
                for (var i = _limitList.Count - 1; i >= 0; i--)
                {
                    var item = _limitList.ElementAt(i);
                    var list = item.Value;
                    // 倒着循环list
                    for (var j = list.Count - 1; j >= 0; j--)
                    {
                        if (list[j] < DateTime.Now.AddSeconds(-60))
                        {
                            list.RemoveAt(j);
                        }
                    }

                    if (list.Count == 0)
                    {
                        _limitList.TryRemove(item.Key, out _);
                    }
                }
            });
        }
    }
}