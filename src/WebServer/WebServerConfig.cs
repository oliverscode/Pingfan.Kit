using System;

namespace Pingfan.Kit.WebServer;

/// <summary>
/// Web服务器配置
/// </summary>
public class WebServerConfig
{
    /// <summary>
    /// 端口, 默认为5000
    /// </summary>
    public int Port { get; set; } = 5000;
    
    /// <summary>
    /// 默认服务器名, 默认为空
    /// </summary>
    public string DefaultServerName { get; set; } = "";
    
    /// <summary>
    /// 指定HttpContext的类型, 默认为HttpContextDefault
    /// </summary>
    public Type HttpContextType { get; set; } = typeof(HttpContextDefault);
    
    
    /// <summary>
    /// 指定HttpRequest的类型, 默认为HttpRequestDefault
    /// </summary>
    public Type HttpRequestType { get; set; } = typeof(HttpRequestDefault);
    
    /// <summary>
    /// 指定HttpResponse的类型, 默认为HttpResponseDefault
    /// </summary>
    public Type HttpResponseType { get; set; } = typeof(HttpResponseDefault);
}