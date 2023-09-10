using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 进程缓存类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class CacheMemory<T>
    {
        private static readonly ConcurrentDictionary<string, CachedItem> CacheMap =
            new ConcurrentDictionary<string, CachedItem>(StringComparer.OrdinalIgnoreCase);

        private class CachedItem
        {
            public DateTime ExpireDateTime { get; set; }
            public T Data { get; set; }
        }

        static CacheMemory()
        {
            Task.Run(async () => 
            {
                while (true)
                {
                    CleanExpiredCache();
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            });
        }

        /// <summary>
        /// 获取缓存数据。如果缓存中不存在该键，则返回默认值。
        /// </summary>
        public static T Get(string key, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            return _TryGet(key, out var item) ? item : defaultValue;
        }

        /// <summary>
        /// 检查指定的键是否在缓存中。
        /// </summary>
        public static bool HasKey(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            return CacheMap.ContainsKey(key);
        }

        /// <summary>
        /// 尝试从缓存中获取值。如果缓存中存在该键，则返回值和true；否则返回默认值和false。
        /// </summary>
        public static bool TryGet(string key, out T result, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            if (_TryGet(key, out var item))
            {
                result = item;
                return true;
            }

            result = defaultValue;
            return false;
        }

        /// <summary>
        /// 设置缓存的值和过期时间。
        /// </summary>
        public static void Set(string key, T data, double seconds)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            var expirationDate = DateTime.UtcNow.AddSeconds(seconds);
            var cacheItem = new CachedItem { Data = data, ExpireDateTime = expirationDate };

            CacheMap.AddOrUpdate(key, cacheItem, (k, v) => cacheItem);
        }

        /// <summary>
        /// 如果缓存中已经存在该键，设置该键的值。
        /// </summary>
        public static bool Set(string key, T data)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            if (CacheMap.TryGetValue(key, out var value))
            {
                value.Data = data;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 设置缓存项的过期时间。
        /// </summary>
        public static bool SetExpire(string key, double seconds = 1)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            if (CacheMap.TryGetValue(key, out var item))
            {
                item.ExpireDateTime = DateTime.UtcNow.AddSeconds(seconds);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 尝试移除缓存项。如果成功移除，返回被移除的项和true；否则返回默认值和false。
        /// </summary>
        public static bool TryRemove(string key, out T result, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            if (CacheMap.TryRemove(key, out var item))
            {
                result = item.Data;
                return true;
            }

            result = defaultValue;
            return false;
        }

        /// <summary>
        /// 清除所有缓存。
        /// </summary>
        public static void Clear()
        {
            CacheMap.Clear();
        }

        /// <summary>
        /// 清除所有匹配指定正则表达式的缓存项。
        /// </summary>
        public static void Clear(string exp)
        {
            var regex = new Regex(exp, RegexOptions.IgnoreCase);

            foreach (var key in CacheMap.Keys)
            {
                if (regex.IsMatch(key))
                {
                    CacheMap.TryRemove(key, out _);
                }
            }
        }

        /// <summary>
        /// 清理过期的缓存项。
        /// </summary>
        private static void CleanExpiredCache()
        {
            var currentTime = DateTime.UtcNow;

            foreach (var kvp in CacheMap)
            {
                if (kvp.Value.ExpireDateTime <= currentTime)
                {
                    CacheMap.TryRemove(kvp.Key, out _);
                }
            }
        }

        /// <summary>
        /// 获取或设置缓存项。如果缓存中存在该键，则直接返回值；否则使用valueFactory生成值，并设置缓存和过期时间，然后返回该值。
        /// </summary>
        public static T GetOrSet(string key, Func<T> valueFactory, double seconds = 1)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

            if (!_TryGet(key, out var item))
            {
                var data = valueFactory();
                Set(key, data, seconds);
                return data;
            }

            return item;
        }

        /// <summary>
        /// 异步获取或设置缓存项。如果缓存中存在该键，则直接返回值；否则使用valueFactory生成值，并设置缓存和过期时间，然后返回该值。
        /// </summary>
        public static async Task<T> GetOrSetAsync(string key, Func<Task<T>> valueFactory, double seconds = 1)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

            if (!_TryGet(key, out var item))
            {
                var data = await valueFactory();
                Set(key, data, seconds);
                return data;
            }

            return item;
        }

        private static bool _TryGet(string key, out T data)
        {
            data = default;
            if (CacheMap.TryGetValue(key, out var item) && item.ExpireDateTime > DateTime.UtcNow)
            {
                data = item.Data;
                return true;
            }

            return false;
        }
    }
}