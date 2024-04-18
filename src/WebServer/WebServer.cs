using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using Pingfan.Kit.Inject;
using Pingfan.Kit.WebServer.Interfaces;

namespace Pingfan.Kit.WebServer;

/// <summary>
/// Web服务器, 只能通过App.UseWebServer来使用
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class WebServer : IContainerReady
{
    private readonly HttpListener _httpListener = new();
    private readonly List<IMiddleware> _middlewares = new();

    private static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);


    /// <summary>
    /// 容器
    /// </summary>
    [Inject]
    public IContainer Container { get; set; } = null!;


    /// <summary>
    /// 配置
    /// </summary>
    public WebServerConfig Config { get; }


    /// <summary>
    /// 请求开始前执行
    /// </summary>
    public event Action<IContainer, IHttpContext>? BeginRequest;

    /// <summary>
    /// 仅仅在BeginRequest之后执行
    /// </summary>
    public event Action<IContainer, IHttpContext>? Handler;


    /// <summary>
    /// 请求结束后执行
    /// </summary>
    public event Action<IContainer, IHttpContext>? EndRequest;

    /// <summary>
    /// 有异常时执行
    /// </summary>
    public event Action<IContainer, IHttpContext, Exception>? RequestError;


    /// <summary>
    /// 构造函数
    /// </summary>
    public WebServer(WebServerConfig config)
    {
        Config = config;

        _httpListener.Prefixes.Add($"http://*:{config.Port}/");
        _httpListener.Start();
    }

    /// <inheritdoc />
    public void OnContainerReady()
    {
        StartListen();
    }


    // 开始循环获取请求
    private async void StartListen()
    {
        while (true)
        {
            var httpListenerContext = await _httpListener.GetContextAsync();
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                var context = (HttpListenerContext)obj!;
                ExecuteHttpContext(context);
            }, httpListenerContext);
        }
        // ReSharper disable once FunctionNeverReturns
    }

    // 正式执行流程
    private void ExecuteHttpContext(HttpListenerContext httpListenerContext)
    {
        var httpContainer = Container.CreateContainer();
        httpContainer.Register<IHttpContext>(Config.HttpContextType);
        httpContainer.Register<IHttpRequest>(Config.HttpRequestType);
        httpContainer.Register<IHttpResponse>(Config.HttpResponseType);

        var items = new Dictionary<string, object>();
        httpContainer.Register(items);


        httpContainer.Register(httpListenerContext);

        var httpContext = (IHttpContext)httpContainer.Get(Config.HttpContextType);

        try
        {
            if (IsWindows)
                httpListenerContext.Response.Headers["Server"] = "";
            else
                httpListenerContext.Response.Headers["Server"] = Config.DefaultServerName;
            httpListenerContext.Response.Headers["Date"] = "";


            this.BeginRequest?.Invoke(httpContainer, httpContext);


            this.Handler?.Invoke(httpContainer, httpContext);

            // 执行中间件
            var midIndex = 0;
            Action? func = null;
            func = () =>
            {
                if (midIndex < _middlewares.Count)
                {
                    // ReSharper disable once AccessToDisposedClosure
                    _middlewares[midIndex++].Invoke(httpContainer, httpContext, func!);
                }
            };
            func();


            this.EndRequest?.Invoke(httpContainer, httpContext);
        }
        catch (HttpEndException)
        {
        }
        catch (HttpArgumentException)
        {
        }
        catch (Exception ex)
        {
            if (httpContext.Response.StatusCode < 500)
                httpContext.Response.StatusCode = 500;
            this.RequestError?.Invoke(httpContainer, httpContext, ex);
        }
        finally
        {
            httpContainer.Dispose();
        }
    }

    /// <summary>
    /// 添加中间件
    /// </summary>
    public void Use(IMiddleware middleware)
    {
        _middlewares.Add(middleware);
    }
}

/// <summary>
/// 扩展
/// </summary>
public static class WebServerExtensions
{
    /// <summary>
    /// 使用WebServer, 同时会注入WebServerConfig
    /// </summary>
    public static WebServer UseWebServer(this IContainer container, Action<WebServerConfig> func)
    {
        var config = new WebServerConfig();
        func(config);
        container.Register(config);
        return container.New<WebServer>();
    }
}