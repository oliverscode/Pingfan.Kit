using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 封装了防抖器和节流器
    /// </summary>
    public static class Fn
    {
        #region 防抖器和节流器

        private class DataItem
        {
            public Task Timer { get; set; }
            public CancellationTokenSource Cts { get; set; }
        }

        private enum FnKind
        {
            Debounce,
            Throttle,
        }


        private static readonly ConcurrentDictionary<string, DataItem> _DebounceList =
            new ConcurrentDictionary<string, DataItem>(StringComparer.OrdinalIgnoreCase);

        private static void _Fn(int delay, FnKind fnKind, Delegate action, params object[] parms)
        {
            var key = action.GetMethodInfo().Name;
            if (_DebounceList.TryGetValue(key, out var item))
            {
                if (fnKind == FnKind.Debounce)
                {
                    item.Cts.Cancel();
                    _DebounceList.TryRemove(key, out _);
                }
                else if (fnKind == FnKind.Throttle)
                {
                    return;
                }
            }

            var cts = new CancellationTokenSource();
            var timer = Timer.SetTimeout(delay, () =>
            {
                action.DynamicInvoke(parms);
                _DebounceList.TryRemove(key, out _);
            }, cts.Token);
            _DebounceList[key] = new DataItem
            {
                Timer = timer,
                Cts = cts
            };
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce(int delay, Action action)
        {
            _Fn(delay, FnKind.Debounce, action);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1>(int delay, Action<T1> action, T1 p1)
        {
            _Fn(delay, FnKind.Debounce, action, p1);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2>(int delay, Action<T1, T2> action, T1 p1, T2 p2)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3>(int delay, Action<T1, T2, T3> action, T1 p1, T2 p2, T3 p3)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4>(int delay,
            Action<T1, T2, T3, T4> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5>(int delay,
            Action<T1, T2, T3, T4, T5> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4, p5);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6>(int delay,
            Action<T1, T2, T3, T4, T5, T6> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7>(int delay,
            Action<T1, T2, T3, T4, T5, T6, T7> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7, T8>(int delay,
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
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int delay,
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
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce(int delay, Func<Task> action)
        {
            _Fn(delay, FnKind.Debounce, action);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1>(int delay, Func<T1, Task> action, T1 p1)
        {
            _Fn(delay, FnKind.Debounce, action, p1);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2>(int delay, Func<T1, T2, Task> action, T1 p1, T2 p2)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3>(int delay, Func<T1, T2, T3, Task> action, T1 p1, T2 p2, T3 p3)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4>(int delay,
            Func<T1, T2, T3, T4, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5>(int delay,
            Func<T1, T2, T3, T4, T5, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4, p5);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6>(int delay,
            Func<T1, T2, T3, T4, T5, T6, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7>(int delay,
            Func<T1, T2, T3, T4, T5, T6, T7, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7)
        {
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7, T8>(int delay,
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
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        /// <summary>
        /// 防抖器, 一定时间内只执行最后一次
        /// </summary>
        public static void Debounce<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int delay,
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
            _Fn(delay, FnKind.Debounce, action, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle(int delay, Action action)
        {
            _Fn(delay, FnKind.Throttle, action);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1>(int delay, Action<T1> action, T1 p1)
        {
            _Fn(delay, FnKind.Throttle, action, p1);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2>(int delay, Action<T1, T2> action, T1 p1, T2 p2)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3>(int delay, Action<T1, T2, T3> action, T1 p1, T2 p2, T3 p3)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4>(int delay,
            Action<T1, T2, T3, T4> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5>(int delay,
            Action<T1, T2, T3, T4, T5> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4, p5);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6>(int delay,
            Action<T1, T2, T3, T4, T5, T6> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7>(int delay,
            Action<T1, T2, T3, T4, T5, T6, T7> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7, T8>(int delay,
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
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int delay,
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
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle(int delay, Func<Task> action)
        {
            _Fn(delay, FnKind.Throttle, action);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1>(int delay, Func<T1, Task> action, T1 p1)
        {
            _Fn(delay, FnKind.Throttle, action, p1);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2>(int delay, Func<T1, T2, Task> action, T1 p1, T2 p2)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3>(int delay, Func<T1, T2, T3, Task> action, T1 p1, T2 p2, T3 p3)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4>(int delay,
            Func<T1, T2, T3, T4, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5>(int delay,
            Func<T1, T2, T3, T4, T5, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4, p5);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6>(int delay,
            Func<T1, T2, T3, T4, T5, T6, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7>(int delay,
            Func<T1, T2, T3, T4, T5, T6, T7, Task> action,
            T1 p1,
            T2 p2,
            T3 p3,
            T4 p4,
            T5 p5,
            T6 p6,
            T7 p7)
        {
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7, T8>(int delay,
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
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        /// <summary>
        /// 节流器, 一定时间内只执行一次
        /// </summary>
        public static void Throttle<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int delay,
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
            _Fn(delay, FnKind.Throttle, action, p1, p2, p3, p4, p5, p6, p7, p8, p9);
        }

        #endregion



        #region 获取方法签名

        private static Dictionary<Delegate, SignItem> _MethodSignatures = new Dictionary<Delegate, SignItem>();
        private static object _LockerSign = new Object();

        public static string GetSignture(Delegate fn)
        {
            if (_MethodSignatures.ContainsKey(fn))
            {
                return _MethodSignatures[fn].Sign;
            }

            lock (_LockerSign)
            {
                if (_MethodSignatures.ContainsKey(fn))
                {
                    return _MethodSignatures[fn].Sign;
                }

                GenFn(fn);
                return _MethodSignatures[fn].Sign;
            }
        }

        public static int GetArgsCount(Delegate fn)
        {
            if (_MethodSignatures.ContainsKey(fn))
            {
                return _MethodSignatures[fn].ArgsCount;
            }

            lock (_LockerSign)
            {
                if (_MethodSignatures.ContainsKey(fn))
                {
                    return _MethodSignatures[fn].ArgsCount;
                }

                GenFn(fn);
                return _MethodSignatures[fn].ArgsCount;
            }
        }

        private static void GenFn(Delegate fn)
        {
            // 获取fn的签名
            var method = fn.Method;
            var sb = new StringBuilder();
            sb.Append(method.Name);
            sb.Append("(");
            var parameters = method.GetParameters();
            foreach (var parameter in parameters)
            {
                sb.Append(parameter.ParameterType.Name);
                sb.Append(",");
            }

            if (parameters.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append(")");
            var sign = sb.ToString();
            _MethodSignatures[fn] = new SignItem() { Sign = sign, ArgsCount = parameters.Length };
        }

        private class SignItem
        {
            public string Sign { get; set; }

            /// <summary>
            /// 参数的个数
            /// </summary>
            public int ArgsCount { get; set; }
        }

        #endregion
    }
}