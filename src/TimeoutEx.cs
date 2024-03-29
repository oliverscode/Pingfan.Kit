﻿using System;
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
        public static void Run(Action action, double seconds)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var task = Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (OperationCanceledException)
                {
                }
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
        public static async Task RunAsync(Func<Task> func, double seconds)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var delayTask = Task.Delay(TimeSpan.FromSeconds(seconds), token);
            var task = func();

            var completedTask = await Task.WhenAny(task, delayTask);
            if (completedTask == task)
            {
                await task; // 如果任务已完成，则等待以观察任何异常
            }
            else
            {
                // ReSharper disable once MethodHasAsyncOverload
                cts.Cancel(); // 取消延迟任务
                throw new TimeoutException("The operation has timed out.");
            }
        }
    }
}