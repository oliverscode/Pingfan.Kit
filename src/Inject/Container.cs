using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pingfan.Kit.Inject.Attributes;
using Pingfan.Kit.Inject.Interfaces;

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public class Container : IDisposable
    {
        private readonly Container? _parent;
        private readonly List<Container> _children;
        private readonly List<PushItem> _objectItems = new List<PushItem>();

        /// <summary>
        /// 每次注入的最大深度, 防止循环依赖
        /// </summary>
        public int MaxDeep { get; set; } = 20;


        /// <summary>
        /// 传递父容器
        /// </summary>
        /// <param name="parent"></param>
        public Container(Container? parent = null)
        {
            _parent = parent;
            _children = new List<Container>();
        }


        /// <summary>
        /// 是否是跟容器, 一般用作全局容器
        /// </summary>
        public bool IsRoot => _parent != null;
        
        /// <summary>
        /// 根容器
        /// </summary>
        public Container Root => _parent?.Root ?? this;

        /// <summary>
        /// 子容器
        /// </summary>
        public List<Container> Children => _children;

        /// <summary>
        /// 父容器
        /// </summary>
        public Container? Parent => _parent;


        /// <summary>
        /// 注入数据
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="Exception"></exception>
        public void Push<T>(string? name = null)
        {
            var type = typeof(T);
            if (type.IsInterface)
                throw new Exception("无法注入接口");

            var item = new PushItem(null, type, name, null);
            _objectItems.Add(item);
        }

        public void Push(object instance)
        {
            var type = instance.GetType();

            var item = new PushItem(null, type, null, instance);
            _objectItems.Add(item);
        }

        public void Push(string name, object instance)
        {
            var type = instance.GetType();

            var item = new PushItem(null, type, name, instance);
            _objectItems.Add(item);
        }


        // TI必须是接口, T必须是类, 并且T必须实现TI
        public void Push<TI, T>(string? name = null) where T : TI
        {
            var interfaceType = typeof(TI);
            var instanceType = typeof(T);

            var item = new PushItem(interfaceType, instanceType, name, null);
            _objectItems.Add(item);
        }


        public T Get<T>(string? name = null)
        {
            return (T)Get(new PopItem(typeof(T), name, 0));
        }


        private object Get(PopItem popItem)
        {
            if (popItem.Deep > MaxDeep)
                throw new Exception($"递归深度超过{MaxDeep}层, 可能存在循环依赖");

            if (popItem.Type.IsInterface)
            {
                var objectItems = _objectItems.Where(x => x.InterfaceType == popItem.Type).ToList();
                PushItem pushItem;
                if (objectItems.Count > 1) // 找到多个, 用name再匹配一次
                {
                    pushItem = objectItems.FirstOrDefault(x => x.InstanceName == popItem.Name) ?? objectItems[0];
                }
                else
                    pushItem = objectItems[0];

                return Get(new PopItem(pushItem.InstanceType!, popItem.Name, ++popItem.Deep));
            }

            if (popItem.Type.IsClass || popItem.Type.IsValueType)
            {
                var objectItems = _objectItems.Where(x => x.InstanceType == popItem.Type).ToList();
                if (objectItems.Count >= 1) // 找到多个, 用name再匹配一次
                {
                    PushItem pushItem;
                    if (objectItems.Count > 1)
                    {
                        pushItem = objectItems.FirstOrDefault(x => x.InstanceName == popItem.Name) ?? objectItems[0];
                    }
                    else
                        pushItem = objectItems[0];

                    if (pushItem.Instance == null)
                    {
                        // 获取所有的构造函数
                        var constructors = popItem.Type.GetConstructors();
                        // 获取参数最多的构造函数
                        var constructorInfo = constructors.OrderByDescending(p => p.GetParameters().Length).First();
                        var parameterInfos = constructorInfo.GetParameters();
                        var parameters = new object[parameterInfos.Length];
                        for (var i = 0; i < parameterInfos.Length; i++)
                        {
                            var parameterInfo = parameterInfos[i];
                            // 获取特性上的名字
                            var name = parameterInfo.GetCustomAttribute<InjectAttribute>()?.Name;
                            parameters[i] = Get(new PopItem(parameterInfo.ParameterType, name,
                                ++popItem.Deep));
                        }

                        pushItem.Instance = constructorInfo.Invoke(parameters);

                        // 判断是否有属性注入
                        var properties = popItem.Type.GetProperties().Where(p => p.IsDefined(typeof(InjectAttribute)));
                        foreach (var property in properties)
                        {
                            var propertyType = property.PropertyType;
                            var name = popItem.Name;
                            if (name.IsNullOrEmpty())
                                name = property.GetCustomAttribute<InjectAttribute>()?.Name;
                            var propertyValue = Get(new PopItem(propertyType, name, ++popItem.Deep));
                            property.SetValue(pushItem.Instance, propertyValue);
                        }
                    }

                    return pushItem.Instance!;
                }


                if (_parent != null)
                {
                    // 如果没有找到, 则从父容器中寻找
                    return _parent.Get(popItem);
                }
            }

         
            throw new Exception("无法创建实例");
        }


        public Container CreateChild()
        {
            var child = new Container(this);
            _children.Add(child);
            return child;
        }


        public void Dispose()
        {
            foreach (var child in _children)
            {
                child.Dispose();
            }

            // 从父类中移除自己
            if (_parent != null)
                _parent._children.Remove(this);

            _children.Clear();

            // 释放所有的实例
            foreach (var objectItem in _objectItems)
            {
                if (objectItem.Instance is IDisposable disposable)
                    disposable.Dispose();
            }

            _objectItems.Clear();
        }
    }
}