using System.Collections.Generic;
using Pingfan.Kit.WebServer.Interfaces;

namespace Pingfan.Kit.WebServer;

/// <summary>
/// Http上下文默认实现
/// </summary>
public class HttpContextDefault : IHttpContext
{
    /// <inheritdoc />
    public IHttpRequest Request { get; }

    /// <inheritdoc />
    public IHttpResponse Response { get; }

    /// <inheritdoc />
    public Dictionary<string, object> Items { get; }


    /// <summary>
    /// 构造函数
    /// </summary>
    public HttpContextDefault(IHttpRequest request, IHttpResponse response, Dictionary<string, object> items)
    {
        Request = request;
        Response = response;
        Items = items;
    }

    /// <summary>
    /// 关闭连接并且释放所有资源
    /// </summary>
    public void Dispose()
    {
        Request.Dispose();
        Response.Dispose();
        Items.Clear();
    }
}