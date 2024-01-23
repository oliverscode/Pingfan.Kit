using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using Pingfan.Kit.WebServer.Interfaces;
#pragma warning disable CS0618 // Type or member is obsolete

namespace Pingfan.Kit.WebServer;

/// <summary>
/// HTTP默认响应
/// </summary>
public class HttpResponseDefault : IHttpResponse
{
    /// <inheritdoc />
    public HttpListenerContext HttpListenerContext { get; }

    /// <summary>
    /// HttpListenerResponse
    /// </summary>
    public HttpListenerResponse HttpListenerResponse => HttpListenerContext.Response;

    /// <summary>
    /// HttpListenerRequest
    /// </summary>
    public HttpListenerRequest HttpListenerRequest => HttpListenerContext.Request;

    /// <summary>
    /// 构造函数
    /// </summary>
    public HttpResponseDefault(HttpListenerContext httpListenerContext)
    {
        HttpListenerContext = httpListenerContext;
    }


    /// <summary>
    /// 关闭连接并且释放
    /// </summary>
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

            HttpListenerResponse.Close();
        }
        catch
        {
            //
        }
        finally
        {
            OutputStream.Dispose();
        }
    }

    private JsonSerializerOptions? _jsonSerializerOptions;

    /// <inheritdoc />
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

    /// <inheritdoc />
    public WebHeaderCollection Headers => HttpListenerResponse.Headers;

    /// <inheritdoc />
    public bool SendChunked
    {
        get => HttpListenerResponse.SendChunked;
        set => HttpListenerResponse.SendChunked = value;
    }

    /// <inheritdoc />
    public bool KeepAlive
    {
        get => HttpListenerResponse.KeepAlive;
        set => HttpListenerResponse.KeepAlive = value;
    }

    private MemoryStream? _outputStream;

    /// <inheritdoc />
    public MemoryStream OutputStream => _outputStream ??= new MemoryStream();

    /// <inheritdoc />
    public Encoding ContentEncoding
    {
        get => HttpListenerResponse.ContentEncoding ?? (HttpListenerResponse.ContentEncoding = Encoding.UTF8);
        set => HttpListenerResponse.ContentEncoding = value;
    }

    /// <inheritdoc />
    public int StatusCode
    {
        get => HttpListenerResponse.StatusCode;
        set => HttpListenerResponse.StatusCode = value;
    }

    /// <inheritdoc />
    public string ContentType
    {
        get => HttpListenerResponse.ContentType!;
        set => HttpListenerResponse.ContentType = value;
    }

    /// <inheritdoc />
    public void Redirect(string url)
    {
        HttpListenerResponse.Redirect(url);
        End();
    }

    /// <inheritdoc />
    public void SetCookie(string key, string value, DateTime expires)
    {
        var cookie = new Cookie(key, value)
        {
            Expires = expires
        };
        HttpListenerResponse.Cookies.Add(cookie);
    }

    /// <inheritdoc />
    public void Write(string text)
    {
        var buffer = ContentEncoding.GetBytes(text);
        Write(buffer);
    }

    /// <inheritdoc />
    public void Write(object? json)
    {
        if (json == null) return;
        var text = JsonSerializer.Serialize(json, JsonSerializerOptions);
        Write(text);
    }

    /// <inheritdoc />
    public void Write(byte[] buffer)
    {
        OutputStream.Write(buffer, 0, buffer.Length);
    }

    /// <inheritdoc />
    public void Write(Stream stream)
    {
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);

            // 将inputStream的内容复制到outputStream
            stream.CopyTo(HttpListenerContext.Response.OutputStream);
        }
    }

    /// <inheritdoc />
    public void Clear()
    {
        OutputStream.SetLength(0);
    }

    /// <inheritdoc />
    public void End()
    {
        throw new HttpEndException();
    }
}