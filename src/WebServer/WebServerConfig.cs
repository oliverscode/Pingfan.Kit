using System;

namespace Pingfan.Kit.WebServer;

public class WebServerConfig
{
    public int Port { get; set; } = 5000;
    public string DefaultServerName { get; set; } = "oliver";
    
    public Type HttpContextType { get; set; } = typeof(HttpContextDefault);
    public Type HttpRequestType { get; set; } = typeof(HttpRequestDefault);
    public Type HttpResponseType { get; set; } = typeof(HttpResponseDefault);
}