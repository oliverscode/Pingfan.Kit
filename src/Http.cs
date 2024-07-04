namespace Pingfan.Kit
{
#if NET6_0_OR_GREATER

    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Http快速访问类
    /// </summary>
    public sealed class Http
    {
        /// <summary>
        /// Http返回结果
        /// </summary>
        public class HttpResult
        {
            /// <summary>
            /// 状态码
            /// </summary>
            public HttpStatusCode StatusCode { get; set; }

            /// <summary>
            /// 返回的内容
            /// </summary>
            public string? Text { get; set; }

            /// <summary>
            /// 返回的字节
            /// </summary>
            public byte[]? Bytes { get; set; }

            /// <summary>
            /// 返回的头
            /// </summary>
            public WebHeaderCollection Headers { get; set; } = new();
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<HttpResult> Get(string url, Dictionary<string, string>? headers = null)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            // 获取Bytes
            var bytes = await response.Content.ReadAsByteArrayAsync();
            var text = Encoding.UTF8.GetString(bytes);

            var result = new HttpResult
            {
                StatusCode = response.StatusCode,
                Text = text,
                Bytes = bytes,
            };
            foreach (var httpResponseHeader in response.Headers)
            {
                var key = httpResponseHeader.Key;
                // 有时候会有多个值
                var value = string.Join(",", httpResponseHeader.Value);
                result.Headers.Add(key, value);
            }

            return result;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<HttpResult> Post(
            string url,
            string postData,
            Dictionary<string, string>? headers = null
        )
        {
            // application/x-www-form-urlencoded
            var client = new HttpClient();
            var content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await client.PostAsync(url, content);

            // 获取Bytes
            var bytes = await response.Content.ReadAsByteArrayAsync();
            var text = Encoding.UTF8.GetString(bytes);
            var result = new HttpResult
            {
                StatusCode = response.StatusCode,
                Text = text,
                Bytes = bytes,
            };
            foreach (var httpResponseHeader in response.Headers)
            {
                var key = httpResponseHeader.Key;
                // 有时候会有多个值
                var value = string.Join(",", httpResponseHeader.Value);
                result.Headers.Add(key, value);
            }

            return result;
        }


        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonData"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<HttpResult> Json
        (
            string url,
            object jsonData,
            Dictionary<string, string>? headers = null
        )
        {
            // application/json
            var client = new HttpClient();
            var content = new StringContent(jsonData.ToJsonString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            // 获取Bytes
            var bytes = await response.Content.ReadAsByteArrayAsync();
            var text = Encoding.UTF8.GetString(bytes);
            var result = new HttpResult
            {
                StatusCode = response.StatusCode,
                Text = text,
                Bytes = bytes,
            };
            foreach (var httpResponseHeader in response.Headers)
            {
                var key = httpResponseHeader.Key;
                // 有时候会有多个值
                var value = string.Join(",", httpResponseHeader.Value);
                result.Headers.Add(key, value);
            }

            return result;
        }
    }

#endif
}