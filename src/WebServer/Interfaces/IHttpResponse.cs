using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Pingfan.Kit.WebServer.Interfaces;

/// <summary>
/// Http响应接口
/// </summary>
public interface IHttpResponse : IDisposable
{
    internal HttpListenerContext HttpListenerContext { get; }
    
    /// <summary>
    /// JSON序列化选项
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; }
    /// <summary>
    /// 响应头
    /// </summary>
    WebHeaderCollection Headers { get; }
    /// <summary>
    /// 是否分块传输, 默认为true
    /// </summary>
    bool SendChunked { get; set; }
    
    /// <summary>
    /// 是否保持连接, 默认为true
    /// </summary>
    bool KeepAlive { get; set; }
    
    /// <summary>
    /// 输出流
    /// </summary>
    MemoryStream OutputStream { get; }
    
    /// <summary>
    /// 输出编码, 默认为UTF-8
    /// </summary>
    Encoding ContentEncoding { get; set; }
    
    /// <summary>
    /// 状态码
    /// </summary>
    int StatusCode { get; set; }
    
    /// <summary>
    /// 输出内容类型
    /// </summary>
    string ContentType { get; set; }
    
    /// <summary>
    /// 重定向
    /// </summary>
    /// <param name="url"></param>
    void Redirect(string url);
    
    /// <summary>
    /// 设置Cookie
    /// </summary>
    void SetCookie(string key, string value, DateTime expires);
    
    /// <summary>
    /// 输出内容
    /// </summary>
    void Write(string text);
    
    /// <summary>
    /// 输出内容
    /// </summary>
    void Write(object? json);
    
    /// <summary>
    /// 输出内容
    /// </summary>
    void Write(byte[] buffer);
    
    /// <summary>
    /// 输出内容
    /// </summary>
    void Write(Stream stream);
    
    /// <summary>
    /// 清空输出内容
    /// </summary>
    void Clear();
    
    /// <summary>
    /// 终止执行
    /// </summary>
    void End();
}