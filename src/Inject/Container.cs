using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public class Container : IContainer
    {
        private readonly int _currentDeep; // 当前深度, 用于判断递归深度
        private readonly List<InjectPush> _objectItems = new List<InjectPush>();
        internal readonly object Lock = new object();

        private Func<InjectPop, object> _onNotFound = pop =>
            throw new InjectNotRegisteredException($"{pop.Name}未被注册, 类型:{pop.Type}", pop);

        /// <inheritdoc />
        public int MaxDeep { get; set; } = 20;

        /// <inheritdoc />
        public bool IsRoot => Parent == null;

        /// <inheritdoc />
        public IContainer Root => Parent?.Root ?? this;

        /// <inheritdoc />
        public IContainer? Parent { get; }

        /// <inheritdoc />
        public List<IContainer> Children { get; }

        /// <inheritdoc />
        public Func<InjectPop, object> OnNotFound
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
            Parent = parent;
            Children = new List<IContainer>();
            this._currentDeep = parent?._currentDeep + 1 ?? 0;
            if (this._currentDeep > this.MaxDeep)
                throw new Exception($"递归深度超过{MaxDeep}层, 可能存在循环依赖");

            if (parent != null)
            {
                this.MaxDeep = parent.MaxDeep;
            }
        }


        /// <inheritdoc />
        public void Push<T>(string? name = null)
        {
            var type = typeof(T);
            if (type.IsInterface)
                throw new Exception("无法注入接口");

            Push(new InjectPush(null, type, name, null));
        }

        /// <inheritdoc />
        public void Push<T>(T instance, string? name = null)
        {
            var type = typeof(T);
            if (type.IsInterface)
                throw new Exception("无法注入接口");

            Push(new InjectPush(null, type, name, instance));
        }


        /// <inheritdoc />
        public void Push<TI, T>(T instance, string? name = null) where T : TI
        {
            var interfaceType = typeof(TI);
            var instanceType = typeof(T);
            Push(new InjectPush(interfaceType, instanceType, name, instance));
        }


        /// <inheritdoc />
        public void Push<TI, T>(string? name = null) where T : TI
        {
            var interfaceType = typeof(TI);
            var instanceType = typeof(T);
            Push(new InjectPush(interfaceType, instanceType, name, null));
        }

        /// <inheritdoc />
        public void Push(Type instanceType, string? name = null)
        {
            Push(new InjectPush(null, instanceType, name, null));
        }

        /// <inheritdoc />
        public void Push(Type interfaceType, object instance, string? name = null)
        {
            Push(new InjectPush(interfaceType, instance.GetType(), name, instance));
        }

        /// <inheritdoc />
        public void Push(Type interfaceType, Type instanceType, string? name = null)
        {
            Push(new InjectPush(interfaceType, instanceType, null, null));
        }

        /// <inheritdoc />
        public void Push<T>(Type instanceType, string? name = null)
        {
            var interfaceType = typeof(T);
            Push(new InjectPush(interfaceType, instanceType, name, null));
        }

        private void Push(InjectPush push)
        {
            if (push.InterfaceType?.IsInterface == false)
                throw new ArgumentException($"{nameof(push.InterfaceType)}必须是接口");
            if (push.InstanceType.IsInterface == true)
                throw new ArgumentException($"{nameof(push.InstanceType)}不能是接口");
            if (push.InstanceType == null)
                throw new ArgumentException($"{nameof(push.InstanceType)}不能为null");

            if (push.InterfaceType != null)
            {
                // 判断instanceType的类型是否是interfaceType的子类
                if (push.InterfaceType.IsAssignableFrom(push.InstanceType) == false)
                    throw new Exception($"无法注入 {push.InstanceType} 到 {push.InterfaceType}");
            }

            // 判断接口类型和实例类型以及名字是否存在, 如果存在就把实例更新一下
            var item = _objectItems.FirstOrDefault(x =>
                x.InterfaceType == push.InterfaceType && x.InstanceType == push.InstanceType &&
                string.Equals(x.InstanceName, push.InstanceName, StringComparison.OrdinalIgnoreCase));
            if (item != null)
                item.Instance = push.Instance;
            else
                _objectItems.Add(push);
        }


        /// <inheritdoc />
        public T Get<T>(string? name = null, object? defaultValue = null)
        {
            return (T)Get(new InjectPop(typeof(T), name, 0, defaultValue));
        }

        /// <inheritdoc />
        public object Get(Type instanceType, string? name = null, object? defaultValue = null)
        {
            return Get(new InjectPop(instanceType, name, 0, defaultValue));
        }

        private object Get(InjectPop injectPop)
        {
            injectPop.Deep++;
            if (injectPop.Deep > MaxDeep)
                throw new Exception($"递归深度超过{MaxDeep}层, 可能存在循环依赖");

            var objectItems = _objectItems
                .Where(x => (x.InterfaceType == injectPop.Type || x.InstanceType == injectPop.Type) &&
                            x.InstanceName == injectPop.Name).ToList();


            // 有多个就返回最后一个
            if (objectItems.Count >= 1)
            {
                var push = objectItems[objectItems.Count - 1];
                if (push.Instance == null)
                {
                    // 获取所有的构造函数
                    var constructors = push.InstanceType.GetConstructorsByCache();


                    ConstructorInfo? constructorInfo = null;
                    // 先判断是否有Inject特性
                    foreach (var info in constructors)
                    {
                        var attr = InjectCache<InjectAttribute>.GetCustomAttributeCache(info);
                        if (attr == null) continue;
                        constructorInfo = info;
                        break;
                    }

                    // 获取参数最多的构造函数
                    if (constructorInfo == null)
                        constructorInfo = constructors.OrderByDescending(p => p.GetParametersByCache().Length)
                            .First();

                    var parameterInfos = constructorInfo.GetParametersByCache();
                    var parameters = new object[parameterInfos.Length];
                    for (var i = 0; i < parameterInfos.Length; i++)
                    {
                        var parameterInfo = parameterInfos[i];
                        {
                            if (parameterInfo.ParameterType == this.GetType())
                                throw new Exception("无法在构造参数中注入容器, 请使用属性注入");
                        }
                        {
                            // 判断parameterInfo.ParameterType 是否是this的子类
                            // if (parameterInfo.ParameterType.IsAssignableFrom(this.GetType()))
                            // if (parameterInfo.ParameterType.Name == typeof(IContainer).Name)
                            // {
                            //     parameters[i] = this;
                            //     continue;
                            // }
                        }


                        var attr = InjectCache<InjectAttribute>.GetCustomAttributeCache(parameterInfo);

                        // 获取名字
                        var name = injectPop.Name ?? attr?.Name ?? null;
                        // 获取默认值
                        var defaultValue = injectPop.DefaultValue ?? attr?.DefaultValue;
                        parameters[i] = Get(new InjectPop(parameterInfo.ParameterType, name, injectPop.Deep,
                            defaultValue));
                    }

                    push.Instance = constructorInfo.Invoke(parameters);

                    // 注入属性
                    var properties = injectPop.Type.GetPropertiesByCache();
                    foreach (var property in properties)
                    {
                        var attr = InjectCache<InjectAttribute>.GetCustomAttributeCache(property);
                        if (attr == null) continue;

                        var propertyType = property.PropertyType;
                        // 判断propertyType是否是IContainer的实现类或者子类
                        if (typeof(IContainer) == property || typeof(IContainer).IsAssignableFrom(propertyType))
                        {
                            property.SetValue(push.Instance, this);
                        }
                        else
                        {
                            var name = injectPop.Name ?? attr?.Name;
                            var defaultValue = injectPop.DefaultValue ?? attr?.DefaultValue;
                            var propertyValue = Get(new InjectPop(propertyType, name, injectPop.Deep, defaultValue));
                            property.SetValue(push.Instance, propertyValue);
                        }
                    }


                    if (push.Instance is IContainerReady containerReady)
                    {
                        containerReady.OnContainerReady();
                    }
                }

                return push.Instance!;
            }

            if (Parent != null)
            {
                return ((Container)Parent!).Get(injectPop);
            }

            if (injectPop.DefaultValue != null)
                return injectPop.DefaultValue;

            return this.OnNotFound(injectPop);
        }


        /// <inheritdoc />
        public bool Has<T>(string? name = null)
        {
            var type = typeof(T);
            return Has(type, name);
        }

        /// <inheritdoc />
        public bool Has(Type type, string? name = null)
        {
            var result = _objectItems.Any(x =>
                (x.InstanceType == type || x.InterfaceType == type) && x.InstanceName == name);
            if (result)
                return true;
            if (result == false && Parent != null)
                return Parent.Has(type, name);
            return false;
        }

        /// <inheritdoc />
        public void Pop<T>(string? name = null)
        {
            var type = typeof(T);
            Pop(type, name);
        }

        /// <inheritdoc />
        public void Pop(Type type, string? name = null)
        {
            _objectItems.RemoveAll(x =>
                (x.InstanceType == type || x.InterfaceType == type) && x.InstanceName == name);

            if (Parent != null)
                Parent.Pop(type, name);
        }

        /// <inheritdoc />
        public void Delete<T>(string? name = null)
        {
            var type = typeof(T);
            Delete(type, name);
        }

        /// <inheritdoc />
        public void Delete(Type type, string? name = null)
        {
            foreach (var injectPush in _objectItems.Where(x =>
                         (x.InstanceType == type || x.InterfaceType == type) && x.InstanceName == name))
            {
                injectPush.Instance = null;
            }
        }

        /// <inheritdoc />
        public T New<T>() where T : class
        {
            var type = typeof(T);
            return (T)New(type);
        }

        /// <inheritdoc />
        public object New(Type type)
        {
            if (type.IsInterface)
                throw new Exception("无法创建接口");
            Push(type);
            return Get(type);
        }

        /// <inheritdoc />
        public IContainer CreateContainer()
        {
            var child = new Container(this);
            this.Children.Add(child);
            return child;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            lock (((Container)Root).Lock)
            {
                foreach (var child in Children)
                {
                    child.Dispose();
                }

                Children.Clear();


                // 从父类中移除自己
                if (Parent != null)
                    Parent.Children.Remove(this);


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
}