using System;
using System.Linq;
using System.Reflection;

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 注入的对象
    /// </summary>
    class ObjectItem
    {
        public Type? InterfaceType { get; }
        public Type? InstanceType { get; }

        // public string? InterfaceName { get; }
        public string? InstanceName { get; }
        public object? Instance { get; set; }


        public ObjectItem(
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
}