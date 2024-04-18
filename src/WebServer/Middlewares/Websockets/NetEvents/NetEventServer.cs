using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Pingfan.Kit.WebServer.Middlewares.Websockets.NetEvents;

/// <summary>
/// 网络事件
/// </summary>
public class NetEventServer : IWebSocketHandler
{
    private readonly ThreadSafeDictionary<WebSocketContextDefault, List<string>> _list = new();

    /// <inheritdoc />
    public void OnMessage(WebSocketContextDefault context, string message)
    {
        if (_list.TryGetValue(context, out var actions) == false)
        {
            return;
        }

        var data = message.FromString<NetEventProtocol>();
        if (data == null)
        {
            return;
        }

        if (data.Type == "UpdateList")
        {
            _list[context] = data.Data.Change<List<string>>()!;
        }
        else if (data.Type == "Emit")
        {
            var emitData = data.Data.Change<NetEventEmitProtocol>();
            if (emitData == null)
                return;

            foreach (var user in _list)
            {
                if (user.Value.ContainsIgnoreCase(emitData.ActionName))
                {
                    user.Key.Send(data);
                }
            }
        }
    }

    public void OnBinary(WebSocketContextDefault context, byte[] data)
    {
    }


    public bool OnCheck(string? protocol)
    {
        return true;
    }

    /// <inheritdoc />
    public void OnOpened(WebSocketContextDefault context)
    {
        if (_list.TryAdd(context, new List<string>()) == false)
        {
            context.Close();
            return;
        }
    }

    /// <inheritdoc />
    public void OnClosed(WebSocketContextDefault context)
    {
        _list.TryRemove(context, out _);
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
        public JsonElement Data { get; set; }
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