using System;

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 注入的对象
    /// </summary>
    public class InjectPush
    {
        public Type? InterfaceType { get; }
        public Type InstanceType { get; }
        public string? InstanceName { get; }
        public object? Instance { get; set; }

        public InjectPush(
            Type? interfaceType,
            Type instanceType,
            string? instanceName,
            object? instance)
        {
            InterfaceType = interfaceType;
            InstanceType = instanceType;
          
            // InterfaceName = interfaceType.GetCustomAttribute<Attributes.NameAttribute>()?.Name;
            // InstanceName = instanceType.GetCustomAttribute<Attributes.NameAttribute>()?.Name;
            Instance = instance;

            // InterfaceName = interfaceName;
            InstanceName = instanceName;
        }
    }
}