using System;

namespace Pingfan.Kit.Inject.Attributes
{
    public class NameAttribute : Attribute
    {
        public NameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 注入的名字
        /// </summary>
        public string Name { get; set; }
    }
}