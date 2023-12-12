using System;

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 对象注入, 可以在构造函数,属性,和形参上使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
        internal string? Name { get; }
        internal object? DefaultValue { get; }
        internal IContainer? Container { get; set; }

        /// <inheritdoc />
        public InjectAttribute()
        {
        }

        /// <summary>
        /// 按指定名称注入
        /// </summary>
        /// <param name="name">容器中的名称</param>
        public InjectAttribute(string name)
        {
            Name = name;
        }


        /// <summary>
        /// 指定容器里注入
        /// </summary>
        /// <param name="container"></param>
        public InjectAttribute(IContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// 支持默认值
        /// </summary>
        /// <param name="defaultValue"></param>
        public InjectAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }


        /// <summary>
        /// 支持默认值
        /// </summary>
        /// <param name="name">容器中的名字</param>
        /// <param name="defaultValue">注入失败的默认值</param>
        public InjectAttribute(string name, object defaultValue)
        {
            Name = name;
            DefaultValue = defaultValue;
        }
    }
}