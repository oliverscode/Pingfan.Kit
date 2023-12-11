using System;
using System.Collections.Generic;

namespace Pingfan.Kit.WebServer.Interfaces;

public interface IHttpContext : IDisposable
{
    IHttpRequest Request { get; }
    IHttpResponse Response { get; }
    Dictionary<string, object> Items { get; }

    // void NextMiddleware();
}