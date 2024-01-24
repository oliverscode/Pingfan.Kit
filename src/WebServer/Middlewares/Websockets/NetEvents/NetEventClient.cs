using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit.WebServer.Middlewares.Websockets.NetEvents;

/// <summary>
/// 网络事件
/// </summary>
public class NetEventClient
{
    private ClientWebSocket _websocket = new ClientWebSocket();

    private Task Connect()
    {
        return _websocket.ConnectAsync(new Uri(this.Url), CancellationToken.None);
    }


    public string Url { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsAvailable => _websocket.State == WebSocketState.Open;

    public NetEventClient(string url)
    {
        this.Url = url;
        Timer.Loop(1500, () =>
        {
            // 判断是否已经关闭, 关闭则重连
            if (_websocket.State == WebSocketState.Closed)
            {
                Connect();
            }
        });
        Connect();
    }

    public void On(string actionName, Action action)
    {
    }

    public void Emit(string action, object data)
    {
        if (IsAvailable == false) return;

        var json = new NetEventProtocol()
            { Type = "Emit", Data = new NetEventEmitProtocol() { ActionName = action, Data = data } };
        
        var message = json.ToJsonString();
        _websocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)), WebSocketMessageType.Text, true,
            CancellationToken.None);
    }


    class NetEventProtocol
    {
        /// <summary>
        /// 协议类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
    }

    class NetEventEmitProtocol
    {
        /// <summary>
        /// 方法名
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public object Data { get; set; }
    }
}