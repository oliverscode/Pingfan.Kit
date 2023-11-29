using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pingfan.Kit.Inject.Interfaces;

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public class Container
    {
        internal readonly IContainer? _parent;
        internal readonly List<IContainer> _children;
        private List<ObjectItem> _objectItems = new List<ObjectItem>();

        /// <summary>
        /// 传递父容器
        /// </summary>
        /// <param name="parent"></param>
        public Container(IContainer? parent)
        {
            _parent = parent;
            _children = new List<IContainer>();
        }


        public bool IsRoot => _parent != null;

        public List<IContainer> Children => _children;

        public IContainer? Parent => _parent;


        public void Push<T>(string? name = null)
        {
            var type = typeof(T);
            if (type.IsInterface)
                throw new Exception("无法注入接口");

            var item = new ObjectItem(null, type, name, null);
            _objectItems.Add(item);
        }

        public void Push(object instance)
        {
            var type = instance.GetType();

            var item = new ObjectItem(null, type, null, instance);
            _objectItems.Add(item);
        }

        public void Push(string name, object instance)
        {
            var type = instance.GetType();

            var item = new ObjectItem(null, type, name, instance);
            _objectItems.Add(item);
        }


        // I必须是接口, T必须是类, 并且T必须实现I
        public void Push<I, T>(string? name = null) where T : I
        {
            var interfaceType = typeof(I);
            var instanceType = typeof(T);

            var item = new ObjectItem(interfaceType, instanceType, name, null);
            _objectItems.Add(item);
        }


        public T Get<T>(string? name = null, bool searchInParent = false)
        {
            return (T)Get(typeof(T), name, searchInParent);
        }


        public object Get(Type type, string? name, bool searchInParent)
        {
            if (type.IsInterface)
            {
                var objectItems = _objectItems.Where(x => x.InterfaceType == type).ToList();
                ObjectItem objectItem;
                if (objectItems.Count > 1) // 找到多个, 用name再匹配一次
                {
                    objectItem = objectItems.FirstOrDefault(x => x.InstanceName == name) ?? objectItems[0];
                }
                else
                    objectItem = objectItems[0];

                return Get(objectItem.InstanceType!, name, searchInParent);
            }
            else if (type.IsClass)
            {
                var objectItems = _objectItems.Where(x => x.InstanceType == type).ToList();
                ObjectItem objectItem;
                if (objectItems.Count > 1) // 找到多个, 用name再匹配一次
                {
                    objectItem = objectItems.FirstOrDefault(x => x.InstanceName == name) ?? objectItems[0];
                }
                else
                    objectItem = objectItems[0];


                if (objectItem.Instance == null)
                {
                    // 获取所有的构造函数
                    var constructors = type.GetConstructors();
                    // 获取参数最多的构造函数
                    var constructorInfo = constructors.OrderByDescending(p => p.GetParameters().Length).First();
                    var parameterInfos = constructorInfo.GetParameters();
                    var parameters = new object[parameterInfos.Length];
                    for (var i = 0; i < parameterInfos.Length; i++)
                    {
                        var parameterInfo = parameterInfos[i];
                        parameters[i] = Get(parameterInfo.ParameterType, name, searchInParent);
                    }

                    objectItem.Instance = constructorInfo.Invoke(parameters);
                }

                return objectItem.Instance!;
            }

            throw new Exception("无法创建实例");
        }


        /// <summary>
        /// 创建对象
        /// </summary>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            // 如果是接口, 就抛出异常
            if (type.IsInterface) throw new Exception($"类型{type.FullName}是接口, 无法创建实例");

            // 获取所有的构造函数
            var constructors = type.GetConstructors();
            // 获取参数最多的构造函数
            var constructorInfo = constructors.OrderByDescending(p => p.GetParameters().Length).First();
            var parameterInfos = constructorInfo.GetParameters();
            var parameters = new object[parameterInfos.Length];
            for (var i = 0; i < parameterInfos.Length; i++)
            {
                var parameterInfo = parameterInfos[i];
                parameters[i] = Resolve(parameterInfo.ParameterType);
            }

            return constructorInfo.Invoke(parameters);
        }
    }
}