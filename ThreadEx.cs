using System.Threading;

namespace Pingfan.Kit
{
    /// <summary>
    /// 线程扩展
    /// </summary>
    public static class ThreadEx
    {
        /// <summary>
        /// 支持一个范围的延迟
        /// </summary>
        /// <param name="minMilliseconds"></param>
        /// <param name="maxMilliseconds"></param>
        public static void Sleep(int minMilliseconds, int maxMilliseconds)
        {
            Thread.Sleep(RandomEx.Next(minMilliseconds, maxMilliseconds));
        }
    }
}