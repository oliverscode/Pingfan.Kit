using System;
using Pingfan.Kit.Inject;
using Pingfan.Kit.WebServer.Interfaces;

namespace Pingfan.Kit.WebServer.Middlewares;
/// <summary>
/// 错误中间件
/// </summary>
public class MidError : IMiddleware
{
    /// <summary>
    /// 是否向客户端返回错误信息, 默认显示错误信息
    /// </summary>
    public bool ShowError { get; set; } = true;
    

    /// <summary>
    /// 当发生错误时
    /// </summary>
    public event Action<IHttpContext, Exception>? OnError;

    
    public void Invoke(IContainer container, IHttpContext ctx, Action next)
    {
        try
        {
            next();
        }
        catch (HttpEndException e)
        {
        }
        catch (HttpArgumentException e)
        {
            
        }
        catch (Exception e)
        {
            OnError?.Invoke(ctx, e);
            if (ctx.Response.StatusCode < 500)
                ctx.Response.StatusCode = 500;
            if (ShowError)
            {
                ctx.Response.Clear();
                ctx.Response.Write(e.ToString());
            }
            
        }
    }
}