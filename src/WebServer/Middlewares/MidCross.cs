using System;
using Pingfan.Kit.Inject;
using Pingfan.Kit.WebServer.Interfaces;

namespace Pingfan.Kit.WebServer.Middlewares;

/// <summary>
/// 跨域中间件
/// </summary>
public class MidCross : IMiddleware
{
    /// <inheritdoc />
    public void Invoke(IContainer container, IHttpContext ctx, Action next)
    {
        ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        ctx.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        ctx.Response.Headers.Add("Access-Control-Allow-Headers",
            "Content-Type, Authorization, Accept, X-Requested-With, Origin, Referer, User-Agent");
        ctx.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
        ctx.Response.Headers.Add("Access-Control-Max-Age", "1728000");
        if (ctx.Request.Method == "OPTIONS")
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.End();
        }
        else
        {
            next();
        }
    }
}
/// <summary>
/// 扩展
/// </summary>
public static class MidCrossEx
{
    /// <summary>
    /// 使用Cross中间件
    /// </summary>
    /// <param name="webServer"></param>
    /// <param name="action"></param>
    public static WebServer UseCross(this WebServer webServer, Action<MidCross>? action)
    {
        var mid = webServer.Container.New<MidCross>();
        action?.Invoke(mid);
        webServer.Use(mid);
        return webServer;
    }
}