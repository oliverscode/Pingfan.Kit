using System.Collections.Generic;

namespace Pingfan.Kit
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Op<T>
    {
        /// <summary>
        /// 对象池列表
        /// </summary>
        public static ThreadSafeDictionary<string, T> Objs = new ThreadSafeDictionary<string, T>();

        /// <summary>
        /// 设置一个对象
        /// </summary>
        /// <param name="key">区分大小写</param>
        /// <param name="obj"></param>
        public static void Set(string key, T obj)
        {
            Objs[key] = obj;
        }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="key">区分大小写</param>
        /// <returns></returns>
        public static T Get(string key)
        {
            return Objs[key];
        }

        /// <summary>
        /// 一个对象是否存在
        /// </summary>
        /// <param name="key">区分大小写</param>
        /// <returns></returns>
        public static bool Has(string key)
        {
            return Objs.ContainsKey(key);
        }

        /// <summary>
        /// 移除某个对象
        /// </summary>
        /// <param name="key">区分大小写</param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            return Objs.TryRemove(key, out _);
        }
    }
}