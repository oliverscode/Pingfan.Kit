using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Pingfan.Kit
{
    /// <summary>
    /// Ip扩展方法
    /// </summary>
    public static class IpEx
    {
        /// <summary>
        /// 获取局域网IPv4地址
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetLocalIPv4()
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

        // /// <summary>
        // /// 获取自己外网Ip, 获取失败会返回null
        // /// </summary>
        // public static string? GetWanIp()
        // {
        //     var urls = new[]
        //     {
        //         "http://2024.ip138.com/",
        //         "http://ifconfig.me/ip",
        //         "http://ip-api.com/json/",
        //         "http://icanhazip.com",
        //     };
        //
        //     return urls.Select(Http.Get)
        //         .Select(html => html.Match(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")
        //             .FirstOrDefault())
        //         .FirstOrDefault(ip => !string.IsNullOrEmpty(ip));
        // }

        // /// <summary>
        // /// 通过http://ip-api.com/json/获取Ip的物理地址, 每分钟最大45次请求
        // /// </summary>
        // public static string? GetIpLocation(string ip, string lang = "zh-CN")
        // {
        //     var url = $"http://ip-api.com/json/{ip}?lang={lang}";
        //     var html = Http.Get(url).Trim();
        //
        //     // {"status":"success","country":"China","countryCode":"CN","region":"BJ","regionName":"Beijing","city":"Beijing","zip":"","lat":39.9289,"lon":116.3883,"timezone":"Asia/Shanghai","isp":"China Telecom Beijing","org":"Chinatelecom","as":"AS4808 China Unicom Beijing Province Network","query":"
        //     // 获取country,regionName,city 这样的格式:country regionName-city
        //     // 通过正则分别获取
        //     
        //     var country = html.Match(@"""country"":""(.+?)""").FirstOrDefault();
        //     var regionName = html.Match(@"""regionName"":""(.+?)""").FirstOrDefault();
        //     var city = html.Match(@"""city"":""(.+?)""").FirstOrDefault();
        //     if (!string.IsNullOrEmpty(country) && !string.IsNullOrEmpty(regionName) && !string.IsNullOrEmpty(city))
        //     {
        //         if (country == regionName)
        //             return $"{country}{city}";
        //         if (country == city)
        //             return $"{country}{regionName}";
        //         if (regionName == city)
        //             return $"{country}{regionName}";
        //         if (country == regionName && regionName == city)
        //             return $"{country}";
        //         return $"{country}{regionName}{city}";
        //     }
        //
        //     return null;
        // }
    }
}