using System;
using Pingfan.Kit.Inject;


namespace Pingfan.Kit.WebServer.Interfaces;

/// <summary>
/// 中间件接口
/// </summary>
public interface IMiddleware
{
    /// <summary>
    /// HTTP请求时开始执行
    /// </summary>
    /// <param name="container">本请求内唯一的容器</param>
    /// <param name="ctx">HTTP上下文</param>
    /// <param name="next">开始执行下一个中间件</param>
    void Invoke(IContainer container, IHttpContext ctx, Action next);
}