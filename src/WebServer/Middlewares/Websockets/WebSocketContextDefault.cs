using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using Pingfan.Kit.WebServer.Interfaces;

namespace Pingfan.Kit.WebServer.Middlewares.Websockets;

/// <summary>
/// WebSocket上下文默认实现
/// </summary>
public class WebSocketContextDefault : IWebSocketContext
{
    private readonly IHttpResponse _httpResponse;
    private WebSocket? _webSocket;

    /// <inheritdoc />
    public Encoding Encoding { get; set; }

    /// <inheritdoc />
    public string Protocol => this.HttpListenerWebSocketContext.WebSocket.SubProtocol!;

    /// <summary>
    /// HttpListenerWebSocketContext实例
    /// </summary>
    public readonly HttpListenerWebSocketContext HttpListenerWebSocketContext;

    /// <inheritdoc />
    public WebSocket WebSocket
    {
        get
        {
            if (_webSocket == null)
                _webSocket = this.HttpListenerWebSocketContext.WebSocket;
            return _webSocket;
        }
        set => _webSocket = value;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void Send(object json)
    {
        if (this.IsAvailable == false) return;
        var txt = JsonSerializer.Serialize(json, _httpResponse.JsonSerializerOptions);
        Send(txt);
    }

    /// <inheritdoc />
    public void Send(string message)
    {
        if (this.IsAvailable == false) return;
        var data = Encoding.GetBytes(message);
        this.HttpListenerWebSocketContext.WebSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text,
            true, CancellationToken.None);
    }

    /// <inheritdoc />
    public void Send(byte[] data)
    {
        if (this.IsAvailable == false) return;
        this.HttpListenerWebSocketContext.WebSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary,
            true, CancellationToken.None);
    }

    /// <inheritdoc />
    public void Close()
    {
        if (this.IsAvailable == false) return;
        this.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);
    }

    /// <inheritdoc />
    public virtual bool OnCheck(string protocol)
    {
        return false;
    }

    /// <inheritdoc />
    public virtual void OnOpen()
    {
    }

    /// <inheritdoc />
    public virtual void OnClose()
    {
    }

    /// <inheritdoc />
    public virtual void OnBinary(byte[] data)
    {
    }

    /// <inheritdoc />
    public virtual void OnMessage(string message)
    {
    }
}