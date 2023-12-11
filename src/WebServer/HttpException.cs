using System;

namespace Pingfan.Kit.WebServer;

/// <summary>
/// 立即结束对http的执行
/// </summary>
public class HttpEndException : Exception
{
}

/// <summary>
/// 参数错误异常
/// </summary>
public class HttpArgumentException : Exception
{
    public Type Type { get; }
    public string Name { get; }

    public HttpArgumentException(string message, Type type, string name) : base(message)
    {
        Type = type;
        Name = name;
    }
}

/// <summary>
/// 关闭WebSocket
/// </summary>
public class WebSocketEndException : Exception
{
}