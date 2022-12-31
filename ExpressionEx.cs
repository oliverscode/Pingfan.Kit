using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Pingfan.Kit.Caching;

namespace Pingfan.Kit
{
    public class ExpressionEx
    {
        /// <summary>
        /// 高性能创建一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>() where T : class, new()
        {
            var type = typeof(T);
            return (T) CreateInstance(type);
        }


        /// <summary>
        /// 高性能创建一个对象
        /// </summary>
        /// <returns></returns>
        public static object CreateInstance(Type type)
        {
            var fn = CacheMemory<Func<object>>.GetOrSet(type.FullName, () =>
            {
                var newExpression = Expression.New(type);
                var lambda = Expression.Lambda<Func<object>>(newExpression);
                return lambda.Compile();
            }, 60 * 60 * 24);
            return fn();
        }
    }
}