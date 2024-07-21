using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Pingfan.Kit.Cache;

/// <summary>
/// 文件缓存类
/// 存储文件结构为
/// 开头14个字符为毫秒级时间戳  DateTime.Now.ToUnixTimeMilliseconds()
/// 然后才是真正的原始数据
/// </summary>
public class CacheFile : ICache
{
    private const string CacheFileDir = "runtime_file_cache";
    private readonly string _projectName;

    public CacheFile(string projectName)
    {
        _projectName = projectName;
    }

    /// <inheritdoc />
    public bool Has(string key)
    {
        var path = GetKeyPath(_projectName, key);
        if (Read(path, true, false, out var timestamp, out _) == false)
            return false;
        return !IsExpired(timestamp);
    }

    /// <inheritdoc />
    public T? Get<T>(string key) where T : class
    {
        TryGet<T>(key, out var result);
        return result;
    }

    /// <inheritdoc />
    public void Set<T>(string key, T? data, float seconds = 1)
    {
        var path = GetKeyPath(_projectName, key);
        var timestamp = DateTime.Now.ToUnixTimeMilliseconds() + (long)(seconds * 1000);

        byte[] result;

        // 对类型单独处理
        var type = typeof(T);
        if (type == typeof(int) || type == typeof(int?))
            result = BitConverter.GetBytes((int)(object)data!);
        else if (type == typeof(short) || type == typeof(short?))
            result = BitConverter.GetBytes((short)(object)data!);
        else if (type == typeof(ushort) || type == typeof(ushort?))
            result = BitConverter.GetBytes((ushort)(object)data!);
        else if (type == typeof(long) || type == typeof(long?))
            result = BitConverter.GetBytes((long)(object)data!);
        else if (type == typeof(ulong) || type == typeof(ulong?))
            result = BitConverter.GetBytes((ulong)(object)data!);
        else if (type == typeof(byte) || type == typeof(byte?))
            result = BitConverter.GetBytes((char)(object)data!);
        else if (type == typeof(sbyte) || type == typeof(sbyte?))
            result = BitConverter.GetBytes((char)(object)data!);
        else if (type == typeof(char) || type == typeof(char?))
            result = BitConverter.GetBytes((char)(object)data!);
        else if (type == typeof(byte[]))
            result = (data as byte[])!;
        else if (type == typeof(float) || type == typeof(float?))
            result = BitConverter.GetBytes((float)(object)data!);
        else if (type == typeof(double) || type == typeof(double?))
            result = BitConverter.GetBytes((double)(object)data!);
        else if (type == typeof(bool) || type == typeof(bool?))
            result = BitConverter.GetBytes((bool)(object)data!);
        else if (type == typeof(string))
            result = Encoding.UTF8.GetBytes((string)(object)data!);
        else if (type == typeof(DateTime) || type == typeof(DateTime?))
            result = BitConverter.GetBytes(((DateTime)(object)data!).ToBinary());
        else if (type == typeof(Guid) || type == typeof(Guid?))
            result = ((Guid)(object)data!).ToByteArray();
        else if (type == typeof(TimeSpan) || type == typeof(TimeSpan?))
            result = BitConverter.GetBytes(((TimeSpan)(object)data!).Ticks);
        else if (type == typeof(decimal) || type == typeof(decimal?))
        {
            decimal num = (decimal)(object)data!;
            int[] bits = decimal.GetBits(num);
            byte[] bytes = new byte[bits.Length * 4];
            Buffer.BlockCopy(bits, 0, bytes, 0, bytes.Length);
            result = bytes;
        }
        else
        {
            result = Encoding.UTF8.GetBytes(Json.ToString(data!));
        }

        Write(path, true, true, timestamp, result);
    }

    /// <inheritdoc />
    public bool TryGet<T>(string key, out T? result) where T : class
    {
        result = null;
        var path = GetKeyPath(_projectName, key);
        if (Read(path, true, true, out var timestamp, out var context) == false)
            return false;
        if (IsExpired(timestamp))
            return false;

        // 对类型单独处理
        var type = typeof(T);
        if (type == typeof(int?))
            result = BitConverter.ToInt32(context!, 0) as T;
        else if (type == typeof(uint?))
            result = BitConverter.ToUInt32(context!, 0) as T;

        else if (type == typeof(short?))
            result = BitConverter.ToInt16(context!, 0) as T;
        else if (type == typeof(ushort?))
            result = BitConverter.ToUInt16(context!, 0) as T;


        else if (type == typeof(long?))
            result = BitConverter.ToInt64(context!, 0) as T;
        else if (type == typeof(ulong?))
            result = BitConverter.ToUInt64(context!, 0) as T;


        else if (type == typeof(byte?))
            result = context![0] as T;
        else if (type == typeof(sbyte?))
            result = context![0] as T;

        else if (type == typeof(char?))
            result = context![0] as T;

        else if (type == typeof(byte[]))
            result = context as T;

        else if (type == typeof(float?))
            result = BitConverter.ToSingle(context!, 0) as T;
        else if (type == typeof(double?))
            result = BitConverter.ToDouble(context!, 0) as T;

        else if (type == typeof(bool?))
            result = BitConverter.ToBoolean(context!, 0) as T;


        else if (type == typeof(string))
            result = Encoding.UTF8.GetString(context!) as T;
        else if (type == typeof(DateTime?))
            result = DateTime.FromBinary(BitConverter.ToInt64(context!, 0)) as T;
        else if (type == typeof(Guid?))
            result = new Guid(context!) as T;
        else if (type == typeof(TimeSpan?))
            result = TimeSpan.FromTicks(BitConverter.ToInt64(context!, 0)) as T;

        else if (type == typeof(decimal?))
        {
            var bits = new int[context!.Length / 4];
            Buffer.BlockCopy(context, 0, bits, 0, context.Length);
            result = new decimal(bits) as T;
        }

        else
        {
            result = Json.FromString<T>(Encoding.UTF8.GetString(context!));
        }

        return true;
    }

