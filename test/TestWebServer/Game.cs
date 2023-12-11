using System.Net.WebSockets;
using System.Text;
using Pingfan.Kit.WebServer.Interfaces;
using Pingfan.Kit.WebServer.Middlewares.Websockets;

namespace TestWebServer;

public class Game : WebSocketContextDefault
{
    public Game(HttpListenerWebSocketContext httpListenerWebSocketContext, IHttpResponse httpResponse,
        Encoding encoding) : base(httpListenerWebSocketContext, httpResponse, encoding)
    {
    }

    public override void OnOpen()
    {
        Console.WriteLine("打开了");
    }

    public override void OnClose()
    {
        Console.WriteLine("关闭了");
    }

    public override void OnMessage(string message)
    {
        Console.WriteLine(message);
        Send("收到你发送的消息了:" + message);
    }

    public override void OnBinary(byte[] data)
    {
        Console.WriteLine(data);
    }

    public override bool OnCheck()
    {
        return true;
    }
}