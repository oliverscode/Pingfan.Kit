
#if NETSTANDARD3_1 || NET6

namespace Pingfan.Kit
{
    using System.Text.Unicode;
    using System.Text.Json;
    
    /// <summary>
    /// JSON序列化
    /// </summary>
    public class Json
    {
        internal static readonly JsonSerializerOptions _JsonSerializerOptions = new JsonSerializerOptions()
        {
            // 中文支持
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All),

            // 忽略空值
            IgnoreNullValues = true,

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
            return JsonSerializer.Serialize(obj, _JsonSerializerOptions);
        }
        
        /// <summary>
        /// 一段JSON字符串转换为对象
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FromString<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _JsonSerializerOptions);
        }
    }

    public static class JsonEx
    {
        /// <summary>
        /// 把对象转换为JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString(this object obj)
        {
            return Json.ToString(obj);
        }
        
        /// <summary>
        /// 把JSON字符串转换为对象
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FromJsonString<T>(this string json)
        {
            return Json.FromString<T>(json);
        }
    }
    
}
#endif