using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    public class EventEx
    {
        private object _EventLocker = new object();

        private readonly ConcurrentDictionary<string, List<EventsAction>> _actions =
            new ConcurrentDictionary<string, List<EventsAction>>(StringComparer
                .OrdinalIgnoreCase); //StringComparer.OrdinalIgnoreCase

        // private event Action<object, object[]> _beginAction = null;


        /// <summary>
        /// 是否存在某个时间监听
        /// </summary>
        /// <param name="key">事件名, 支持正则</param>
        /// <returns></returns>
        public bool Has(string key)
        {
            return _actions.ContainsKey(key);
        }


        /// <summary>
        /// 是否存在某个时间监听
        /// </summary>
        /// <param name="key">事件名, 支持正则</param>
        /// <returns></returns>
        public bool HasExp(string key)
        {
            return _actions.ContainsKey(key)
                   || _actions.Any(x => Regex.IsMatch(x.Key, key));
        }


        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="key">事件名</param>
        /// <param name="args">参数</param>
        public void Emit(string key, params object[] args)
        {
            if (_actions.ContainsKey(key) == false)
                return;

            lock (_EventLocker)
            {
                if (_actions.ContainsKey(key) == false)
                    return;

                var argsCount = args.Length;

                var eventsActions = _actions[key];
                for (var i = eventsActions.Count - 1; i >= 0; i--)
                {
                    var eventsAction = eventsActions[i];
                    // 参数不对就不执行了
                    if (eventsAction.ArgsCount != argsCount)
                        continue;

                    Task.Run(() =>
                    {
                        var result = eventsAction.Action?.DynamicInvoke(args) as Task;
                        result?.Wait();
                    });


                    //是否是单次执行
                    if (eventsAction.IsOnce)
                    {
                        eventsActions.Remove(eventsAction);
                    }
                }
            }
        }

        /// <summary>
        /// 执行第一个事件, 同时获取返回值
        /// </summary>
        /// <param name="key">事件名</param>
        /// <param name="args">参数</param>
        public T Emit<T>(string key, params object[] args)
        {
            if (_actions.ContainsKey(key) == false)
                throw new Exception("事件不存在");

            lock (_EventLocker)
            {
                if (_actions.ContainsKey(key) == false)
                    throw new Exception("事件不存在");

                if (_actions[key].Count > 1)
                    throw new Exception("事件存在多个监听, 无法获取返回值");


                var eventsAction = _actions[key].First();

                var result = eventsAction.Action?.DynamicInvoke(args);

                //是否是单次执行
                if (eventsAction.IsOnce)
                {
                    _actions.TryRemove(key, out _);
                }

                var ts = result as Task<T>;
                if (ts == null)
                {
                    return (T) ConvertEx.ChangeType(result, typeof(T));
                }

                result = ts.Result;
                return (T) ConvertEx.ChangeType(result, typeof(T));
            }
        }


        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="key">事件名, 支持正则</param>
        /// <param name="args">参数</param>
        public void EmitExp(string key, params object[] args)
        {
            // 是否有匹配的事件
            if (_actions.Any(x => Regex.IsMatch(x.Key, key)) == false)
                return;

            lock (_EventLocker)
            {
                // 是否有匹配的事件
                if (_actions.Any(x => Regex.IsMatch(x.Key, key)) == false)
                    return;

                // 获取匹配的事件
                var eventsActions = _actions.Where(x => Regex.IsMatch(x.Key, key))
                    .SelectMany(x => x.Value).ToList();

                for (var i = eventsActions.Count - 1; i >= 0; i--)
                {
                    var eventsAction = eventsActions[i];
                    Task.Run(() =>
                    {
                        var result = eventsAction.Action?.DynamicInvoke(args) as Task;
                        result?.Wait();
                    });

                    //是否是单次执行
                    if (eventsAction.IsOnce)
                    {
                        eventsActions.Remove(eventsAction);
                    }
                }
            }
        }

        /// <summary>
        /// 广播事件, 即使执行失败, 也不会异常
        /// </summary>
        /// <param name="key">事件名</param>
        /// <param name="args">参数</param>
        public void EmitWithTry(string key, params object[] args)
        {
            if (_actions.ContainsKey(key) == false)
                return;

            lock (_EventLocker)
            {
                if (_actions.ContainsKey(key) == false)
                    return;

                var eventsActions = _actions[key];
                for (var i = eventsActions.Count - 1; i >= 0; i--)
                {
                    var eventsAction = eventsActions[i];
                    Task.Run(() =>
                    {
                        try
                        {
                            var result = eventsAction.Action?.DynamicInvoke(args) as Task;
                            result?.Wait();
                        }
                        catch (Exception e)
                        {
                        }
                    });

                    //是否是单次执行
                    if (eventsAction.IsOnce)
                    {
                        eventsActions.Remove(eventsAction);
                    }
                }
            }
        }


        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="key">事件名, 支持正则</param>
        /// <param name="args">参数</param>
        public void EmitExpWithTry(string key, params object[] args)
        {
            // 是否有匹配的事件
            if (_actions.Any(x => Regex.IsMatch(x.Key, key)) == false)
                return;

            lock (_EventLocker)
            {
                // 是否有匹配的事件
                if (_actions.Any(x => Regex.IsMatch(x.Key, key)) == false)
                    return;

                // 获取匹配的事件
                var eventsActions = _actions.Where(x => Regex.IsMatch(x.Key, key))
                    .SelectMany(x => x.Value).ToList();

                for (var i = eventsActions.Count - 1; i >= 0; i--)
                {
                    var eventsAction = eventsActions[i];
                    Task.Run(() =>
                    {
                        try
                        {
                            var result = eventsAction.Action?.DynamicInvoke(args) as Task;
                            result?.Wait();
                        }
                        catch (Exception e)
                        {
                        }
                    });

                    //是否是单次执行
                    if (eventsAction.IsOnce)
                    {
                        eventsActions.Remove(eventsAction);
                    }
                }
            }
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On(string key, Action action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1>(string key, Action<T1> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2>(string key, Action<T1, T2> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3>(string key, Action<T1, T2, T3> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4>(string key, Action<T1, T2, T3, T4> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4, T5>(string key, Action<T1, T2, T3, T4, T5> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6>(string key, Action<T1, T2, T3, T4, T5, T6> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6, T7>(string key, Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6, T7, T8>(string key, Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On(string key, Func<Task> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1>(string key, Func<T1, Task> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2>(string key, Func<T1, T2, Task> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3>(string key, Func<T1, T2, T3, Task> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4>(string key, Func<T1, T2, T3, T4, Task> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4, T5>(string key, Func<T1, T2, T3, T4, T5, Task> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6>(string key, Func<T1, T2, T3, T4, T5, T6, Task> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6, T7>(string key, Func<T1, T2, T3, T4, T5, T6, T7, Task> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6, T7, T8>(string key,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件, 并获取返回值
        /// </summary>
        public void On<T1>(string key, Func<T1> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件, 并获取返回值
        /// </summary>
        public void On<T1, T2>(string key, Func<T1, T2> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件, 并获取返回值
        /// </summary>
        public void On<T1, T2, T3>(string key, Func<T1, T2, T3> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件, 并获取返回值
        /// </summary>
        public void On<T1, T2, T3, T4>(string key, Func<T1, T2, T3, T4> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件, 并获取返回值
        /// </summary>
        public void On<T1, T2, T3, T4, T5>(string key, Func<T1, T2, T3, T4, T5> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件, 并获取返回值
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6>(string key, Func<T1, T2, T3, T4, T5, T6> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件, 并获取返回值
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6, T7>(string key, Func<T1, T2, T3, T4, T5, T6, T7> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件, 并获取返回值
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6, T7, T8>(string key,
            Func<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            AddAction(key, action, false);
        }

        /// <summary>
        /// 监听事件, 并获取返回值
        /// </summary>
        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            AddAction(key, action, false);
        }


        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off(string key, Action action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1>(string key, Action<T1> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2>(string key, Action<T1, T2> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3>(string key, Action<T1, T2, T3> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4>(string key, Action<T1, T2, T3, T4> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4, T5>(string key, Action<T1, T2, T3, T4, T5> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4, T5, T6>(string key, Action<T1, T2, T3, T4, T5, T6> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4, T5, T6, T7>(string key, Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4, T5, T6, T7, T8>(string key,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off(string key, Func<Task> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1>(string key, Func<T1, Task> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2>(string key, Func<T1, T2, Task> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3>(string key, Func<T1, T2, T3, Task> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4>(string key, Func<T1, T2, T3, T4, Task> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4, T5>(string key, Func<T1, T2, T3, T4, T5, Task> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4, T5, T6>(string key, Func<T1, T2, T3, T4, T5, T6, Task> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4, T5, T6, T7>(string key, Func<T1, T2, T3, T4, T5, T6, T7, Task> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4, T5, T6, T7, T8>(string key,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        public void Off<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> action)
        {
            RemoveAction(key, action);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once(string key, Action action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1>(string key, Action<T1> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2>(string key, Action<T1, T2> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3>(string key, Action<T1, T2, T3> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4>(string key, Action<T1, T2, T3, T4> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4, T5>(string key, Action<T1, T2, T3, T4, T5> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4, T5, T6>(string key, Action<T1, T2, T3, T4, T5, T6> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4, T5, T6, T7>(string key, Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4, T5, T6, T7, T8>(string key,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once(string key, Func<Task> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1>(string key, Func<T1, Task> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2>(string key, Func<T1, T2, Task> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3>(string key, Func<T1, T2, T3, Task> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4>(string key, Func<T1, T2, T3, T4, Task> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4, T5>(string key, Func<T1, T2, T3, T4, T5, Task> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4, T5, T6>(string key, Func<T1, T2, T3, T4, T5, T6, Task> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4, T5, T6, T7>(string key, Func<T1, T2, T3, T4, T5, T6, T7, Task> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4, T5, T6, T7, T8>(string key,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action)
        {
            AddAction(key, action, true);
        }

        /// <summary>
        /// 监听事件, 但只监听一次
        /// </summary>
        public void Once<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> action)
        {
            AddAction(key, action, true);
        }

        private void AddAction(string key, Delegate action, bool isOnce)
        {
            if (key is null)
            {
                throw new ArgumentException(nameof(key));
            }
            //添加类名前缀
            // key = className + key;

            var eventsAction = new EventsAction()
            {
                IsOnce = isOnce,
                Action = action,
                ArgsCount = Fn.GetArgsCount(action),
            };

            lock (_EventLocker)
            {
                if (_actions.ContainsKey(key) == false)
                {
                    _actions.TryAdd(key, new List<EventsAction>());
                }

                var eventsActions = _actions[key];
                eventsActions.Add(eventsAction);
            }
        }

        private void RemoveAction(string key, Delegate action)
        {
            if (key is null)
            {
                throw new ArgumentException(nameof(key));
            }

            //已经不存在了
            if (_actions.ContainsKey(key) == false)
            {
                return;
            }

            lock (_EventLocker)
            {
                if (_actions.ContainsKey(key) == false)
                {
                    return;
                }

                if (action != null)
                {
                    var eventsActions = _actions[key];
                    eventsActions.RemoveAll(p => p.Action == action);
                }
                else
                {
                    _actions.TryRemove(key, out _);
                }
            }
        }

        /// <summary>
        /// 根据事件名移除所有事件
        /// </summary>
        /// <param name="key"></param>
        public void Off(string key)
        {
            RemoveAction(key, null);
        }

        /// <summary>
        /// 根据key移除所有事件
        /// </summary>
        /// <param name="key">事件名, 支持正则</param>
        public void OffExp(string key)
        {
            if (key is null)
            {
                throw new ArgumentException(nameof(key));
            }


            lock (_EventLocker)
            {
                var keys = _actions.Keys.Where(p => Regex.IsMatch(p, key)).ToList();
                foreach (var k in keys)
                {
                    _actions.TryRemove(k, out _);
                }
            }
        }

        class EventsAction
        {
            public bool IsOnce { get; set; }

            /// <summary>
            /// 方法参数的个数
            /// </summary>
            public int ArgsCount { get; set; }

            public Delegate Action { get; set; }
        }
    }
}