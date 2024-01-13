using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Pingfan.Kit;

/// <summary>
/// 端口辅助类
/// </summary>
public class PortEx
{
 
    /// <summary>
    /// 获取所有被占用的端口
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<int> GetUsedPorts()
    {
        return IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Select(x => x.Port);
    }
    
    
    /// <summary>
    /// 检查端口是否被占用
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public static bool IsUsed(int port)
    {
        return GetUsedPorts().Contains(port); 
    }
    
 
}