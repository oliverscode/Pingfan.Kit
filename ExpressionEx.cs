using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Pingfan.Kit
{
    public class ExpressionEx
    {
        /// <summary>
        /// 高性能创建一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>(params object[] parms) where T : class, new()
        {
            var type = typeof(T);
            return (T)CreateInstance(type, parms);
        }


        /// <summary>
        /// 高性能创建一个对象
        /// </summary>
        /// <returns></returns>
        public static object CreateInstance(Type type, params object[] parms)
        {
            var fn = CacheMemory<Func<object>>.GetOrSet(type.FullName, () =>
            {
                Expression<Func<object>> lambda = () => Activator.CreateInstance(type, parms);
                return lambda.Compile();
            }, 60 * 60 * 24);
            return fn();
        }
    }
}