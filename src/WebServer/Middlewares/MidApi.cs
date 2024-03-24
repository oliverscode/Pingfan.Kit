using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Pingfan.Kit.Inject;
using Pingfan.Kit.WebServer.Interfaces;

namespace Pingfan.Kit.WebServer.Middlewares;

/// <summary>
/// Api中间件, 会自动注入ControllerName和ActionName
/// API的方法支持参数绑定, 支持基本类型, 支持继承IMidRequestModel的类
/// </summary>
public class MidApi : IMiddleware
{
    /// <summary>
    /// 控制器列表
    /// </summary>
    private readonly List<ControllerItem> _controllers = new List<ControllerItem>();

    // [Inject]
    // public IContainer Container { get; set; } = null!;

    /// <summary>
    /// 默认控制器, 默认是/Home/Index
    /// </summary>
    public string DefaultController { get; set; } = "/Home/Index";


    /// <inheritdoc />
    public void Invoke(IContainer container, IHttpContext ctx, Action next)
    {
        var path = ctx.Request.Path;
        if (path == "/")
            path = DefaultController;


        var item = _controllers.FirstOrDefault(p => string.Equals(p.Path, path, StringComparison.OrdinalIgnoreCase));
        if (item == null)
        {
            next();
            return;
        }

        container.Register<string>(item.InstanceType.Name, "ControllerName");
        container.Register<string>(item.MethodInfo.Name, "ActionName");

        var args = new object?[item.ParameterInfos.Length];
        for (var i = 0; i < args.Length; i++)
        {
            var parameterInfo = item.ParameterInfos[i];
            var value = ctx.Request[parameterInfo.Name!];

            try
            {
                if (parameterInfo.ParameterType == typeof(string))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        value = (string?)parameterInfo.DefaultValue;
                    args[i] = value;
                }
                else if (parameterInfo.ParameterType == typeof(int))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        args[i] = parameterInfo.DefaultValue;
                    args[i] = int.Parse(value!);
                }
                else if (parameterInfo.ParameterType == typeof(long))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        args[i] = parameterInfo.DefaultValue;
                    args[i] = long.Parse(value!);
                }
                else if (parameterInfo.ParameterType == typeof(uint))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        args[i] = parameterInfo.DefaultValue;
                    args[i] = uint.Parse(value!);
                }
                else if (parameterInfo.ParameterType == typeof(ulong))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        args[i] = parameterInfo.DefaultValue;
                    args[i] = ulong.Parse(value!);
                }
                else if (parameterInfo.ParameterType == typeof(float))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        args[i] = parameterInfo.DefaultValue;
                    args[i] = float.Parse(value!);
                }
                else if (parameterInfo.ParameterType == typeof(double))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        args[i] = parameterInfo.DefaultValue;
                    args[i] = double.Parse(value!);
                }
                else if (parameterInfo.ParameterType == typeof(decimal))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        args[i] = parameterInfo.DefaultValue;
                    args[i] = decimal.Parse(value!);
                }
                else if (parameterInfo.ParameterType == typeof(bool))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        args[i] = parameterInfo.DefaultValue;
                    args[i] = bool.Parse(value!);
                }
                else if (parameterInfo.ParameterType == typeof(DateTime))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        args[i] = parameterInfo.DefaultValue;
                    args[i] = DateTime.Parse(value!);
                }
                else if (parameterInfo.ParameterType == typeof(JsonDocument))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        args[i] = parameterInfo.DefaultValue;
                    args[i] = JsonDocument.Parse(value!);
                }
                else if (parameterInfo.ParameterType == typeof(BigInteger))
                {
                    if (value == null && parameterInfo.HasDefaultValue)
                        args[i] = parameterInfo.DefaultValue;
                    args[i] = BigInteger.Parse(value!);
                }
            }
            catch (Exception)
            {
                throw new HttpArgumentException($"{parameterInfo.Name}参数不正确", parameterInfo.ParameterType,
                    parameterInfo.Name!);
            }


            // 是否parameterInfo.ParameterType是否继承IMidRequestModel
            if (parameterInfo.ParameterType.IsClass &&
                parameterInfo.ParameterType.GetInterfaces().Any(t => t == typeof(IMidRequestModel)))
            {
                IMidRequestModel? requestModel = null;
                try
                {
                    requestModel =
                        (IMidRequestModel)JsonSerializer.Deserialize(ctx.Request.Body, parameterInfo.ParameterType)!;
                }
                catch (Exception e)
                {
                    var err = new HttpArgumentException($"{parameterInfo.Name}参数不正确", parameterInfo.ParameterType,
                        parameterInfo.Name!, e);
                    throw err;
                }

                requestModel.Check();
                args[i] = requestModel;
            }
        }


        var instance = container.New(item.InstanceType);
        var methodInfo = item.MethodInfo;
        var obj = methodInfo.Invoke(instance, args);

        // 处理返回值
        switch (obj)
        {
            case Task<object> task:
            {
                task.ContinueWith(t =>
                {
                    var result = t.Result;
                    ctx.Response.Write(result);
                });
                break;
            }
            case Task task:
            {
                task.Wait();
                break;
            }
            case string s:
            {
                ctx.Response.Write(s);
                break;
            }
            case byte[] bytes:
            {
                ctx.Response.Write(bytes);
                break;
            }
            case Stream stream:
            {
                ctx.Response.Write(stream);
                break;
            }
            case null:
            {
                break;
            }
            default:
            {
                ctx.Response.Write(obj);
                break;
            }
        }


        next();
    }


    /// <summary>
    /// 添加一组控制器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Add<T>() where T : class
    {
        Add<T>(null);
    }

    /// <summary>
    /// 添加一组控制器
    /// </summary>
    /// <param name="urlPrefix">/urlPrefix/控制器/方法名</param>
    /// <typeparam name="T"></typeparam>
    public void Add<T>(string? urlPrefix) where T : class
    {
        var type = typeof(T);
        Add(urlPrefix, type);
    }

    /// <summary>
    /// 添加一组控制器
    /// </summary>
    /// <param name="urlPrefix">/urlPrefix/控制器/方法名</param>
    /// <param name="type"></param>
    /// <exception cref="Exception"></exception>
    public void Add(string? urlPrefix, Type type)
    {
        var methodInfos = type.GetMethods();
        foreach (var methodInfo in methodInfos)
        {
            // 不是公开的, 而且不能是构造函数, 同时不能是父类的方法
            if (!methodInfo.IsPublic || methodInfo.IsConstructor || methodInfo.DeclaringType != type)
                continue;


            var isAsync = methodInfo.IsDefined(typeof(AsyncStateMachineAttribute), false);
            if (isAsync)
            {
                // 判断返回值是否是Task
                if (methodInfo.ReturnType != typeof(Task) && methodInfo.ReturnType.BaseType != typeof(Task))
                    throw new Exception($"{type.Name}->{methodInfo.Name} 异步方法必须返回Task");
            }

            var item = new ControllerItem()
            {
                Path = string.IsNullOrWhiteSpace(urlPrefix)
                    ? $"/{type.Name}/{methodInfo.Name}"
                    : $"/{urlPrefix}/{type.Name}/{methodInfo.Name}",
                MethodInfo = methodInfo,
                InstanceType = type,
                ParameterInfos = methodInfo.GetParameters(),
            };

            _controllers.Add(item);

            // var methodType = 
            // Container.Push(  );
        }
    }

    private class ControllerItem
    {
        public string Path { get; set; } = null!;

        public Type InstanceType { get; set; } = null!;

        public MethodInfo MethodInfo { get; set; } = null!;

        public ParameterInfo[] ParameterInfos { get; set; } = null!;
        // public object? Instance { get; set; }
    }
}

/// <summary>
/// 扩展
/// </summary>
public static class MidApiEx
{
    /// <summary>
    /// 使用Api中间件
    /// </summary>
    /// <param name="webServer"></param>
    /// <param name="action"></param>
    public static WebServer UseApi(this WebServer webServer, Action<MidApi>? action)
    {
        var mid = webServer.Container.New<MidApi>();
        action?.Invoke(mid);
        webServer.Use(mid);
        return webServer;
    }
}