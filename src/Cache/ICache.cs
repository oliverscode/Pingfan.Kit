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
        /// 获取缓存数据
        /// </summary>
        T? Get<T>(string key, T? defaultValue = null) where T : class?;
        
        /// <summary>
        /// 尝试从缓存中获取值。如果缓存中存在该键，则返回值和true；否则返回默认值和false。
        /// </summary>
        bool TryGet<T>(string key, out T? result, T? defaultValue = null) where T : class?;
        
        /// <summary>
        /// 获取或设置缓存项。如果缓存中存在该键，则直接返回值；否则使用valueFactory生成值，并设置缓存和过期时间，然后返回该值。
        /// </summary>
        T? GetOrSet<T>(string key, Func<T?> valueFactory, float seconds = 1) where T : class?;

        /// <summary>
        /// 异步获取或设置缓存项。如果缓存中存在该键，则直接返回值；否则使用valueFactory生成值，并设置缓存和过期时间，然后返回该值。
        /// </summary>
        Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> valueFactory, float seconds = 1) where T : class?;
        
        
        /// <summary>
        /// 检查指定的键是否在缓存中
        /// </summary>
        bool Has(string key);

        
        /// <summary>
        /// 设置缓存的值和过期时间。
        /// </summary>
        void Set<T>(string key, T? data, float seconds) where T : class?;


        /// <summary>
        /// 如果缓存中已经存在该键，设置该键的值。
        /// </summary>
        bool Set<T>(string key, T? data) where T : class?;

        /// <summary>
        /// 设置缓存项的过期时间。
        /// </summary>
        bool SetExpire(string key, float seconds = 1);

        /// <summary>
        /// 尝试移除缓存项。如果成功移除，返回被移除的项和true；否则返回默认值和false。
        /// </summary>
        bool TryRemove<T>(string key, out T? result, T? defaultValue = null) where T : class?;

        /// <summary>
        /// 清除所有缓存。
        /// </summary>
        void Clear();

        /// <summary>
        /// 清除所有匹配指定正则表达式的缓存项。
        /// </summary>
        void Clear(string pattern);


    }
}