using System;

namespace Pingfan.Kit.Inject.Attributes
{
    /// <summary>
    /// 对象注入, 只能在属性, 和形参上使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
        internal string? Name { get; }
        internal bool New { get; } = false;

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

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="isNew">是否每次都注入一个信息的</param>
        public InjectAttribute(bool isNew)
        {
            New = isNew;
        }
    }
}