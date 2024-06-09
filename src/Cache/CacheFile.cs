using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pingfan.Kit.Cache;

/// <summary>
/// 文件缓存类
/// </summary>
public class CacheFile : ICache
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public CacheFile()
    {
        Ticker.LoopWithTry(30 * 1000, AutoCleanExpiredCache);
        PathEx.CreateCurrentDirectoryIfNotExists("runtime_file_cache");
    }

    /// <inheritdoc />
    public T? Get<T>(string key, T? defaultValue = default)
    {
        TryGet(key, out var result, defaultValue);
        return result;
    }

    /// <inheritdoc />
    public bool TryGet<T>(string key, out T? result, T? defaultValue = default)
    {
        try
        {
            var filename = HashEx.Crc32(key);
            var path = PathEx.CombineCurrentDirectory("runtime_file_cache", filename);
            if (File.Exists(path) == false)
                result = defaultValue;
            // 判断是否过期
            if (File.GetLastWriteTime(path) < DateTime.UtcNow)
            {
                File.Delete(path);
                result = defaultValue;
                return false;
            }
            
            var buffer = File.ReadAllText(path);
            result = Json.FromString<T>(buffer);
            return true;
        }
        catch
        {
            result = defaultValue;
            return false;
        }
    }

    /// <inheritdoc />
    public T? GetOrSet<T>(string key, Func<T?> valueFactory, float seconds = 1)
    {
        if (TryGet<T>(key, out var result))
            return result;

        var data = valueFactory();
        Set(key, data, seconds);
        return data;
    }

    /// <inheritdoc />
    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> valueFactory, float seconds = 1)
    {
        if (TryGet<T>(key, out var result))
            return result;

        var data = await valueFactory();
        Set(key, data, seconds);
        return data;
    }

    /// <inheritdoc />
    public bool Has(string key)
    {
        var filename = HashEx.Crc32(key);
        var path = PathEx.CombineCurrentDirectory("runtime_file_cache", filename);
        return File.Exists(path);
    }

    /// <inheritdoc />
    public void Set<T>(string key, T? data, float seconds)
    {
        var filename = HashEx.Crc32(key);
        var path = PathEx.CombineCurrentDirectory("runtime_file_cache", filename);
        var buffer = Json.ToString(data);
        FileEx.WriteAllText(path, buffer);
        // 把文件修改时间设置为过期时间
        File.SetLastWriteTime(path, DateTime.UtcNow.AddSeconds(seconds));
    }

    /// <inheritdoc />
    public bool Set<T>(string key, T? data)
    {
        Set(key, data, 1);
        return true;
    }

    /// <inheritdoc />
    public bool SetExpire(string key, float seconds = 1)
    {
        var filename = HashEx.Crc32(key);
        var path = PathEx.CombineCurrentDirectory("runtime_file_cache", filename);
        if (File.Exists(path) == false)
            return false;
        File.SetLastWriteTime(path, DateTime.UtcNow.AddSeconds(seconds));
        return true;
    }

    /// <inheritdoc />
    public bool TryRemove<T>(string key, out T? result, T? defaultValue = default)
    {
        if (TryGet(key, out result, defaultValue))
        {
            var filename = HashEx.Crc32(key);
            var path = PathEx.CombineCurrentDirectory("runtime_file_cache", filename);
            
            
            File.Delete(path);
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public void Clear()
    {
        var path = PathEx.CombineCurrentDirectory("runtime_file_cache");
        Directory.Delete(path, true);
    }

    /// <inheritdoc />
    public void Clear(string pattern)
    {
        var path = PathEx.CombineCurrentDirectory("runtime_file_cache");
        var files = DirectoryEx.GetFiles(path);
        foreach (var file in files)
        {
            // 正则匹配
            if (Regex.IsMatch(file, pattern))
            {
                File.Delete(file);
            }
        }
    }

    private void AutoCleanExpiredCache()
    {
        var path = PathEx.CombineCurrentDirectory("runtime_file_cache");
        var files = DirectoryEx.GetFiles(path);
        foreach (var file in files)
        {
            if (File.GetLastWriteTime(file) < DateTime.UtcNow)
            {
                File.Delete(file);
            }
        }
    }

}