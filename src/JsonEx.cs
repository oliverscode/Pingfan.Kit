#if NETSTANDARD3_1 || NET6

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
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
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
        public static T FromString<T>(string json, T defaultValue = default!)
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
        public static T FromJsonString<T>(this string json, T defaultValue = default!)
        {
            return Json.FromString<T>(json, defaultValue);
        }
    }

    /// <summary>
    /// JsonElement扩展
    /// </summary>
    public static class JsonElementEx
    {
#if NET6
        /// <summary>
        /// 获取JsonElement的值
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>(this JsonElement jObject, string key, T defaultValue = default(T))
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
#endif