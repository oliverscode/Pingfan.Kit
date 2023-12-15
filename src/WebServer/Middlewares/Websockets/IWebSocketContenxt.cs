using System.Net.WebSockets;
using System.Text;

namespace Pingfan.Kit.WebServer.Middlewares.Websockets;

/// <summary>
/// 
/// </summary>
public interface IWebSocketContext
{
    bool IsAvailable { get; }
    WebSocket WebSocket { get; set; }
    Encoding Encoding { get; }
    string Protocol { get; }
    
    
    /// <summary>
    /// 检查请求是否合法
    /// </summary>
    bool OnCheck(string protocol);

    /// <summary>
    /// 客户端已经连接上后
    /// </summary>
    void OnOpen();

    /// <summary>
    /// 客户端关闭后
    /// </summary>
    void OnClose();

    /// <summary>
    /// 收到数据
    /// </summary>
    void OnBinary(byte[] data);

    /// <summary>
    /// 收到文本数据
    /// </summary>
    void OnMessage(string message);

    void Send(object json);
    void Send(string message);
    void Send(byte[] data);

    void Close();
}