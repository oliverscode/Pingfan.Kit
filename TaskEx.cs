
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// Task扩展
    /// </summary>
    public static class TaskEx
    {
        /// <summary>
        /// 支持一个范围的延迟
        /// </summary>
        /// <param name="minMilliseconds"></param>
        /// <param name="maxMilliseconds"></param>
        /// <returns></returns>
        public static Task Delay(int minMilliseconds, int maxMilliseconds)
        {
            return Task.Delay(RandomEx.Next(minMilliseconds, maxMilliseconds));
        }

  
    }
}

