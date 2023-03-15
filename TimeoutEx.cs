using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{

    /// <summary>
    /// 可以判断超时的类
    /// </summary>
    public static class TimeoutEx
    {
        /// <summary>
        /// 可以超时的方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="seconds"></param>
        /// <exception cref="TimeoutException"></exception>
        public static void RunWithTimeout(Action action, double seconds)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task task = Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (OperationCanceledException) { }
            }, token);

            if (!task.Wait(TimeSpan.FromSeconds(seconds)))
            {
                cts.Cancel();
                throw new TimeoutException("The operation has timed out.");
            }
        }

        /// <summary>
        /// 可以超时的异步方法
        /// </summary>
        /// <param name="func"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        /// <exception cref="TimeoutException"></exception>
        public static async Task RunWithTimeout(Func<Task> func, double seconds)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task delayTask = Task.Delay(TimeSpan.FromSeconds(seconds), token);
            Task task = func();

            var completedTask = await Task.WhenAny(task, delayTask);
            if (completedTask == task)
            {
                await task; // 如果任务已完成，则等待以观察任何异常
            }
            else
            {
                cts.Cancel(); // 取消延迟任务
                throw new TimeoutException("The operation has timed out.");
            }
        }
    }
}