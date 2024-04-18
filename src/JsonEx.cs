using System;

#pragma warning disable CS0612 // Type or member is obsolete

namespace Pingfan.Kit
{
    using System.Text.Unicode;
    using System.Text.Json;

    /// <summary>
    /// JSON序列化
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// JSON序列化配置
        /// </summary>
        [Obsolete] public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
        {
            // 中文支持
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All),

#if NET6
            // 忽略空值
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
#else
            // 忽略默认值
            IgnoreNullValues = true,
#endif


            // 全部大写参照上面注释代码
            // PropertyNamingPolicy = new UpperCaseNamingPolicy(),
        };

        /// <summary>
        /// 把对象转换为JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToString(object obj)
        {
            return JsonSerializer.Serialize(obj, JsonSerializerOptions);
        }

        /// <summary>
        /// 一段JSON字符串转换为对象
        /// </summary>
        public static T? FromString<T>(string json, T? defaultValue = default)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions)!;
            }
            catch
            {
                return defaultValue;
            }
        }
        
        /// <summary>
        /// 一段JSON字符串转换为对象
        /// </summary>
        public static JsonElement? FromString(string json)
        {
            try
            {
                return FromString<JsonElement>(json);
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// JSON扩展
    /// </summary>
    public static class JsonEx
    {
        /// <summary>
        /// 把对象转换为JSON字符串
        /// </summary>
        public static string ToJsonString(this object obj)
        {
            return Json.ToString(obj);
        }

        /// <summary>
        /// 把JSON字符串转换为对象
        /// </summary>
        public static T? FromString<T>(this string json, T? defaultValue = default)
        {
            return Json.FromString<T>(json, defaultValue);
        }

        /// <summary>
        /// 改变Json类型
        /// </summary>
        public static T? Change<T>(this JsonElement json, T? defaultValue = default)
        {
            try
            {
                return json.Deserialize<T>();
            }
            catch (Exception e)
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// JsonElement扩展
    /// </summary>
    public static class JsonElementEx
    {
#if NETCOREAPP
        /// <summary>
        /// 获取JsonElement的值
        /// </summary>
        public static T? Get<T>(this JsonElement jObject, string key, T? defaultValue = default)
        {
            try
            {
                if (jObject.TryGetProperty(key, out var value))
                {
                    return value.Deserialize<T>()!;
                }
            }
            catch
            {
                // ignored
            }

            return defaultValue;
        }

#endif
    }
}