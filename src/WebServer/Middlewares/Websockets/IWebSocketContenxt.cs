using System.Net.WebSockets;
using System.Text;

namespace Pingfan.Kit.WebServer.Middlewares.Websockets;

/// <summary>
/// WebSocket上下文接口
/// </summary>
public interface IWebSocketContext
{
    /// <summary>
    /// 是否已经连接上
    /// </summary>
    bool IsAvailable { get; }
    
    /// <summary>
    /// WebSocket的实例
    /// </summary>
    WebSocket WebSocket { get; set; }
    
    /// <summary>
    /// 编码, 默认为UTF8
    /// </summary>
    Encoding Encoding { get; }
    
    /// <summary>
    /// 协议
    /// </summary>
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

    /// <summary>
    /// 发送数据
    /// </summary>
    void Send(object json);
    
    /// <summary>
    /// 发送数据
    /// </summary>
    void Send(string message);
    
    /// <summary>
    /// 发送数据
    /// </summary>
    void Send(byte[] data);

    /// <summary>
    /// 关闭连接
    /// </summary>
    void Close();
}