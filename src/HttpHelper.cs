using System.Net;
using System.Text;


namespace Pingfan.Kit
{
    /// <summary>
    /// Http快速访问类
    /// </summary>
    public sealed class Http
    {

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            var httpclient = new WebClient();
            return httpclient.DownloadString(url);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string Post(string url, string postData)
        {
            var webClient = new WebClient();
            var data = Encoding.UTF8.GetBytes(postData);
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var result = webClient.UploadData(url, "POST", data);
            return Encoding.UTF8.GetString(result);
        }
    }
}