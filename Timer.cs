using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    public static class Timer
    {
        /// <summary>
        /// 创建一个定时器，但只执行1次
        /// </summary>
        public static Task SetTimeout(int milliSecond, Action action, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                await Task.Delay(milliSecond, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    return;
                action();
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器，但只执行1次, 同时不抛出异常
        /// </summary>
        public static Task SetTimeoutWithTry(int milliSecond,
            Action action,
            Action<Exception> errAction = null,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                await Task.Delay(milliSecond, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    return;

                try
                {
                    action();
                }
                catch (Exception e)
                {
                    errAction?.Invoke(e);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器，但只执行1次, 同时不抛出异常
        /// </summary>
        public static Task SetTimeoutWithTry(int milliSecond,
            Func<Task> action,
            Func<Exception, Task> errAction = null,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                await Task.Delay(milliSecond, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    return;
                try
                {
                    await action();
                }
                catch (Exception e)
                {
                    await errAction?.Invoke(e);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器
        /// </summary>
        public static Task SetInterval(int milliSecond, Action action, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    action();
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// 创建一个定时器, 同时不抛出异常
        /// </summary>
        public static Task SetIntervalWithTry(int milliSecond,
            Action action,
            Action<Exception> errAction = null,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        errAction?.Invoke(e);
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
        public static Task SetInterval(int milliSecond,
            Func<Task> action,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    await action();
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器, 同时不抛出异常
        /// </summary>
        public static Task SetIntervalWithTry(int milliSecond,
            Func<Task> action,
            Action<Exception> errAction = null,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        await action();
                    }
                    catch (Exception e)
                    {
                        errAction?.Invoke(e);
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器, 同时不抛出异常
        /// </summary>
        public static Task SetIntervalWithTry(int milliSecond,
            Func<Task> action,
            Func<Exception, Task> errAction = null,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        await action();
                    }
                    catch (Exception e)
                    {
                        await errAction?.Invoke(e);
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器, 如果返回false, 将终止定时器
        /// </summary>
        public static Task SetInterval(int milliSecond,
            Func<bool> action,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    var result = action();
                    //判断是否返回的false
                    if (result == false)
                    {
                        return;
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器, 如果返回false, 将终止定时器, 同时不抛出异常
        /// </summary>
        public static Task SetIntervalWithTry(int milliSecond,
            Func<bool> action,
            Action<Exception> errAction = null,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        var result = action();
                        //判断是否返回的false
                        if (result == false)
                        {
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        errAction?.Invoke(e);
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器
        /// </summary>
        public static Task SetInterval(int milliSecond,
            Func<Task<bool>> action,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    var result = await action();
                    //判断是否返回的false
                    if (result == false)
                    {
                        return;
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// 创建一个定时器, 如果返回false, 将终止定时器, 同时不抛出异常
        /// </summary>
        public static Task SetIntervalWithTry(int milliSecond,
            Func<Task<bool>> action,
            Func<Exception, Task> errAction = null,
            CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    try
                    {
                        var result = await action();
                        //判断是否返回的false
                        if (result == false)
                        {
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        await errAction?.Invoke(e);
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, cancellationToken);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }
    }
}