    /// <inheritdoc />
    public T? GetOrSet<T>(string key, Func<T?> valueFactory, float seconds = 1) where T : class
    {
        if (TryGet<T>(key, out var result))
            return result;

        var data = valueFactory();
        Set(key, data, seconds);
        return data;
    }

    /// <inheritdoc />
    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> valueFactory, float seconds = 1)
        where T : class
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
        var path = GetKeyPath(_projectName, key);
        return Write(path, true, false, DateTime.Now.ToUnixTimeMilliseconds() + (long)(seconds * 1000), null);
    }

    /// <inheritdoc />
    public bool Remove(string key)
    {
        var path = GetKeyPath(_projectName, key);
        return Delete(path);
    }

    /// <inheritdoc />
    public void Clear()
    {
        var path = GetKeyPath(_projectName, null);
        ClearKey(path);
    }


    #region 帮助方法

    static CacheFile()
    {
        Ticker.LoopWithTry(30 * 1000, CleanExpiredCache);
    }

    private static string GetKeyPath(string name, string? key)
    {
        name = HashEx.Crc32(name);
        if (key != null)
            key = HashEx.Crc32(key);
        var path = PathEx.CombineCurrentDirectory(CacheFileDir, $"{name}_{key}");
        return path;
    }

    private static void CleanExpiredCache()
    {
        var dir = PathEx.CombineCurrentDirectory(CacheFileDir);
        var files = DirectoryEx.GetFiles(dir);
        foreach (var file in files)
        {
            try
            {
                ClearFile(file);
            }
            catch
            {
                // ignored
            }
        }
    }

    private static void ClearFile(string path)
    {
        Read(path, true, false, out var timestamp, out _);
        if (IsExpired(timestamp))
        {
            Delete(path);
        }
    }

    /// <summary>
    /// 判断时间戳是否过期, True已过期, False未过期
    /// </summary>
    /// <returns></returns>
    private static bool IsExpired(long timestamp)
    {
        return timestamp <= DateTime.Now.ToUnixTimeMilliseconds();
    }


    private static readonly object SourceLocker = new();
    private static readonly Dictionary<string, object> Locks = new();


    /// <summary>
    /// 读取文件
    /// </summary>
    private static bool Read(string path,
        bool isTimeStamp,
        bool isContext,
        out long timestamp,
        out byte[]? context)
    {
        timestamp = 0;
        context = null;


        object lockObj;
        lock (SourceLocker)
        {
            if (!Locks.TryGetValue(path, out lockObj))
            {
                lockObj = new object();
                Locks[path] = lockObj;
            }
        }

        try
        {
            lock (lockObj)
            {
                if (File.Exists(path) == false)
                    return false;

                using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                if (isTimeStamp)
                {
                    var buffer = new byte[14];
                    var read = fileStream.Read(buffer, 0, 14);
                    if (read != 14)
                        return false;
                    timestamp = BitConverter.ToInt64(buffer, 0);
                }

                if (isContext)
                {
                    var buffer = new byte[fileStream.Length - 14];
                    fileStream.Seek(14, SeekOrigin.Begin);
                    var read = fileStream.Read(buffer, 0, buffer.Length);
                    if (read != buffer.Length)
                        return false;
                    context = buffer;
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 写入文件
    /// </summary>
    private static bool Write(string path,
        bool isTimeStamp,
        bool isContext,
        long timestamp,
        byte[]? context)
    {
        object lockObj;
        lock (SourceLocker)
        {
            if (!Locks.TryGetValue(path, out lockObj))
            {
                lockObj = new object();
                Locks[path] = lockObj;
            }
        }

        try
        {
            lock (lockObj)
            {
                using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                if (isTimeStamp)
                {
                    var buffer = BitConverter.GetBytes(timestamp);
                    fileStream.Write(buffer, 0, buffer.Length);
                }

                if (isContext && context != null)
                {
                    fileStream.Write(context, 0, context.Length);
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }


    /// <summary>
    /// 删除文件
    /// </summary>
    private static bool Delete(string path)
    {
        object lockObj;
        lock (SourceLocker)
        {
            if (!Locks.TryGetValue(path, out lockObj))
            {
                lockObj = new object();
                Locks[path] = lockObj;
            }
        }

        try
        {
            lock (lockObj)
            {
                File.Delete(path);
            }
        }
        catch
        {
            //    
        }

        return File.Exists(path);
    }

    /// <summary>
    /// 清空
    /// </summary>
    private static bool ClearKey(string path)
    {
        object lockObj;
        lock (SourceLocker)
        {
            if (!Locks.TryGetValue(path, out lockObj))
            {
                lockObj = new object();
                Locks[path] = lockObj;
            }
        }

        try
        {
            lock (lockObj)
            {
                var success = true;
                var files = DirectoryEx.GetFiles(CacheFileDir);
                foreach (var file in files)
                {
                    if (file.StartsWith(path))
                    {
                        if (Delete(file) == false)
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