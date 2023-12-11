namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 容器事件接口
    /// </summary>
    public interface IContainerReady
    {
        /// <summary>
        /// 注入完成后调用
        /// </summary>
        void OnContainerReady();
    }
}