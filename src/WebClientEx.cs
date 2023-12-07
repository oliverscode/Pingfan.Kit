using System;
using System.Net;


namespace Pingfan.Kit
{
    /// <summary>
    /// 支持设置超时时间
    /// </summary>
    public class WebClientEx : WebClient
    {
        /// <summary>
        /// 超时时间, 单位毫秒, 默认30秒
        /// </summary>
        public int Timeout { get; set; }

        /// <inheritdoc />
        public WebClientEx(int timeout = 30 * 1000)
        {
            Timeout = timeout;
        }

        /// <inheritdoc />
        protected override WebRequest GetWebRequest(Uri uri)
        {
            var wr = base.GetWebRequest(uri);
            wr.Timeout = this.Timeout;
            return wr;
        }
    }
}