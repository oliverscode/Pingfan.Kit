using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    public class StopwatchEx
    {
        /// <summary>
        /// 计算执行时间
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="onComplete">执行完成</param>
        /// <returns></returns>
        public static TimeSpan Run(Action fn, Action<TimeSpan> onComplete = null)
        {
            var sw = new Stopwatch();
            sw.Start();
            fn();
            sw.Stop();

            onComplete?.Invoke(sw.Elapsed);
            return sw.Elapsed;
        }

        /// <summary>
        /// 计算执行时间
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="onComplete">执行完成</param>
        /// <returns></returns>
        public static async Task<TimeSpan> Run(Func<Task> fn, Action<TimeSpan> onComplete = null)
        {
            var sw = new Stopwatch();
            sw.Start();
            await fn();
            sw.Stop();
            onComplete?.Invoke(sw.Elapsed);
            return sw.Elapsed;
        }
    }
}