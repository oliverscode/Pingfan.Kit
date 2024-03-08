using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Pingfan.Kit;

/// <summary>
/// 测试网络是否通常
/// </summary>
public class Telnet
{
    /// <summary>
    /// 测试是否通常
    /// </summary>
    public static bool Test(string hostname, int port, int timeout = 1000)
    {
        try
        {
            using var client = new TcpClient();
            var result = client.BeginConnect(hostname, port, null, null);
            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeout));
            if (!success)
            {
                return false;
            }

            client.EndConnect(result);
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 测试是否通常
    /// </summary>
    public static async Task<bool> TestAsync(string hostname, int port, int timeout = 1000)
    {
        try
        {
            using var client = new TcpClient();
            var task = client.ConnectAsync(hostname, port);

            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                await task;
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }


    /// <summary>
    /// 判断一个ipv4地址是否是局域网地址
    /// 10.0.0.0 至 10.255.255.255, 172.16.0.0 至 172.31.255.255, 192.168.0.0 至 192.168.255.255
    /// </summary>
    public static bool IsLocalIpAddress(string ipAddress)
    {
        // 将字符串IP地址转换为IPAddress对象
        var ip = IPAddress.Parse(ipAddress);

        // 获取IP地址的字节表示
        var ipBytes = ip.GetAddressBytes();

        // 检查IP地址是否在10.0.0.0至10.255.255.255范围内
        if (ipBytes[0] == 10)
        {
            return true;
        }

        // 检查IP地址是否在172.16.0.0至172.31.255.255范围内
        if (ipBytes[0] == 172 && (ipBytes[1] >= 16 && ipBytes[1] <= 31))
        {
            return true;
        }

        // 检查IP地址是否在192.168.0.0至192.168.255.255范围内
        if (ipBytes[0] == 192 && ipBytes[1] == 168)
        {
            return true;
        }

        // 不在上述范围内，不是局域网IP
        return false;
    }


    /// <summary>
    /// 查找一个局域网内的端口可以用的ip, 用于局域网内的服务发现
    /// </summary>
    /// <param name="port">服务端口, 仅支持TCP</param>
    /// <param name="timeout">超时时间, 单位毫秒</param>
    /// <param name="testUrl">路由跟踪测试的URL, 默认qq.com</param>
    /// <returns></returns>
    /// <exception cref="Exception">如果没找到会异常</exception>
    public static async Task<IPEndPoint> FindEndPoint(int port, int timeout = 10, string testUrl = "qq.com")
    {
        // 1.路由跟踪一下qq.com, 看一下本地网段内都有哪些局域网Ip
        // 2.扫描一下本地网段内的局域网Ip,看一下有哪些开放的端口

        // var result = Cmd.RunWithOutput("tracert -d -w 10 -h 3 qq.com");
        var result = Cmd.RunWithOutput($"tracert -d -w {timeout} -h 3 {testUrl}");

        var ips = result.Matches(@"\s+?(<?\d+) ms.+?(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})");
        var localIps = ips.Where(p => p.Count == 2)
            .Select(p => p[1])
            .Where(IsLocalIpAddress)
            .Distinct()
            .ToList();
        

        IPEndPoint? answer = null;


        // 分别测试这些ip的指定端口是否开放
        foreach (var ip in localIps)
        {
            // 扫描这个ip网段内所有的ip
            var ipSegments = ip.Split('.');
            var ipSegment = $"{ipSegments[0]}.{ipSegments[1]}.{ipSegments[2]}";
            var scan = new List<string>(254);
            for (var i = 1; i < 255; i++)
            {
                var ipToTest = $"{ipSegment}.{i}";
                scan.Add(ipToTest);
            }

            await scan.Each(subIp =>
            {
                if (answer != null) // 已经找到了
                    return;
                if (Test(subIp, port, timeout))
                    answer = new IPEndPoint(IPAddress.Parse(subIp), port);
            });
        }

        if (answer == null)
            throw new Exception("没有找到可用的局域网Ip"); // 没有找到抛出异常
        return answer;
    }
}