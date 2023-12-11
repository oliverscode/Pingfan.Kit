using System;

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 获取的对象
    /// </summary>
    public class InjectPop
    {
        public Type Type { get; set; }
        public string? Name { get; set; }
        public int Deep { get; set; }
        public object? DefaultValue { get; set; }

        public InjectPop(Type type, string? name, int deep, object? defaultValue)
        {
            Type = type;
            Name = name;
            Deep = deep;
            DefaultValue = defaultValue;
        }
    }
}