using System;

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 对象注入, 只能在属性, 和形参上使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
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
}