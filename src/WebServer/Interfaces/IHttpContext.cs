using System;
using System.Collections.Generic;

namespace Pingfan.Kit.WebServer.Interfaces;

/// <summary>
/// Http上下文接口
/// </summary>
public interface IHttpContext : IDisposable
{
    /// <summary>
    /// 请求
    /// </summary>
    IHttpRequest Request { get; }

    /// <summary>
    /// 响应
    /// </summary>
    IHttpResponse Response { get; }

    /// <summary>
    /// 自定义数据
    /// </summary>
    Dictionary<string, object> Items { get; }

}