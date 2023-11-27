using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 定时器封装
    /// </summary>
    public static class Timer
    {
        /// <summary>
        /// 错误事件
        /// </summary>
        public static event Action<Exception>? OnError;

        /// <summary>
        /// 创建一个定时器，但只执行1次
        /// </summary>
        public static Task SetTimeout(
            int milliSecond,
            Action method,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Factory.StartNew(async () =>
            {
                await Task.Delay(milliSecond, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    return;
                method();
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器，但只执行1次, 同时不抛出异常
        /// </summary>
        public static Task SetTimeoutWithTry(
            int milliSecond,
            Action method,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Factory.StartNew(async () =>
            {
                await Task.Delay(milliSecond, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    return;

                try
                {
                    method();
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器，但只执行1次, 同时不抛出异常
        /// </summary>
        public static Task SetTimeoutWithTryAsync(
            int milliSecond,
            Func<Task> method,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Factory.StartNew(async () =>
            {
                await Task.Delay(milliSecond, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    return;
                try
                {
                    await method();
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器
        /// </summary>
        public static Task SetInterval(
            int milliSecond,
            Action method,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    method();
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// 创建一个定时器, 同时不抛出异常
        /// </summary>
        public static Task SetIntervalWithTry(
            int milliSecond,
            Action method,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        method();
                    }
                    catch (Exception e)
                    {
                        OnError?.Invoke(e);
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// 创建一个定时器
        /// </summary>
        public static Task SetIntervalAsync(
            int milliSecond,
            Func<Task> method,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    await method();
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }


        /// <summary>
        /// 创建一个定时器, 同时不抛出异常
        /// </summary>
        public static Task SetIntervalWithTryAsync(int milliSecond,
            Func<Task> method,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        await method();
                    }
                    catch (Exception e)
                    {
                        OnError?.Invoke(e);
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 准点执行, 不能在夏令时地区使用, hour, minute, second为-1时, 则会忽略该参数
        /// </summary>
        public static Task SetTime(
            int hour,
            int minute,
            int second,
            Action method,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    var now = DateTime.Now;
                    if (hour != -1 && now.Hour != hour)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    if (minute != -1 && now.Minute != minute)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    if (second != -1 && now.Second != second)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    method();
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(1000, cancellationToken);
                }
            }, cancellationToken);
        }

        /// <summary>
        /// 准点执行, 不能在夏令时地区使用, hour, minute, second为-1时, 则会忽略该参数
        /// </summary>
        public static Task SetTimeAsync(
            int hour,
            int minute,
            int second,
            Func<Task> method,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    var now = DateTime.Now;
                    if (hour != -1 && now.Hour != hour)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    if (minute != -1 && now.Minute != minute)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    if (second != -1 && now.Second != second)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    await method();
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(1000, cancellationToken);
                }
            }, cancellationToken);
        }

        /// <summary>
        /// 准点执行, 不能在夏令时地区使用, hour, minute, second为-1时, 则会忽略该参数
        /// </summary>
        public static Task SetTimeWithTry(
            int hour,
            int minute,
            int second,
            Action method,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    var now = DateTime.Now;
                    if (hour != -1 && now.Hour != hour)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    if (minute != -1 && now.Minute != minute)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    if (second != -1 && now.Second != second)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    try
                    {
                        method();
                    }
                    catch (Exception e)
                    {
                        OnError?.Invoke(e);
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(1000, cancellationToken);
                }
            }, cancellationToken);
        }

        /// <summary>
        /// 准点执行, 不能在夏令时地区使用, hour, minute, second为-1时, 则会忽略该参数
        /// </summary>
        public static Task SetTimeWithTryAsync(
            int hour,
            int minute,
            int second,
            Func<Task> method,
            CancellationToken cancellationToken = default
        )
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    var now = DateTime.Now;
                    if (hour != -1 && now.Hour != hour)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    if (minute != -1 && now.Minute != minute)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    if (second != -1 && now.Second != second)
                    {
                        await Task.Delay(1000, cancellationToken);
                        continue;
                    }

                    try
                    {
                        await method();
                    }
                    catch (Exception e)
                    {
                        OnError?.Invoke(e);
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(1000, cancellationToken);
                }
            }, cancellationToken);
        }
    }
}