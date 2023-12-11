namespace Pingfan.Kit.WebServer.Middlewares;

/// <summary>
/// 中间件请求检查参数
/// </summary>
public interface IMidRequestModel
{
    /// <summary>
    /// 检查是否符合要求
    /// </summary>
    void Check();
}