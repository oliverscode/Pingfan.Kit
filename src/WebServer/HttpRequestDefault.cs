using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using Pingfan.Kit.WebServer.Interfaces;

namespace Pingfan.Kit.WebServer;

/// <summary>
/// HTTP默认请求
/// </summary>
public class HttpRequestDefault : IHttpRequest
{
    /// <inheritdoc />
    public HttpListenerContext HttpListenerContext { get; }

    /// <summary>
    /// HttpListenerRequest
    /// </summary>
    public HttpListenerRequest HttpListenerRequest => HttpListenerContext.Request;

    /// <summary>
    /// 构造函数
    /// </summary>
    public HttpRequestDefault(HttpListenerContext httpListenerContext)
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
            InputStream.Dispose();
            HttpListenerContext.Request.InputStream.Dispose();
        }
        catch
        {
            //
        }
    }


    /// <inheritdoc />
    public Uri Url => HttpListenerRequest.Url!;

    private string _path = null!;

    /// <inheritdoc />
    public string Path
    {
        get
        {
            if (string.IsNullOrEmpty(_path))
            {
                var path = Regex.Replace(Url.LocalPath, "//", "/");
                path = Regex.Replace(path, @"\\", @"\");
                _path = path;
            }

            return _path;
        }
        set => _path = value;
    }

    /// <inheritdoc />
    public string Method => HttpListenerRequest.HttpMethod.ToUpper();

    /// <inheritdoc />
    public Stream InputStream => HttpListenerRequest.InputStream;

    /// <inheritdoc />
    public NameValueCollection Headers => HttpListenerRequest.Headers;

    /// <inheritdoc />
    public string UserAgent => HttpListenerRequest.UserAgent ?? "";


    private string _ip = "";

    /// <inheritdoc />
    public string Ip
    {
        get
        {
            if (!string.IsNullOrEmpty(_ip))
                return _ip!;
            string[] ipHeads = { "CF-Connecting-IP", "X_FORWARDED_FOR", "X-Forwarded-For", "X-Real-IP" };
            var ips = ipHeads.Select(head => HttpListenerRequest.Headers[head])
                .Where(t => string.IsNullOrEmpty(t) == false)
                .ToList();

            ips.Add(HttpListenerRequest.RemoteEndPoint?.Address?.ToString() ?? string.Empty);

            // 只需要第一个IP
            foreach (var m in ips.Select(ip => Regex.Match(ip!, @"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})"))
                         .Where(m => m.Success))
            {
                _ip = m.Groups[1].Value;
                break;
            }

            return _ip!;
        }
    }

    private string? _device = "";

    /// <inheritdoc />
    public string Device
    {
        get
        {
            if (!string.IsNullOrEmpty(_device)) return _device!;


            if (HttpListenerRequest.UserAgent!.IndexOf("Android", StringComparison.OrdinalIgnoreCase) >= 0)
                _device = "Android";
            else if (HttpListenerRequest.UserAgent.IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) >= 0)
                _device = "iPhone";
            else if (HttpListenerRequest.UserAgent.IndexOf("iPad", StringComparison.OrdinalIgnoreCase) >= 0)
                _device = "iPad";
            else if (HttpListenerRequest.UserAgent.IndexOf("Windows Phone", StringComparison.OrdinalIgnoreCase) >= 0)
                _device = "Windows Phone";
            else if (HttpListenerRequest.UserAgent.IndexOf("Windows NT", StringComparison.OrdinalIgnoreCase) >= 0)
                _device = "Windows";
            else if (HttpListenerRequest.UserAgent.IndexOf("Mac OS", StringComparison.OrdinalIgnoreCase) >= 0)
                _device = "Mac OS";
            else if (HttpListenerRequest.UserAgent.IndexOf("Linux", StringComparison.OrdinalIgnoreCase) >= 0)
                _device = "Linux";
            else _device = "Other";

            return _device;
        }
    }


    private string? _queryString = "";

    /// <inheritdoc />
    public string QueryString
    {
        get
        {
            if (string.IsNullOrEmpty(_queryString) == false) return _queryString!;
            if (HttpListenerRequest.Url == null) return "";

            _queryString = HttpListenerRequest.Url.Query;
            if (_queryString.StartsWith("?"))
                _queryString = _queryString.Substring(1);

            return _queryString;
        }
    }

    private string? _postString;

    /// <inheritdoc />
    public string Body
    {
        get
        {
            if (_postString != null) return _postString;
            using var sr = new StreamReader(HttpListenerRequest.InputStream);
            _postString = sr.ReadToEnd();
            return _postString;
        }
        set => _postString = value;
    }

    /// <inheritdoc />
    public CookieCollection Cookies => HttpListenerRequest.Cookies;

    /// <inheritdoc />
    public string? Get(string key)
    {
        return GetValue(QueryString, key)!;
    }

    /// <inheritdoc />
    public string? Form(string key)
    {
        return GetValue(Body, key)!;
    }

    private JsonDocument? _json;

    /// <inheritdoc />
    public JsonDocument? Json
    {
        get
        {
            if (_json != null) return _json;
            if (string.IsNullOrEmpty(Body)) return null;
            try
            {
                _json = JsonDocument.Parse(Body);
                return _json;
            }
            catch
            {
                return null;
            }
        }
    }

    /// <inheritdoc />
    public string? GetCookie(string key)
    {
        var cookie = HttpListenerRequest.Cookies[key];
        return cookie?.Value;
    }

    /// <inheritdoc />
    public string? this[string key]
    {
        // post->get->cookie->header
        get
        {
            var value = ((Form(key) ?? Get(key)) ?? GetCookie(key)) ?? Headers[key];
            return value;
        }
    }

    private static string? GetValue(string? source, string key)
    {
        if (string.IsNullOrEmpty(source)) return null;
        var m = Regex.Match(source, $@"{key}=(?<value>[^&]*)");
        return m.Success ? m.Groups["value"].Value : null;
    }
}