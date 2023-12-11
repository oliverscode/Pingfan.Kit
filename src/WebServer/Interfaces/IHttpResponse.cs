using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Pingfan.Kit.WebServer.Interfaces;

public interface IHttpResponse : IDisposable
{
    internal HttpListenerContext HttpListenerContext { get; }
    
    public JsonSerializerOptions JsonSerializerOptions { get; set; }
    WebHeaderCollection Headers { get; }
    bool SendChunked { get; set; }
    bool KeepAlive { get; set; }
    MemoryStream OutputStream { get; }
    Encoding ContentEncoding { get; set; }
    int StatusCode { get; set; }
    string ContentType { get; set; }
    void Redirect(string url);
    void SetCookie(string key, string value, DateTime expires);
    void Write(string text);
    void Write(object? json);
    void Write(byte[] buffer);
    void Write(Stream stream);
    void Clear();
    void End();
}