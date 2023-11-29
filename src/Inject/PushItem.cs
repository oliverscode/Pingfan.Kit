using System;
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

        // public string? InterfaceName { get; }
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
}