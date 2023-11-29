using System;
using System.Collections.Generic;

namespace Pingfan.Kit.Inject.Interfaces
{
    /// <summary>
    /// 依赖注入容器接口
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// 是否是根容器, 一般情况下, 根容器不会被销毁, 并且用作全局容器
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// 子容器
        /// </summary>
        List<IContainer> Children { get; }

        /// <summary>
        /// 父容器
        /// </summary>
        IContainer? Parent { get; }

        /// <summary>
        /// 注入接口以及实例, T必须是类, 并且T必须实现I, I必须是接口
        /// </summary>
        void Push<I, T>() where T : I;

        /// <summary>
        /// 注入实例
        /// </summary>
        void Push<T>();

        /// <summary>
        /// 注入实例
        /// </summary>
        /// <param name="instance">任意类型</param>
        /// <typeparam name="T"></typeparam>
        void Push<T>(T instance);

        /// <summary>
        /// 获取当前容器中实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="searchInParent">如果本容器没有, 是否从父类中寻找</param>
        /// <returns></returns>
        object Get(Type type, bool searchInParent = false);
    }
}