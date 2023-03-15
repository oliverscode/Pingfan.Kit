using System.Reflection;
using System.Threading;

namespace Pingfan.Kit
{
    public static class ProcessEx
    {
        /// <summary>
        /// 判断是否运行过
        /// </summary>
        public static bool HasRun
        {
            get
            {
                new Mutex(initiallyOwned: true, Assembly.GetExecutingAssembly().GetType().GUID.ToString(), out var createdNew);
                return !createdNew;
            }
        }
    }
}