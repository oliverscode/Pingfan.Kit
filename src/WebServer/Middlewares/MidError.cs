﻿using System;
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


    /// <inheritdoc />
    public void Invoke(IContainer container, IHttpContext ctx, Action next)
    {
        try
        {
            next();
        }
        catch (HttpEndException)
        {
        }
        catch (HttpArgumentException)
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

/// <summary>
/// 扩展
/// </summary>
public static class MidErrorEx{
    /// <summary>
    /// 添加错误中间件
    /// </summary>
    public static WebServer UseError(this WebServer webServer, Action<MidError>? action = null)
    {
        var mid = webServer.Container.New<MidError>();
        action?.Invoke(mid);
        webServer.Use(mid);
        return webServer;
    }
}