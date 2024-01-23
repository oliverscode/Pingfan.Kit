using System;
using System.Collections.Concurrent;
using System.Reflection;
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 依赖注入接口的缓存
    /// </summary>
    public static class InjectCache
    {
        // 线程安全的集合
        private static ConcurrentDictionary<MethodInfo, ParameterInfo[]> ParameterInfoCache { get; } =
            new ConcurrentDictionary<MethodInfo, ParameterInfo[]>();

        /// <summary>
        /// 获取方法的参数, 并且缓存
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static ParameterInfo[] GetParametersByCache(this MethodInfo methodInfo)
        {
            if (ParameterInfoCache.TryGetValue(methodInfo, out var parameterInfos))
                return parameterInfos;

            parameterInfos = methodInfo.GetParameters();
            ParameterInfoCache.TryAdd(methodInfo, parameterInfos);
            return parameterInfos;
        }


        // 线程安全的集合
        private static ConcurrentDictionary<Type, ConstructorInfo[]> ConstructorInfoCache { get; } =
            new ConcurrentDictionary<Type, ConstructorInfo[]>();

        /// <summary>
        /// 获取构造函数的参数, 并且缓存
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ConstructorInfo[] GetConstructorsByCache(this Type type)
        {
            if (ConstructorInfoCache.TryGetValue(type, out var constructorInfos))
                return constructorInfos;

            constructorInfos = type.GetConstructors();
            ConstructorInfoCache.TryAdd(type, constructorInfos);
            return constructorInfos;
        }


        // 线程安全的集合
        private static ConcurrentDictionary<Type, PropertyInfo[]> PropertyInfoCache { get; } =
            new ConcurrentDictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// 获取类型的属性, 并且缓存
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertiesByCache(this Type type)
        {
            if (PropertyInfoCache.TryGetValue(type, out var propertyInfos))
                return propertyInfos;

            propertyInfos = type.GetProperties();
            PropertyInfoCache.TryAdd(type, propertyInfos);
            return propertyInfos;
        }


        // 线程安全的集合
        private static ConcurrentDictionary<ConstructorInfo, ParameterInfo[]> FieldInfoCache { get; } =
            new ConcurrentDictionary<ConstructorInfo, ParameterInfo[]>();

        /// <summary>
        /// 获取构造函数的参数, 并且缓存
        /// </summary>
        /// <param name="constructorInfo"></param>
        /// <returns></returns>
        public static ParameterInfo[] GetParametersByCache(this ConstructorInfo constructorInfo)
        {
            if (FieldInfoCache.TryGetValue(constructorInfo, out var parameterInfos))
                return parameterInfos;

            parameterInfos = constructorInfo.GetParameters();
            FieldInfoCache.TryAdd(constructorInfo, parameterInfos);
            return parameterInfos;
        }
    }


    /// <summary>
    /// 依赖注入接口的缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class InjectCache<T> where T : Attribute
    {
       
        // 线程安全的集合
        private static ConcurrentDictionary<PropertyInfo, T> PropertyInfoCache { get; } =
            new ConcurrentDictionary<PropertyInfo, T>();

        public static T? GetCustomAttributeCache(PropertyInfo info)
        {
            if (PropertyInfoCache.TryGetValue(info, out var result))
                return result;

            result = info.GetCustomAttribute<T>();
            PropertyInfoCache.TryAdd(info, result);
            return result;
        }
        
        
        // 线程安全的集合
        private static ConcurrentDictionary<ParameterInfo, T> ParameterInfoCache { get; } =
            new ConcurrentDictionary<ParameterInfo, T>();

        public static T? GetCustomAttributeCache(ParameterInfo info)
        {
            if (ParameterInfoCache.TryGetValue(info, out var result))
                return result;

            result = info.GetCustomAttribute<T>();
            ParameterInfoCache.TryAdd(info, result);
            return result;
        }
        
        
        // 线程安全的集合
        private static ConcurrentDictionary<ConstructorInfo, T> ConstructorInfoCache { get; } =
            new ConcurrentDictionary<ConstructorInfo, T>();

        public static T? GetCustomAttributeCache(ConstructorInfo info)
        {
            if (ConstructorInfoCache.TryGetValue(info, out var result))
                return result;

            result = info.GetCustomAttribute<T>();
            ConstructorInfoCache.TryAdd(info, result);
            return result;
        }
        
    }
}