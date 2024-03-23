using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using Pingfan.Kit.WebServer.Interfaces;

namespace Pingfan.Kit.WebServer.Middlewares.Websockets;

/// <summary>
/// WebSocket上下文默认实现
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class WebSocketContextDefault : IDisposable
{
    private readonly IHttpResponse _httpResponse;

    /// <summary>
    /// 自定义数据
    /// </summary>
    public Dictionary<string, object> Items { get; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 默认编码
    /// </summary>
    public Encoding Encoding { get; set; }

    /// <summary>
    /// 协议
    /// </summary>
    public string? Protocol => this.HttpListenerWebSocketContext.WebSocket.SubProtocol;

    /// <summary>
    /// HttpListenerWebSocketContext实例
    /// </summary>
    public HttpListenerWebSocketContext HttpListenerWebSocketContext { get; set; }

    /// <summary>
    /// WebSocket的Id
    /// </summary>
    public int Id => this.HttpListenerWebSocketContext.WebSocket.GetHashCode();

    /// <summary>
    /// WebSocket的实例
    /// </summary>
    public WebSocket WebSocket => this.HttpListenerWebSocketContext.WebSocket;

    /// <summary>
    /// 是否已经连接上
    /// </summary>
    public bool IsAvailable => this.HttpListenerWebSocketContext.WebSocket.State == WebSocketState.Open;

    /// <summary>
    /// 构造函数
    /// </summary>
    public WebSocketContextDefault(
        HttpListenerWebSocketContext httpListenerWebSocketContext,
        IHttpResponse httpResponse,
        Encoding encoding)
    {
        HttpListenerWebSocketContext = httpListenerWebSocketContext;
        _httpResponse = httpResponse;
        Encoding = encoding;
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    public void Send(object json)
    {
        if (this.IsAvailable == false) return;
        var txt = JsonSerializer.Serialize(json, _httpResponse.JsonSerializerOptions);
        Send(txt);
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    public void Send(string message)
    {
        if (this.IsAvailable == false) return;
        var data = Encoding.GetBytes(message);
        this.HttpListenerWebSocketContext.WebSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text,
            true, CancellationToken.None);
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    public void Send(byte[] data)
    {
        if (this.IsAvailable == false) return;
        this.HttpListenerWebSocketContext.WebSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary,
            true, CancellationToken.None);
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    public void Close()
    {
        if (this.IsAvailable == false) return;
        this.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Close();
    }
}