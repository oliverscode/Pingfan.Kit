using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pingfan.Kit.Cache
{
    /// <summary>
    /// 进程缓存类
    /// </summary>
    public class CacheMemory : ICache
    {
        private readonly ConcurrentDictionary<string, CachedItem> _cacheMap =
            new ConcurrentDictionary<string, CachedItem>(StringComparer.OrdinalIgnoreCase);

        private class CachedItem
        {
            public DateTime ExpireDateTime;
            public object? Data;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheMemory()
        {
            Ticker.LoopWithTry(30 * 1000, AutoCleanExpiredCache);
        }


        /// <inheritdoc />
        public T? Get<T>(string key, T? defaultValue = default) 
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            return _tryGet<T>(key, out var item) ? item : defaultValue;
        }


        /// <inheritdoc />
        public bool TryGet<T>(string key, out T? result, T? defaultValue = default) 
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            if (_tryGet<T>(key, out var item))
            {
                result = item;
                return true;
            }

            result = defaultValue;
            return false;
        }

        /// <inheritdoc />
        public T? GetOrSet<T>(string key, Func<T?> valueFactory, float seconds = 1) 
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

            if (!_tryGet<T>(key, out var item))
            {
                var data = valueFactory();
                Set(key, data, seconds);
                return data;
            }

            return item;
        }


        /// <inheritdoc />
        public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> valueFactory, float seconds = 1)
            
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (valueFactory == null) throw new ArgumentNullException(nameof(valueFactory));

            if (!_tryGet<T>(key, out var item))
            {
                var data = await valueFactory();
                Set(key, data, seconds);
                return data;
            }

            return item;
        }


        /// <inheritdoc />
        public bool Has(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            return _cacheMap.ContainsKey(key);
        }


        /// <inheritdoc />
        public void Set<T>(string key, T? data, float seconds) 
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            var expirationDate = DateTime.UtcNow.AddSeconds(seconds);
            var cacheItem = new CachedItem { Data = data, ExpireDateTime = expirationDate };
            _cacheMap.AddOrUpdate(key, cacheItem, (k, v) => cacheItem);
        }


        /// <inheritdoc />
        public bool Set<T>(string key, T? data) 
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            if (_cacheMap.TryGetValue(key, out var value))
            {
                value.Data = data;
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public bool SetExpire(string key, float seconds = 1)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            if (_cacheMap.TryGetValue(key, out var item))
            {
                item.ExpireDateTime = DateTime.UtcNow.AddSeconds(seconds);
                return true;
            }

            return false;
        }


        /// <inheritdoc />
        public bool TryRemove<T>(string key, out T? result, T? defaultValue = default) 
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            if (_cacheMap.TryRemove(key, out var item))
            {
                result = (T?)item.Data;
                return true;
            }

            result = defaultValue;
            return false;
        }


        /// <inheritdoc />
        public void Clear()
        {
            _cacheMap.Clear();
        }


        /// <inheritdoc />
        public void Clear(string exp)
        {
            var regex = new Regex(exp, RegexOptions.IgnoreCase);

            foreach (var key in _cacheMap.Keys)
            {
                if (regex.IsMatch(key))
                {
                    _cacheMap.TryRemove(key, out _);
                }
            }
        }


        private void AutoCleanExpiredCache()
        {
            var currentTime = DateTime.UtcNow;

            foreach (var kvp in _cacheMap)
            {
                if (kvp.Value.ExpireDateTime <= currentTime)
                {
                    _cacheMap.TryRemove(kvp.Key, out _);
                }
            }
        }


        private bool _tryGet<T>(string key, out T? data) 
        {
            data = default;
            if (_cacheMap.TryGetValue(key, out var item) && item.ExpireDateTime > DateTime.UtcNow)
            {
                data = (T?)item.Data;
                return true;
            }

            return false;
        }
    }
}