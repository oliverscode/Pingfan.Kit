using System;
using System.Collections.Generic;

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 容器接口
    /// </summary>
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// 每次注入的最大深度, 防止循环依赖
        /// </summary>
        int MaxDeep { get; set; }

        /// <summary>
        /// 没有找到对象时, 调用此方法
        /// </summary>
        Func<Type, object> OnNotFound { get; set; }

        /// <summary>
        /// 是否是根容器, 一般用作单例
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// 根容器
        /// </summary>
        IContainer Root { get; }

        /// <summary>
        /// 父容器
        /// </summary>
        IContainer? Parent { get; }

        /// <summary>
        /// 子容器列表
        /// </summary>
        List<IContainer> Children { get; }

        /// <summary>
        /// 注入实例类型
        /// </summary>
        /// <param name="name">如果重复可以别名区分, 区分大小写</param>
        /// <typeparam name="T">不能是接口</typeparam>
        void Push<T>(string? name = null);

        /// <summary>
        /// 注入实例
        /// </summary>
        /// <param name="instance">必须是实例</param>
        /// <param name="name">如果重复可以别名区分, 区分大小写</param>
        void Push(object instance, string? name = null);

        /// <summary>
        /// 注入接口和实例类型
        /// </summary>
        /// <param name="name">如果重复可以别名区分, 区分大小写</param>
        /// <typeparam name="TI">接口</typeparam>
        /// <typeparam name="T">接口的实力类型</typeparam>
        void Push<TI, T>(string? name = null) where T : TI;

        /// <summary>
        /// 获取容器中的实例
        /// </summary>
        /// <param name="name">如果实例重复可以别名区分, 区分大小写</param>
        /// <typeparam name="T">实例或者接口的类型</typeparam>
        /// <returns></returns>
        T Get<T>(string? name = null);

        /// <summary>
        /// 创建一个子容器
        /// </summary>
        /// <returns></returns>
        IContainer CreateContainer();
    }
}