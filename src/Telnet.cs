using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
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
}