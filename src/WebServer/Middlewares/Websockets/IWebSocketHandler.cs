using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Pingfan.Kit.WebServer.Interfaces;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Pingfan.Kit.WebServer.Middlewares.Websockets;

/// <summary>
/// WebSocket处理事件
/// </summary>
public interface IWebSocketHandler
{
    /// <summary>
    /// 检查协议是否合法
    /// </summary>
    bool OnCheck(string? protocol);

    /// <summary>
    /// 连接已经打开
    /// </summary>
    void OnOpened(WebSocketContextDefault context);

    /// <summary>
    /// 收到数据
    /// </summary>
    void OnMessage(WebSocketContextDefault context, string message);

    /// <summary>
    /// 收到数据
    /// </summary>
    void OnBinary(WebSocketContextDefault context, byte[] data);

    /// <summary>
    /// 连接关闭
    /// </summary>
    void OnClosed(WebSocketContextDefault context);
    
}