using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 事件监听派发类, 事件名区分大小写
    /// </summary>
    public static class EventEx
    {
        private static readonly ConcurrentDictionary<string, List<EventsAction>> Actions =
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
            if (!EventEx.Actions.TryGetValue(eventName, out List<EventsAction> eventsActions))
                return;

            var removeList = new List<EventsAction>();

            foreach (var eventsAction in eventsActions)
            {
                if (eventsAction.CheckParameters(args))
                {
                    var task = eventsAction.Invoke(args) as Task;
                    task?.Wait();

                    // 是否是单次执行, 如果是, 则移除
                    if (eventsAction.IsOnce)
                    {
                        removeList.Add(eventsAction);
                    }
                }
            }

            foreach (var eventsAction in removeList)
            {
                eventsActions.Remove(eventsAction);
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
            EventEx.Actions.GetOrAdd(eventsAction.EventName, _ => new List<EventsAction>());
            var eventsActions = EventEx.Actions[eventsAction.EventName];
            eventsActions.Add(eventsAction);
        }

        private static void RemoveAction(object obj, string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                lock (Actions)
                {
                    // 删除obj对象的所有事件
                    foreach (var pair in Actions)
                    {
                        pair.Value.RemoveAll(action => action.Obj == obj);
                    }
                }
            }
            else
            {
                List<EventsAction> eventActions;
                lock (Actions)
                {
                    if (!EventEx.Actions.TryGetValue(eventName, out eventActions))
                    {
                        // 如果在给定的事件名称下找不到动作，直接返回
                        return;
                    }
                }

                lock (eventActions)
                {
                    eventActions.RemoveAll(action => action.Obj == obj);
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
            public bool IsOnce;


            private Delegate _action;
            private ParameterInfo[] _parameterInfos;


            public EventsAction(object obj, string eventName, Delegate action, bool isOnce)
            {
                this.Obj = obj;
                this.EventName = eventName;
                this.IsOnce = isOnce;
                this._action = action;
                this._parameterInfos = action.Method.GetParameters();
            }

            /// <summary>
            /// 检查参数是否匹配
            /// </summary>
            /// <param name="args"></param>
            /// <returns></returns>
            public bool CheckParameters(object[] args)
            {
                if (args.Length > _parameterInfos.Length)
                    return false;

                for (int i = 0; i < args.Length; i++)
                {
                    var paramType = _parameterInfos[i].ParameterType;
                    var argType = args[i].GetType();
                    if (paramType.IsGenericType && paramType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        paramType = Nullable.GetUnderlyingType(paramType);

                    if (paramType != null && !paramType.IsAssignableFrom(argType))
                        return false;
                }

                if (args.Length < _parameterInfos.Length)
                {
                    for (int i = args.Length; i < _parameterInfos.Length; i++)
                    {
                        if (!_parameterInfos[i].HasDefaultValue)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            /// <summary>
            /// 调用方法
            /// </summary>
            /// <param name="args"></param>
            /// <returns></returns>
            public object Invoke(object[] args)
            {
                return _action.DynamicInvoke(args);
            }
        }
    }
}