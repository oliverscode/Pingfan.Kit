using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 内存缓存库
    /// </summary>
    public static class CacheMemory<T>
    {
        private static readonly ConcurrentDictionary<string, CachedItem> _cacheMap =
            new ConcurrentDictionary<string, CachedItem>(StringComparer.OrdinalIgnoreCase);

        private class CachedItem
        {
            public DateTime ExpireDateTime { get; set; }
            public T Data { get; set; }
        }

        static CacheMemory()
        {
            // 每5秒清理一下缓存
            Timer.SetIntervalWithTry(5 * 1000, CleanExpiredCache);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T Get(string key, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _TryGet(key, out var item)
                ? item
                : defaultValue;
        }

        /// <summary>
        /// 缓存中是否存在指定的键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cacheMap.ContainsKey(key);
        }

        /// <summary>
        /// 尝试获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool TryGet(string key, out T result, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }


            if (_TryGet(key, out var item))
            {
                result = item;
                return true;
            }

            result = defaultValue;
            return false;
        }

        /// <summary>
        /// 设置缓存, 自动更新过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="seconds"></param>
        public static void Set(string key, T data, double seconds)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _cacheMap[key] = new CachedItem
            {
                ExpireDateTime = DateTime.Now.AddMilliseconds(seconds * 1000),
                Data = data
            };
        }

        /// <summary>
        /// 设置缓存, 但不会设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public static bool Set(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_cacheMap.ContainsKey(key))
            {
                lock (key)
                {
                    if (_cacheMap.ContainsKey(key))
                    {
                        _cacheMap[key].Data = data;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 更新缓存时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool SetExpire(string key, double seconds = 1)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (_cacheMap.TryGetValue(key, out var item))
            {
                item.ExpireDateTime = DateTime.Now.AddSeconds(seconds);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        public static bool TryRemove(string key, out T result, T defaultValue = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_cacheMap.TryRemove(key, out var item))
            {
                result = item.Data;
                return true;
            }

            result = defaultValue;
            return false;
        }

        /// <summary>
        /// 清理全部缓存
        /// </summary>
        public static void Clear()
        {
            _cacheMap.Clear();
        }

        /// <summary>
        /// 根据规则清理缓存
        /// </summary>
        /// <param name="exp">支持字符串或者正则</param>
        public static void Clear(string exp)
        {
            foreach (var kv in _cacheMap)
            {
                if (kv.Key.ContainsIgnoreCase(exp) || Regex.IsMatch(kv.Key, exp))
                {
                    TryRemove(kv.Key, out _);
                }
            }
        }

        /// <summary>
        /// 清理过期缓存
        /// </summary>
        private static void CleanExpiredCache()
        {
            foreach (var kv in _cacheMap)
            {
                if (kv.Value.ExpireDateTime <= DateTime.Now)
                {
                    TryRemove(kv.Key, out _);
                }
            }
        }

        /// <summary>
        /// 获取缓存,如果不存在则执行委托获取数据并缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T GetOrSet(string key,
            Func<T> valueFactory,
            double seconds = 1d)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!_TryGet(key, out var item))
            {
                if (valueFactory == null)
                    throw new ArgumentNullException(nameof(valueFactory));

                lock (key)
                {
                    if (!_TryGet(key, out item))
                    {
                        var data = valueFactory();
                        Set(key, data, seconds);
                        return data;
                    }
                    else
                    {
                        return item;
                    }
                }
            }
            else
            {
                return item;
            }
        }

        /// <summary>
        /// 获取缓存,如果不存在则执行委托获取数据并缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<T> GetOrSet(string key, Func<Task<T>> valueFactory, double seconds = 1d)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!_TryGet(key, out var item))
            {
                if (valueFactory == null)
                {
                    throw new ArgumentNullException(nameof(valueFactory));
                }

                // 等待异步操作完成并获取数据
                var data = await valueFactory();

                lock (key)
                {
                    if (!_TryGet(key, out item))
                    {
                        Set(key, data, seconds);
                    }
                }

                return data;
            }

            return item;
        }

        /// <summary>
        /// 会判断缓存是否有效
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool _TryGet(string key, out T data)
        {
            data = default;
            if (_cacheMap.TryGetValue(key, out var item) && item.ExpireDateTime > DateTime.Now)
            {
                data = item.Data;
                return true;
            }

            return false;
        }
    }
}