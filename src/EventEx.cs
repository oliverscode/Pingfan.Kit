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
    public class EventEx
    {
        /// <summary>
        /// 默认的全局事件
        /// </summary>
        public static readonly EventEx Default = new EventEx();

        private readonly ConcurrentDictionary<string, List<EventAction>> _actions =
            new ConcurrentDictionary<string, List<EventAction>>();

        #region 监听事件

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void On(object obj, string eventName, Action method)

        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1>(object obj, string eventName, Action<T1> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2>(object obj, string eventName, Action<T1, T2> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3>(object obj, string eventName, Action<T1, T2, T3> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4>(object obj, string eventName, Action<T1, T2, T3, T4> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4, T5>(object obj, string eventName, Action<T1, T2, T3, T4, T5> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4, T5, T6>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4, T5, T6, T7>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4, T5, T6, T7, T8>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        #endregion

        #region 监听一次事件

        public void Once(object obj, string eventName, Action method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1>(object obj, string eventName, Action<T1> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2>(object obj, string eventName, Action<T1, T2> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3>(object obj, string eventName, Action<T1, T2, T3> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4>(object obj, string eventName, Action<T1, T2, T3, T4> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4, T5>(object obj, string eventName, Action<T1, T2, T3, T4, T5> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4, T5, T6>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4, T5, T6, T7>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4, T5, T6, T7, T8>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4, T5, T6, T7, T8, T9>(object obj, string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        #endregion

        #region 监听事件,支持异步方法

        public void On(object obj, string eventName, Func<Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1>(object obj, string eventName, Func<T1, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2>(object obj, string eventName, Func<T1, T2, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3>(object obj, string eventName, Func<T1, T2, T3, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4>(object obj, string eventName, Func<T1, T2, T3, T4, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4, T5>(object obj, string eventName, Func<T1, T2, T3, T4, T5, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4, T5, T6>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4, T5, T6, T7>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4, T5, T6, T7, T8>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        public void On<T1, T2, T3, T4, T5, T6, T7, T8, T9>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, false));
        }

        #endregion

        #region 监听事件,支持异步方法,单次执行

        public void Once(object obj, string eventName, Func<Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1>(object obj, string eventName, Func<T1, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2>(object obj, string eventName, Func<T1, T2, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3>(object obj, string eventName, Func<T1, T2, T3, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4>(object obj, string eventName, Func<T1, T2, T3, T4, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4, T5>(object obj, string eventName, Func<T1, T2, T3, T4, T5, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4, T5, T6>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4, T5, T6, T7>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4, T5, T6, T7, T8>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        public void Once<T1, T2, T3, T4, T5, T6, T7, T8, T9>(object obj, string eventName,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> method)
        {
            AddAction(new EventAction(obj, eventName, method, true));
        }

        #endregion

        #region 派发事件

        public void Emit(string eventName, params object[] args)
        {
            if (!_actions.TryGetValue(eventName, out List<EventAction> eventsActions))
                return;

            var removeList = new List<EventAction>();

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
        public void Off(object obj, string eventName)
        {
            RemoveAction(obj, eventName);
        }

        /// <summary>
        /// 删除该对象的全部监听方法
        /// </summary>
        /// <param name="obj"></param>
        public void Off(object obj)
        {
            RemoveAction(obj, null);
        }

        #endregion

        #region 静态处理方法

        private void AddAction(EventAction eventAction)
        {
            _actions.GetOrAdd(eventAction.EventName, _ => new List<EventAction>());
            var eventsActions = _actions[eventAction.EventName];
            eventsActions.Add(eventAction);
        }

        private void RemoveAction(object obj, string? eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                lock (_actions)
                {
                    // 删除obj对象的所有事件
                    foreach (var pair in _actions)
                    {
                        pair.Value.RemoveAll(method => method.Obj == obj);
                    }
                }
            }
            else
            {
                List<EventAction> eventActions;
                lock (_actions)
                {
                    if (!_actions.TryGetValue(eventName!, out eventActions!))
                    {
                        // 如果在给定的事件名称下找不到动作，直接返回
                        return;
                    }
                }

                lock (eventActions)
                {
                    eventActions.RemoveAll(method => method.Obj == obj);
                }
            }
        }

        #endregion

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        private class EventAction
        {
            /// <summary>
            /// 绑定的对象,方便做识别
            /// </summary>
            public object Obj { get; set; }

            public string EventName { get; set; }
            public bool IsOnce { get; set; }


            private readonly Delegate _action;
            private readonly ParameterInfo[] _parameterInfos;


            public EventAction(object obj, string eventName, Delegate method, bool isOnce)
            {
                this.Obj = obj;
                this.EventName = eventName;
                this.IsOnce = isOnce;
                this._action = method;
                this._parameterInfos = method.Method.GetParameters();
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
            public object? Invoke(object[] args)
            {
                return _action.DynamicInvoke(args);
            }
        }
    }
}