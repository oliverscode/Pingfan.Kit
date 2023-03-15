using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 事件监听派发类, 事件名区分大小写
    /// </summary>
    public static class EventEx
    {
        private static readonly ConcurrentDictionary<string, List<EventsAction>> _actions =
            new ConcurrentDictionary<string, List<EventsAction>>();

        #region 监听事件

        public static void On(object obj, string eventName, Action action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1>(object obj, string eventName, Action<T1> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2>(object obj, string eventName, Action<T1, T2> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3>(object obj, string eventName, Action<T1, T2, T3> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4>(object obj, string eventName, Action<T1, T2, T3, T4> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4, T5>(object obj, string eventName, Action<T1, T2, T3, T4, T5> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4, T5, T6>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4, T5, T6, T7>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4, T5, T6, T7, T8>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4, T5, T6, T7, T8, T9>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        #endregion

        #region 监听一次事件

        public static void Once(object obj, string eventName, Action action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1>(object obj, string eventName, Action<T1> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2>(object obj, string eventName, Action<T1, T2> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3>(object obj, string eventName, Action<T1, T2, T3> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4>(object obj, string eventName, Action<T1, T2, T3, T4> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4, T5>(object obj, string eventName, Action<T1, T2, T3, T4, T5> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4, T5, T6>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4, T5, T6, T7>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4, T5, T6, T7, T8>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4, T5, T6, T7, T8, T9>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        #endregion

        #region 监听事件,支持异步方法

        public static void On(object obj, string eventName, Func<Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1>(object obj, string eventName, Func<T1, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2>(object obj, string eventName, Func<T1, T2, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3>(object obj, string eventName, Func<T1, T2, T3, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4>(object obj, string eventName, Func<T1, T2, T3, T4, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4, T5>(object obj, string eventName, Func<T1, T2, T3, T4, T5, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4, T5, T6>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4, T5, T6, T7>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4, T5, T6, T7, T8>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        public static void On<T1, T2, T3, T4, T5, T6, T7, T8, T9>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, false));
        }

        #endregion

        #region 监听事件,支持异步方法,单次执行

        public static void Once(object obj, string eventName, Func<Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1>(object obj, string eventName, Func<T1, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2>(object obj, string eventName, Func<T1, T2, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3>(object obj, string eventName, Func<T1, T2, T3, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4>(object obj, string eventName, Func<T1, T2, T3, T4, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4, T5>(object obj, string eventName, Func<T1, T2, T3, T4, T5, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4, T5, T6>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4, T5, T6, T7>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4, T5, T6, T7, T8>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        public static void Once<T1, T2, T3, T4, T5, T6, T7, T8, T9>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> action)
        {
            AddAction(new EventsAction(obj, eventName, action, true));
        }

        #endregion

        #region 派发事件

        public static void Emit(string eventName, params object[] args)
        {
            if (!_actions.TryGetValue(eventName, out List<EventsAction> actions))
                return;

            lock (eventName)
            {
                foreach (var eventsAction in actions)
                {
                    var task = eventsAction.Action.DynamicInvoke(args) as Task;
                    task?.Wait();

                    // 是否是单次执行, 如果是, 则移除
                    if (eventsAction.IsOnce)
                    {
                        actions.Remove(eventsAction);
                    }
                }
            }
        }

        public static void EmitRunWithTry(string eventName, params object[] args)
        {
            if (!_actions.TryGetValue(eventName, out List<EventsAction> actions))
                return;

            lock (eventName)
            {
                foreach (var eventsAction in actions)
                {
                    try
                    {
                        var task = eventsAction.Action.DynamicInvoke(args) as Task;
                        task.Wait();
                    }
                    catch
                    {
                    }

                    // 是否是单次执行, 如果是, 则移除
                    if (eventsAction.IsOnce)
                    {
                        actions.Remove(eventsAction);
                    }
                }
            }
        }

        #endregion

        #region 移除监听

        /// <summary>
        /// 移除监听事件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="eventName"></param>
        public static void Off(object obj, string eventName)
        {
            RemoveAction(obj, eventName);
        }

        /// <summary>
        /// 删除该对象的全部监听方法
        /// </summary>
        /// <param name="obj"></param>
        public static void Off(object obj)
        {
            RemoveAction(obj, null);
        }

        #endregion

        #region 静态处理方法

        private static void AddAction(EventsAction eventsAction)
        {
            lock (_actions)
            {
                if (!_actions.ContainsKey(eventsAction.EventName))
                {
                    _actions[eventsAction.EventName] = new List<EventsAction>();
                }

                eventsAction.Sign = Fn.GetSignture(eventsAction.Action);
                var actions = _actions[eventsAction.EventName];
                // sign必须和action全部一致才添加
                if (actions.Count > 0 && actions.Any(x => x.Sign != eventsAction.Sign))
                {
                    throw new Exception("方法签名不一致");
                }

                actions.Add(new EventsAction
                {
                    Obj = eventsAction.Obj,
                    Action = eventsAction.Action,
                    IsOnce = eventsAction.IsOnce,
                    Sign = eventsAction.Sign,
                });
            }
        }

        private static void RemoveAction(object obj, string eventName)
        {
            lock (_actions)
            {
                if (string.IsNullOrEmpty(eventName))
                {
                    // 删除obj对象的所有事件
                    foreach (var pair in _actions)
                    {
                        pair.Value.RemoveAll(action => action.Obj == obj);
                    }
                }
                else
                {
                    // 删除obj对象的eventName事件
                    if (_actions.TryGetValue(eventName, out var actions))
                    {
                        actions.RemoveAll(action => action.Obj == obj);
                    }
                }
            }
        }

        #endregion

        private class EventsAction
        {
            /// <summary>
            /// 绑定的对象,方便做识别
            /// </summary>
            public object Obj;

            public string EventName;
            public Delegate Action;
            public bool IsOnce;
            public string Sign;

            public EventsAction()
            {
            }

            public EventsAction(object obj, string eventName, Delegate action, bool isOnce)
            {
                Obj = obj;
                EventName = eventName;
                Action = action;
                IsOnce = isOnce;
            }
        }
    }
}