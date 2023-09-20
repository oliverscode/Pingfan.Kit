using System;
using System.Collections.Concurrent;
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
        private class DataItem
        {
            public Task Timer;
            public CancellationTokenSource Cts;
        }
        
        
        private static readonly ConcurrentDictionary<string, DataItem> List =
            new ConcurrentDictionary<string, DataItem>(StringComparer.OrdinalIgnoreCase);

        private static void Fn(string key, int delay, FnKind fnKind, Delegate action, params object[] args)
        {
            // var key = action.GetMethodInfo().Name;
            if (List.TryGetValue(key, out var item))
            {
                if (fnKind == FnKind.Debounce)
                {
                    item.Cts.Cancel();
                    List.TryRemove(key, out _);
                }
                else if (fnKind == FnKind.Throttle)
                {
                    return;
                }
            }

            var cts = new CancellationTokenSource();
            var timer = Timer.SetTimeout(delay, () =>
            {
                action.DynamicInvoke(args);
                List.TryRemove(key, out _);
            }, cts.Token);
            List[key] = new DataItem
            {
                Timer = timer,
                Cts = cts
            };
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
        public static void Debounce<T1, T2, T3>(string key, int delay, Func<T1, T2, T3, Task> action, T1 p1, T2 p2, T3 p3)
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
        public static void Throttle<T1, T2, T3>(string key, int delay, Func<T1, T2, T3, Task> action, T1 p1, T2 p2, T3 p3)
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
        
    }
}