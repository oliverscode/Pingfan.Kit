using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 注入的对象
    /// </summary>
    class PushItem
    {
        public Type? InterfaceType { get; }
        public Type? InstanceType { get; }

        public string? InstanceName { get; }
        public object? Instance { get; set; }


        public PushItem(
            Type? interfaceType,
            Type? instanceType,
            string? instanceName,
            object? instance)
        {
            InterfaceType = interfaceType;
            InstanceType = instanceType;
            if (interfaceType is null && instanceType is null)
                throw new Exception("接口和实例不能同时为空");

            // InterfaceName = interfaceType.GetCustomAttribute<Attributes.NameAttribute>()?.Name;
            // InstanceName = instanceType.GetCustomAttribute<Attributes.NameAttribute>()?.Name;
            Instance = instance;

            // InterfaceName = interfaceName;
            InstanceName = instanceName;
        }
    }


    /// <summary>
    /// 获取的对象
    /// </summary>
    class PopItem
    {
        public Type Type { get; set; }
        public string? Name { get; set; }
        public int Deep { get; set; }

        public PopItem(Type type, string? name, int deep)
        {
            Type = type;
            Name = name;
            Deep = deep;
        }
    }

    /// <summary>
    /// 对象注入, 可以在构造函数,属性,和形参上使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
        internal string? Name { get; }

        public InjectAttribute()
        {
        }

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="name">容器中的名称</param>
        public InjectAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// 容器事件接口
    /// </summary>
    public interface IContainerReady
    {
        /// <summary>
        /// 注入完成后调用
        /// </summary>
        void OnContainerReady();
    }


    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public class Container : IContainer, IDisposable
    {
        private readonly IContainer? _parent;
        private readonly List<IContainer> _children;
        private readonly int _currentDeep; // 当前深度, 用于判断递归深度
        private readonly List<PushItem> _objectItems = new List<PushItem>();
        private Func<Type, object> _onNotFound = type => throw new Exception($"无法创建实例 {type}");


        public int MaxDeep { get; set; } = 20;
        public bool IsRoot => _parent == null;
        public IContainer Root => _parent?.Root ?? this;
        public IContainer? Parent => _parent;
        public List<IContainer> Children => _children;

        public Func<Type, object> OnNotFound
        {
            get => _onNotFound;
            set
            {
                if (IsRoot == false)
                {
                    throw new Exception("因为找不到会向上搜索, 所有只有根容器才能设置OnNotFound");
                }

                _onNotFound = value;
            }
        }


        /// <summary>
        /// 创建一个容器
        /// </summary>
        /// <param name="parent">默认为根容器</param>
        public Container(Container? parent = null)
        {
            _parent = parent;
            _children = new List<IContainer>();
            this._currentDeep = parent?._currentDeep + 1 ?? 0;
            if (this._currentDeep > this.MaxDeep)
                throw new Exception($"递归深度超过{MaxDeep}层, 可能存在循环依赖");

            if (parent != null)
            {
                this.MaxDeep = parent.MaxDeep;
            }
        }

        public void Push<T>(string? name = null)
        {
            var type = typeof(T);
            if (type.IsInterface)
                throw new Exception("无法注入接口");

            {
                var item = new PushItem(null, type, name, null);
                _objectItems.Add(item);
            }
        }

        public void Push(object instance, string? name = null)
        {
            var type = instance.GetType();

            {
                var item = new PushItem(null, type, name, instance);
                _objectItems.Add(item);
            }
        }

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
                if (objectItems.Count >= 1) // 找到多个, 用name再匹配一次
                {
                    PushItem pushItem;
                    if (objectItems.Count > 1)
                        pushItem = objectItems.FirstOrDefault(x => x.InstanceName == popItem.Name) ?? objectItems[0];
                    else
                        pushItem = objectItems[0];
                    return Get(new PopItem(pushItem.InstanceType!, popItem.Name, ++popItem.Deep));
                }

                if (_parent != null)
                {
                    // 如果没有找到, 则从父容器中寻找
                    return ((Container)_parent).Get(popItem);
                }
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
                        // 先判断是否有Inject特性
                        var constructorInfo = constructors.FirstOrDefault(p => p.IsDefined(typeof(InjectAttribute)));
                        if (constructorInfo == null)
                            // 获取参数最多的构造函数
                            constructorInfo = constructors.OrderByDescending(p => p.GetParameters().Length).First();

                        var parameterInfos = constructorInfo.GetParameters();
                        var parameters = new object[parameterInfos.Length];
                        for (var i = 0; i < parameterInfos.Length; i++)
                        {
                            popItem.Deep++;
                            var parameterInfo = parameterInfos[i];
                            if (parameterInfo.ParameterType == this.GetType())
                                throw new Exception("无法在构造参数中注入容器, 请使用属性注入");

                            // 获取特性上的名字
                            var name = popItem.Name;
                            if (name.IsNullOrEmpty())
                                name = parameterInfo.GetCustomAttribute<InjectAttribute>()?.Name;

                            parameters[i] = Get(new PopItem(parameterInfo.ParameterType, name,
                                popItem.Deep));
                        }

                        pushItem.Instance = constructorInfo.Invoke(parameters);

                        // 判断是否有属性注入
                        InjectProperty(popItem, pushItem.Instance);

                        // 注入完成 是否继承IContainerReady
                        if (pushItem.Instance is IContainerReady containerReady)
                        {
                            containerReady.OnContainerReady();
                        }
                    }


                    return pushItem.Instance!;
                }


                if (_parent != null)
                {
                    // 如果没有找到, 则从父容器中寻找
                    var obj = ((Container)_parent).Get(popItem);
                    // 判断是否有属性注入
                    InjectProperty(popItem, obj);
                    if (obj is IContainerReady containerReady)
                    {
                        containerReady.OnContainerReady();
                    }

                    return obj;
                }
            }
            return this.OnNotFound(popItem.Type);
        }


        private void InjectProperty(PopItem popItem, object instance)
        {
            var properties = popItem.Type.GetProperties().Where(p => p.IsDefined(typeof(InjectAttribute)));
            foreach (var property in properties)
            {
                popItem.Deep++;
                var propertyType = property.PropertyType;
                // 如果是注入自己, 则注入当前实例
                if (propertyType == typeof(IContainer))
                {
                    property.SetValue(instance, this);
                }
                else
                {
                    // 获取特性上的名字
                    var name = popItem.Name;
                    if (name.IsNullOrEmpty())
                        name = property.GetCustomAttribute<InjectAttribute>()?.Name;
                    var propertyValue =
                        Get(new PopItem(propertyType, name, popItem.Deep));
                    property.SetValue(instance, propertyValue);
                }
            }
        }


        /// <summary>
        /// 创建一个子容器
        /// </summary>
        /// <returns></returns>
        public IContainer CreateChild()
        {
            var child = new Container(this);
            return child;
        }


        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var child in _children)
            {
                // // 排除自身容器
                // if (child == this)
                //     continue;
                child.Dispose();
            }


            // 从父类中移除自己
            if (_parent != null)
                _parent.Children.Remove(this);

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