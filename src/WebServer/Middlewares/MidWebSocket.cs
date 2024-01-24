using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Pingfan.Kit.Inject;
using Pingfan.Kit.WebServer.Interfaces;
using Pingfan.Kit.WebServer.Middlewares.Websockets;

// ReSharper disable ClassNeverInstantiated.Global

namespace Pingfan.Kit.WebServer.Middlewares;

/// <summary>
/// WebSocket中间件
/// </summary>
public class MidWebSocket : IMiddleware
{
    private readonly Dictionary<string, WebSocketHandlerDefault> _handlers =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 默认的WebSocketContext类型
    /// </summary>
    public Type WebSocketContextType { get; set; } = typeof(WebSocketContextDefault);

    /// <summary>
    /// 默认UTF-8编码    
    /// </summary>
    public Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <inheritdoc />
    public void Invoke(IContainer container, IHttpContext ctx, Action next)
    {
        if (ctx.Request.HttpListenerContext.Request.IsWebSocketRequest == false)
        {
            next();
            return;
        }

        var path = ctx.Request.Path;

        if (_handlers.TryGetValue(path, out var handler) == false)
        {
            next();
            return;
        }

        var protocol = ctx.Request.Headers["Sec-WebSocket-Protocol"];
        var socketContext = ctx.Request.HttpListenerContext.AcceptWebSocketAsync(protocol).Result;
        container.Register(socketContext);
        container.Register(Encoding);
        var webSocketContext = (WebSocketContextDefault)container.New(WebSocketContextType);

        if (handler.OnCheck(protocol) == false)
        {
            return;
        }

        handler.OnOpened(webSocketContext);


        // 接收数据
        var buffer = new byte[1024 * 4];
        while (webSocketContext.IsAvailable)
        {
            var result = webSocketContext.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer),
                CancellationToken.None).Result;
            if (result.MessageType == WebSocketMessageType.Close)
            {
                break;
            }

            if (result.MessageType == WebSocketMessageType.Binary)
            {
                handler.OnBinary(webSocketContext, buffer);
            }
            else if (result.MessageType == WebSocketMessageType.Text)
            {
                var txt = Encoding.GetString(buffer, 0, result.Count);
                handler.OnMessage(webSocketContext, txt);
            }
        }

        handler.OnClosed(webSocketContext);
    }


    /// <summary>
    /// 添加一组处理器
    /// </summary>
    /// <param name="path"></param>
    /// <typeparam name="T"></typeparam>
    public void Add<T>(string path = "/") where T : WebSocketHandlerDefault
    {
        var type = typeof(T);
        Add(path, type);
    }

    /// <summary>
    /// 添加一组处理器
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    public void Add(string path, Type type)
    {
        // 创建对象
        var instance = (WebSocketHandlerDefault)Activator.CreateInstance(type)!;
        _handlers.Add(path, instance);
    }
}

/// <summary>
/// 扩展
/// </summary>
public static class MidWebSocketEx
{
    /// <summary>
    /// 使用WebSocket中间件
    /// </summary>
    public static WebServer UseWebSocket(this WebServer webServer, Action<MidWebSocket>? action = null)
    {
        var mid = webServer.Container.New<MidWebSocket>();
        action?.Invoke(mid);
        webServer.Use(mid);
        return webServer;
    }
}