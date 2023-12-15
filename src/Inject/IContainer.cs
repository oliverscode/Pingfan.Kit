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
        /// 容器名
        /// </summary>
        string? Name { get; set; }

        /// <summary>
        /// 每次注入的最大深度, 防止循环依赖
        /// </summary>
        int MaxDeep { get; set; }

        /// <summary>
        /// 没有找到对象时, 调用此方法
        /// </summary>
        Func<InjectPop, object> OnNotFound { get; set; }

        /// <summary>
        /// 是否是根容器, 一般用作单例
        /// </summary>
        bool IsRoot { get; }

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
        /// <param name="name">如果重复可以别名区分, 不区分大小写</param>
        /// <typeparam name="T">不能是接口</typeparam>
        void Register<T>(string? name = null);

        /// <summary>
        /// 注入实例类型
        /// </summary>
        /// <param name="instance">必须是实例</param>
        /// <param name="name">如果重复可以别名区分, 不区分大小写</param>
        /// <typeparam name="T">接口的实例类型</typeparam>
        void Register<T>(T instance, string? name = null);


        /// <summary>
        /// 注入接口和实例类型
        /// </summary>
        /// <param name="instance">必须是实例</param>
        /// <param name="name">如果重复可以别名区分, 不区分大小写</param>
        /// <typeparam name="TI">接口</typeparam>
        /// <typeparam name="T">接口的实例类型</typeparam>
        void Register<TI, T>(T instance, string? name = null) where T : TI;


        /// <summary>
        /// 注入接口和实例类型
        /// </summary>
        /// <param name="name">如果重复可以别名区分, 不区分大小写</param>
        /// <typeparam name="TI">接口</typeparam>
        /// <typeparam name="T">接口的实例类型</typeparam>
        void Register<TI, T>(string? name = null) where T : TI;

        /// <summary>
        /// 注入一个实例类型
        /// </summary>
        /// <param name="instanceType">实例的类型</param>
        /// <param name="name">如果重复可以别名区分, 不区分大小写</param>
        void Register(Type instanceType, string? name = null);

        /// <summary>
        /// 注入一个接口和实例
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <param name="instance">实例</param>
        /// <param name="name">如果重复可以别名区分, 不区分大小写</param>
        void Register(Type interfaceType, object instance, string? name = null);

        /// <summary>
        /// 注册一个接口类型, 和一个实例类型
        /// </summary>
        /// <param name="interfaceType">接口的类型</param>
        /// <param name="instanceType">实例的类型</param>
        /// <param name="name">如果重复可以别名区分, 不区分大小写</param>
        void Register(Type interfaceType, Type instanceType, string? name = null);

        /// <summary>
        /// 注册一个实例类型
        /// </summary>
        /// <param name="instanceType">实例的类型</param>
        /// <param name="name">如果重复可以别名区分, 不区分大小写</param>
        void Register<T>(Type instanceType, string? name = null);

        /// <summary>
        /// 获取容器中的实例
        /// </summary>
        /// <param name="name">如果实例重复可以别名区分, 不区分大小写</param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T">实例或者接口的类型</typeparam>
        /// <returns></returns>
        T Get<T>(string? name = null, object? defaultValue = null);

        /// <summary>
        /// 获取容器中的实例
        /// </summary>
        /// <param name="instanceType"></param>
        /// <param name="name">如果实例重复可以别名区分, 不区分大小写</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        object Get(Type instanceType, string? name = null, object? defaultValue = null);

        /// <summary>
        /// 是否有指定类型的实例
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T">接口或者实例类型</typeparam>
        /// <returns></returns>
        bool Has<T>(string? name = null);

        /// <summary>
        /// 是否有指定类型的实例
        /// </summary>
        /// <param name="type">接口或者实例类型</param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Has(Type type, string? name = null);

        /// <summary>
        /// 取消注册指定类型的接口和实例类型
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T">接口或者实例类型</typeparam>
        /// <returns></returns>
        void Pop<T>(string? name = null);

        /// <summary>
        /// 取消注册指定类型的接口和实例类型
        /// </summary>
        /// <param name="type">接口或者实例类型</param>
        /// <param name="name"></param>
        /// <returns></returns>
        void Pop(Type type, string? name = null);


        /// <summary>
        /// 删除指定类型的接口和实例
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T">接口或者实例类型</typeparam>
        /// <returns></returns>
        void Delete<T>(string? name = null);

        /// <summary>
        /// 删除指定类型的接口和实例
        /// </summary>
        /// <param name="type">接口或者实例类型</param>
        /// <param name="name"></param>
        /// <returns></returns>
        void Delete(Type type, string? name = null);

        /// <summary>
        /// 注入实例, 并且再此获取这个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T New<T>() where T : class;

        /// <summary>
        /// 注入实例, 并且再此获取这个值
        /// </summary>
        object New(Type type);

        /// <summary>
        /// 注入属性
        /// </summary>
        /// <param name="instance"></param>
        void InjectProperties(object instance);

        // /// <summary>
        // /// 调用一个方法, 并注入参数
        // /// </summary>
        // /// <param name="instance"></param>
        // /// <param name="methodInfo"></param>
        // /// <returns></returns>
        // object Invoke(object instance, MethodInfo methodInfo);

        /// <summary>
        /// 创建一个子容器
        /// </summary>
        /// <returns></returns>
        IContainer CreateContainer(string? name = null);

        /// <summary>
        /// 查找容器
        /// </summary>
        /// <param name="name">区分大小写</param>
        /// <returns></returns>
        IContainer? FindContainer(string name);
    }
}