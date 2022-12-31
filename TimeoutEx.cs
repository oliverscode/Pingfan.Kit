using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    public class TimeoutEx
    {
        /// <summary>
        /// 执行一个方法,并在指定的时间内返回结果
        /// </summary>
        /// <param name="action"></param>
        /// <param name="seconds"></param>
        /// <exception cref="TimeoutException"></exception>
        public static void Run(Action action, double seconds)
        {
            var thread = new Thread(() => action());
            thread.Start();
            thread.Join(TimeSpan.FromSeconds(seconds));
            if (thread.IsAlive)
            {
                thread.Abort();
                throw new TimeoutException();
            }
        }

        /// <summary>
        /// 执行一个方法,并在指定的时间内返回结果
        /// </summary>
        /// <param name="action"></param>
        /// <param name="seconds"></param>
        /// <exception cref="TimeoutException"></exception>
        public static void Run(Func<Task> action, double seconds)
        {
            var thread = new Thread(async () => await action());
            thread.Start();
            thread.Join(TimeSpan.FromSeconds(seconds));
            if (thread.IsAlive)
            {
                thread.Abort();
                throw new TimeoutException();
            }
        }
    }
}