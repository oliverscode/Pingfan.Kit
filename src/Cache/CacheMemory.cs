using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pingfan.Kit.Cache
{
    /// <summary>
    /// 内存缓存类
    /// </summary>
    public class CacheMemory : ICache
    {
        private readonly string _projectName;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheMemory(string projectName)
        {
            _projectName = projectName;
        }

        /// <inheritdoc />
        public bool Has(string key)
        {
            key = GetKey(_projectName, key);
            if (Read(key, out var expire, out _) == false)
                return false;
            return !IsExpired(expire);
        }


        /// <inheritdoc />
        public T Get<T>(string key)
        {
            TryGet<T>(key, out var item);
            return item;
        }

        /// <inheritdoc />
        public void Set<T>(string key, T data, float seconds)
        {
            key = GetKey(_projectName, key);
            var expire = DateTime.UtcNow.AddSeconds(seconds);
            Write(key, true, true, expire, data);
        }


        /// <inheritdoc />
        public bool TryGet<T>(string key, out T result)
        {
            result = default!;
            key = GetKey(_projectName, key);
            var t = Read(key, out _, out var data);
            result = (T)(object)data!;
            return t;
        }

        /// <inheritdoc />
        public T GetOrSet<T>(string key, Func<T> valueFactory, float seconds = 1)
        {
            if (TryGet<T>(key, out var result))
                return result;
            var data = valueFactory();
            Set(key, data, seconds);
            return data;
        }


        /// <inheritdoc />
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, float seconds = 1)
        {
            if (TryGet<T>(key, out var result))
                return result;
            var data = await valueFactory();
            Set(key, data, seconds);
            return data;
        }


        /// <inheritdoc />
        public bool SetExpire(string key, float seconds = 1)
        {
            key = GetKey(_projectName, key);
            var expire = DateTime.UtcNow.AddSeconds(seconds);
            return Write(key, true, false, expire, null);
        }


        /// <inheritdoc />
        public bool Remove(string key)
        {
            key = GetKey(_projectName, key);
            return Delete(key);
        }


        /// <inheritdoc />
        public void Clear()
        {
            var key = GetKey(_projectName, null);
            ClearKey(key);
        }


        #region 帮助方法

        static CacheMemory()
        {
            Ticker.LoopWithTry(30 * 1000, CleanExpiredCache);
        }

        private static void CleanExpiredCache()
        {
            foreach (var kv in CacheMap)
            {
                try
                {
                    ClearMemory(kv.Key, kv.Value);
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static void ClearMemory(string key, CachedItem item)
        {
            if (IsExpired(item.ExpireDateTime))
            {
                Delete(key);
            }
        }


        /// <summary>
        /// 判断时间戳是否过期, True已过期, False未过期
        /// </summary>
        /// <returns></returns>
        private static bool IsExpired(DateTime timestamp)
        {
            return timestamp <= DateTime.UtcNow;
        }


        private class CachedItem
        {
            public DateTime ExpireDateTime;
            public object? Data;
        }

        private static string GetKey(string name, string? key)
        {
            return $"{name}_{key}";
        }

        private static readonly object SourceLocker = new();
        private static readonly Dictionary<string, object> Locks = new();

        private static readonly ConcurrentDictionary<string, CachedItem> CacheMap = new();

        private static bool Read(string key,
            out DateTime expire,
            out object? data)
        {
            expire = default;
            data = default;

            object lockObj;
            lock (SourceLocker)
            {
                if (!Locks.TryGetValue(key, out lockObj))
                {
                    lockObj = new object();
                    Locks[key] = lockObj;
                }
            }

            try
            {
                lock (lockObj)
                {
                    if (!CacheMap.TryGetValue(key, out var value))
                        return false;
                    expire = value.ExpireDateTime;
                    data = value.Data;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool Write(string key,
            bool isTimeStamp,
            bool isContext,
            DateTime expire,
            object? data)
        {
            object lockObj;
            lock (SourceLocker)
            {
                if (!Locks.TryGetValue(key, out lockObj))
                {
                    lockObj = new object();
                    Locks[key] = lockObj;
                }
            }

            try
            {
                lock (lockObj)
                {
                    if (!CacheMap.ContainsKey(key))
                    {
                        CacheMap[key] = new CachedItem();
                    }

                    var sourceItem = CacheMap[key];
                    if (isTimeStamp)
                        sourceItem.ExpireDateTime = expire;
                    if (isContext)
                        sourceItem.Data = data;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool Delete(string key)
        {
            object lockObj;
            lock (SourceLocker)
            {
                if (!Locks.TryGetValue(key, out lockObj))
                {
                    lockObj = new object();
                    Locks[key] = lockObj;
                }
            }

            try
            {
                lock (lockObj)
                {
                    return CacheMap.TryRemove(key, out _);
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool ClearKey(string key)
        {
            object lockObj;
            lock (SourceLocker)
            {
                if (!Locks.TryGetValue(key, out lockObj))
                {
                    lockObj = new object();
                    Locks[key] = lockObj;
                }
            }

            try
            {
                lock (lockObj)
                {
                    var success = true;
                    foreach (var kv in CacheMap)
                    {
                        if (kv.Key.StartsWith(key))
                        {
                            if (CacheMap.TryRemove(kv.Key, out _) == false)
                            {
                                success = false;
                            }
                        }
                    }

                    return success;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}