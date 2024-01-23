using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text.Json;

namespace Pingfan.Kit.WebServer.Interfaces;

/// <summary>
/// Http请求接口
/// </summary>
public interface IHttpRequest : IDisposable
{
    internal HttpListenerContext HttpListenerContext { get; }

    /// <summary>
    /// 请求的Url
    /// </summary>
    Uri? Url { get; }
    
    /// <summary>
    /// 请求的的路径, 不包含QueryString
    /// </summary>
    string Path { get; }
    
    /// <summary>
    /// 请求方法
    /// </summary>
    string Method { get; }
    
    /// <summary>
    /// 输入流
    /// </summary>
    Stream InputStream { get; }
    
    /// <summary>
    /// 请求头
    /// </summary>
    NameValueCollection Headers { get; }
    
    /// <summary>
    /// 
    /// </summary>
    string UserAgent { get; }
    
    /// <summary>
    /// 客户端Ip
    /// </summary>
    string Ip { get; }
    
    /// <summary>
    /// 客户端设备类型
    /// </summary>
    string Device { get; }
    
    /// <summary>
    /// 请求字符串
    /// </summary>
    string QueryString { get; }
    
    /// <summary>
    /// 请求体
    /// </summary>
    string Body { get; }
    
    /// <summary>
    /// 请求的所有Cookie
    /// </summary>
    CookieCollection Cookies { get; }
    
    /// <summary>
    /// 获取请求参数
    /// </summary>
    string? Get(string key);
    
    /// <summary>
    /// 获取请求参数
    /// </summary>
    string? Form(string key);
    
    /// <summary>
    /// 获取请求参数
    /// </summary>
    JsonDocument? Json { get; }

    /// <summary>
    /// 获取某一个Cookie
    /// </summary>
    string? GetCookie(string key);
    
    /// <summary>
    /// 获取请求头, 顺序为:POST->GET->Cookie->Header
    /// </summary>
    string? this[string key] { get; }
}