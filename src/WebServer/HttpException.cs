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
    /// <summary>
    /// 参数类型
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// 参数名
    /// </summary>
    public string Name { get; }


    /// <inheritdoc />
    public HttpArgumentException(string message, Type type, string name) : base(message)
    {
        Type = type;
        Name = name;
    }

    /// <inheritdoc />
    public HttpArgumentException(string message, Type type, string name, Exception innerException) : base(message,
        innerException)
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