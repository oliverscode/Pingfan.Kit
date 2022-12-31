using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Pingfan.Kit
{
    public class IPEx
    {
        /// <summary>
        /// 获取局域网IP
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetInterIP()
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
    }
}