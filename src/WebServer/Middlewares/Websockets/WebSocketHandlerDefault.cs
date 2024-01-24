using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Pingfan.Kit.WebServer.Interfaces;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Pingfan.Kit.WebServer.Middlewares.Websockets;

/// <summary>
/// WebSocket处理事件
/// </summary>
public class WebSocketHandlerDefault
{
    /// <summary>
    /// 在线列表
    /// </summary>
    public ThreadSafeList<WebSocketDefault> Online { get; } = new();

    /// <summary>
    /// 检查协议是否合法
    /// </summary>
    public virtual bool OnCheck(string? protocol)
    {
        return false;
    }

    /// <summary>
    /// 连接已经打开
    /// </summary>
    public virtual void OnOpened(WebSocketDefault context)
    {
        this.Online.Add(context);
    }

    /// <summary>
    /// 收到数据
    /// </summary>
    public virtual void OnMessage(WebSocketDefault context, string message)
    {
    }

    /// <summary>
    /// 收到数据
    /// </summary>
    public virtual void OnBinary(WebSocketDefault context, byte[] data)
    {
    }

    /// <summary>
    /// 连接关闭
    /// </summary>
    public virtual void OnClosed(WebSocketDefault context)
    {
        this.Online.Remove(context);
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    public void Send(WebSocketDefault context, string message)
    {
        context.Send(message);
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    public void Send(WebSocketDefault context, object data)
    {
        context.Send(data);
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    public void Send(WebSocketDefault context, byte[] data)
    {
        context.Send(data);
    }
}