using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pingfan.Kit.WebServer.Interfaces;

namespace Pingfan.Kit.WebServer;

public class HttpResponseDefault : IHttpResponse
{
    public HttpListenerContext HttpListenerContext { get; }
    public HttpListenerResponse HttpListenerResponse => HttpListenerContext.Response;
    public HttpListenerRequest HttpListenerRequest => HttpListenerContext.Request;

    public HttpResponseDefault(HttpListenerContext httpListenerContext)
    {
        HttpListenerContext = httpListenerContext;
    }


    public void Dispose()
    {
        try
        {
            // 发送数据
            if (OutputStream.CanRead && HttpListenerResponse.OutputStream.CanWrite)
            {
                if (string.IsNullOrWhiteSpace(ContentType))
                    ContentType = $"text/html; charset={ContentEncoding.WebName}";


                if (HttpListenerResponse.SendChunked == false || OutputStream.Length == 0)
                {
                    HttpListenerResponse.ContentLength64 = OutputStream.Length;
                }

                if (HttpListenerRequest.HttpMethod.Equals("HEAD") == false && OutputStream.Length > 0)
                {
                    OutputStream.WriteTo(HttpListenerResponse.OutputStream);
                }
            }
        }
        catch
        {
            //
        }
        finally
        {
            HttpListenerResponse.Close();
            OutputStream.Dispose();
        }
    }

    private JsonSerializerOptions? _jsonSerializerOptions;

    [Obsolete("Obsolete")]
    public JsonSerializerOptions JsonSerializerOptions
    {
        get
        {
            if (_jsonSerializerOptions == null)
            {
                _jsonSerializerOptions = new JsonSerializerOptions
                {
                    // 支持中文
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    // 忽略Null值
                    IgnoreNullValues = true,
                    // DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                };
            }

            return _jsonSerializerOptions;
        }
        set => _jsonSerializerOptions = value;
    }

    public WebHeaderCollection Headers => HttpListenerResponse.Headers;

    public bool SendChunked
    {
        get => HttpListenerResponse.SendChunked;
        set => HttpListenerResponse.SendChunked = value;
    }

    public bool KeepAlive
    {
        get => HttpListenerResponse.KeepAlive;
        set => HttpListenerResponse.KeepAlive = value;
    }

    private MemoryStream? _outputStream;

    public MemoryStream OutputStream => _outputStream ??= new MemoryStream();

    public Encoding ContentEncoding
    {
        get => HttpListenerResponse.ContentEncoding ?? (HttpListenerResponse.ContentEncoding = Encoding.UTF8);
        set => HttpListenerResponse.ContentEncoding = value;
    }

    public int StatusCode
    {
        get => HttpListenerResponse.StatusCode;
        set => HttpListenerResponse.StatusCode = value;
    }

    public string ContentType
    {
        get => HttpListenerResponse.ContentType!;
        set => HttpListenerResponse.ContentType = value;
    }

    public void Redirect(string url)
    {
        HttpListenerResponse.Redirect(url);
        End();
    }

    public void SetCookie(string key, string value, DateTime expires)
    {
        var cookie = new Cookie(key, value)
        {
            Expires = expires
        };
        HttpListenerResponse.Cookies.Add(cookie);
    }

    public void Write(string text)
    {
        var buffer = ContentEncoding.GetBytes(text);
        Write(buffer);
    }

    public void Write(object? json)
    {
        if (json == null) return;
        var text = JsonSerializer.Serialize(json, JsonSerializerOptions);
        Write(text);
    }

    public void Write(byte[] buffer)
    {
        OutputStream.Write(buffer, 0, buffer.Length);
    }

    public void Write(Stream stream)
    {
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);

            // 将inputStream的内容复制到outputStream
            stream.CopyTo(HttpListenerContext.Response.OutputStream);
        }
    }

    public void Clear()
    {
        OutputStream.SetLength(0);
    }


    public void End()
    {
        throw new HttpEndException();
    }
}