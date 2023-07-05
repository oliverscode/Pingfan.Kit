using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Pingfan.Kit
{
    public static class IpEx
    {
        /// <summary>
        /// 获取局域网IPv4地址
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetInterIp()
        {
            //得到本机名 
            var hostname = Dns.GetHostName();
            var localhost = Dns.GetHostEntry(hostname);
            foreach (var item in localhost.AddressList)
            {
                //判断是否是内网IPv4地址
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    yield return item.MapToIPv4().ToString();
                }
            }
        }

        /// <summary>
        /// 获取自己外网Ip, 获取失败会返回空字符串
        /// </summary>
        /// <returns></returns>
        public static string GetPublicIp()
        {
            var url = "http://ifconfig.me/ip";
            // const string url = "http://ip-api.com/json/?lang=zh-CN";
            // https://icanhazip.com

            string externalip;
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;
                string all = wc.DownloadString(url);
                return all;
            }
            catch
            {
                externalip = "";
            }
            return externalip;
        }
    }
}