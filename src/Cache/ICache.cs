using System;
using System.Threading.Tasks;

namespace Pingfan.Kit.Cache
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 检查指定的键是否在缓存中
        /// </summary>
        bool Has(string key);

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        T Get<T>(string key);

        /// <summary>
        /// 设置缓存的值和过期时间。
        /// </summary>
        void Set<T>(string key, T data, float seconds = 1);

        /// <summary>
        /// 尝试从缓存中获取值。如果缓存中存在该键，则返回值和true；否则返回默认值和false。
        /// </summary>
        bool TryGet<T>(string key, out T result); 

        /// <summary>
        /// 设置缓存项的过期时间。
        /// </summary>
        bool SetExpire(string key, float seconds = 1);

        /// <summary>
        /// 获取或设置缓存项。如果缓存中存在该键，则直接返回值；否则使用valueFactory生成值，并设置缓存和过期时间，然后返回该值。
        /// </summary>
        T GetOrSet<T>(string key, Func<T> valueFactory, float seconds = 1);

        /// <summary>
        /// 异步获取或设置缓存项。如果缓存中存在该键，则直接返回值；否则使用valueFactory生成值，并设置缓存和过期时间，然后返回该值。
        /// </summary>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, float seconds = 1); 

        /// <summary>
        /// 移除缓存。如果成功移除返回true; 否则返回false。
        /// </summary>
        bool Remove(string key);

        /// <summary>
        /// 清除当前name所有缓存。
        /// </summary>
        void Clear();
    }
}