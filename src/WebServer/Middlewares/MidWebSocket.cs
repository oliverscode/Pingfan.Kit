using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Pingfan.Kit.Inject;
using Pingfan.Kit.WebServer.Interfaces;
using Pingfan.Kit.WebServer.Middlewares.Websockets;

namespace Pingfan.Kit.WebServer.Middlewares;

/// <summary>
/// WebSocket中间件
/// </summary>
public class MidWebSocket : IMiddleware
{
    private readonly List<WebSocketItem> _webSockets = new List<WebSocketItem>();

    /// <summary>
    /// 默认UTF8编码    
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
        var item = _webSockets.FirstOrDefault(p => string.Equals(path, p.Path, StringComparison.OrdinalIgnoreCase));
        if (item == null)
        {
            next();
            return;
        }

        var protocol = ctx.Request.Headers["Sec-WebSocket-Protocol"];
        var listenerWebSocketContext = ctx.Request.HttpListenerContext.AcceptWebSocketAsync(protocol).Result;
        container.Push(listenerWebSocketContext);
        container.Push(Encoding);
        var webSocketContext = (IWebSocketContext)container.New(item.InstanceType);
        if (webSocketContext.OnCheck() == false)
        {
            return;
        }

        webSocketContext.OnOpen();


        // 接收数据
        var buffer = new byte[1024 * 4];
        while (webSocketContext.IsAvailable)
        {
            var result = webSocketContext.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer),
                CancellationToken.None).Result;
            if (result.MessageType == WebSocketMessageType.Close)
            {
                webSocketContext.OnClose();
                break;
            }

            if (result.MessageType == WebSocketMessageType.Binary)
            {
                webSocketContext.OnBinary(buffer);
            }
            else if (result.MessageType == WebSocketMessageType.Text)
            {
                var txt = Encoding.GetString(buffer, 0, result.Count);
                webSocketContext.OnMessage(txt);
            }
        }
    }


    /// <summary>
    /// 添加一组处理器
    /// </summary>
    /// <param name="path"></param>
    /// <typeparam name="T"></typeparam>
    public void Add<T>(string path = "/") where T : IWebSocketContext
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
        _webSockets.Add(new WebSocketItem
        {
            Path = path,
            InstanceType = type
        });
    }


    private class WebSocketItem
    {
        public string Path { get; set; } = null!;
        public Type InstanceType { get; set; } = null!;
    }
}

/// <summary>
/// 扩展
/// </summary>
public static class MidWebSocketEx
{
    /// <summary>
    /// 使用静态文件中间件
    /// </summary>
    public static WebServer UseWebSocket(this WebServer webServer, Action<MidWebSocket>? action = null)
    {
        var mid = webServer.Container.New<MidWebSocket>();
        action?.Invoke(mid);
        webServer.Use(mid);
        return webServer;
    }
}