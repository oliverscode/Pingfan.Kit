using System;
using System.Net;

namespace Pingfan.Kit
{
    /// <summary>
    /// 支持设置超时时间
    /// </summary>
    public class WebClientEx : WebClient
    {
        public int Timeout { get; set; }

        public WebClientEx(int timeout = 30 * 1000)
        {
            Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var wr = base.GetWebRequest(uri);
            wr.Timeout = this.Timeout;
            return wr;
        }
    }
}