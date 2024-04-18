using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 定时器封装
    /// </summary>
    public static class Ticker
    {
        /// <summary>
        /// 错误事件
        /// </summary>
        public static event Action<Exception>? OnError;

        /// <summary>
        /// 创建一个定时器，但只执行1次
        /// </summary>
        public static CancellationTokenSource Once(int milliSecond, Action method)
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(milliSecond, token.Token);
                if (token.IsCancellationRequested)
                    return;
                method();
            }, token.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            return token;
        }

        /// <summary>
        /// 创建一个定时器，但只执行1次, 同时不抛出异常
        /// </summary>
        public static CancellationTokenSource OnceWithTry(int milliSecond, Action method)
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(milliSecond, token.Token);
                if (token.IsCancellationRequested)
                    return;

                try
                {
                    method();
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e);
                }
            }, token.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            return token;
        }

        /// <summary>
        /// 创建一个定时器，但只执行1次, 同时不抛出异常
        /// </summary>
        public static CancellationTokenSource OnceWithTryAsync(
            int milliSecond,
            Func<Task> method
        )
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(milliSecond, token.Token);
                if (token.IsCancellationRequested)
                    return;
                try
                {
                    await method();
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e);
                }
            }, token.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            return token;
        }

        /// <summary>
        /// 创建一个定时器
        /// </summary>
        public static CancellationTokenSource Loop(int milliSecond, Action method)
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                while (token.IsCancellationRequested == false)
                {
                    method();
                    if (token.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, token.Token);
                }
            }, token.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            return token;
        }

        /// <summary>
        /// 创建一个定时器, 同时不抛出异常
        /// </summary>
        public static CancellationTokenSource LoopWithTry(int milliSecond, Action method)
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                while (token.IsCancellationRequested == false)
                {
                    try
                    {
                        method();
                    }
                    catch (Exception e)
                    {
                        OnError?.Invoke(e);
                    }

                    if (token.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, token.Token);
                }
            }, token.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            return token;
        }

        /// <summary>
        /// 创建一个定时器
        /// </summary>
        public static CancellationTokenSource LoopAsync(int milliSecond, Func<Task> method)
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                while (token.IsCancellationRequested == false)
                {
                    await method();
                    if (token.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, token.Token);
                }
            }, token.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            return token;
        }


        /// <summary>
        /// 创建一个定时器, 同时不抛出异常
        /// </summary>
        public static CancellationTokenSource LoopWithTryAsync(int milliSecond, Func<Task> method)
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                while (token.IsCancellationRequested == false)
                {
                    try
                    {
                        await method();
                    }
                    catch (Exception e)
                    {
                        OnError?.Invoke(e);
                    }

                    if (token.IsCancellationRequested)
                        return;
                    await Task.Delay(milliSecond, token.Token);
                }
            }, token.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
            return token;
        }

        /// <summary>
        /// 准点执行, 不能在夏令时地区使用, hour, minute, second为-1时, 则会忽略该参数
        /// </summary>
        public static CancellationTokenSource At(int hour, int minute, int second, Action method)
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                while (token.IsCancellationRequested == false)
                {
                    var now = DateTime.Now;
                    if (hour != -1 && now.Hour != hour)
                    {
                        await Task.Delay(1000, token.Token);
                        continue;
                    }

                    if (minute != -1 && now.Minute != minute)
                    {
                        await Task.Delay(1000, token.Token);
                        continue;
                    }

                    if (second != -1 && now.Second != second)
                    {
                        await Task.Delay(1000, token.Token);
                        continue;
                    }

                    method();
                    if (token.IsCancellationRequested)
                        return;
                    await Task.Delay(1000, token.Token);
                }
            }, token.Token);
            return token;
        }

        /// <summary>
        /// 准点执行, 不能在夏令时地区使用, hour, minute, second为-1时, 则会忽略该参数
        /// </summary>
        public static CancellationTokenSource AtAsync(int hour, int minute, int second, Func<Task> method)
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                while (token.IsCancellationRequested == false)
                {
                    var now = DateTime.Now;
                    if (hour != -1 && now.Hour != hour)
                    {
                        await Task.Delay(1000, token.Token);
                        continue;
                    }

                    if (minute != -1 && now.Minute != minute)
                    {
                        await Task.Delay(1000, token.Token);
                        continue;
                    }

                    if (second != -1 && now.Second != second)
                    {
                        await Task.Delay(1000, token.Token);
                        continue;
                    }

                    await method();
                    if (token.IsCancellationRequested)
                        return;
                    await Task.Delay(1000, token.Token);
                }
            }, token.Token);
            return token;
        }

        /// <summary>
        /// 准点执行, 不能在夏令时地区使用, hour, minute, second为-1时, 则会忽略该参数
        /// </summary>
        public static CancellationTokenSource AtWithTry(int hour, int minute, int second, Action method)
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                while (token.IsCancellationRequested == false)
                {
                    var now = DateTime.Now;
                    if (hour != -1 && now.Hour != hour)
                    {
                        await Task.Delay(1000, token.Token);
                        continue;
                    }

                    if (minute != -1 && now.Minute != minute)
                    {
                        await Task.Delay(1000, token.Token);
                        continue;
                    }

                    if (second != -1 && now.Second != second)
                    {
                        await Task.Delay(1000, token.Token);
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

                    if (token.IsCancellationRequested)
                        return;
                    await Task.Delay(1000, token.Token);
                }
            }, token.Token);
            return token;
        }

        /// <summary>
        /// 准点执行, 不能在夏令时地区使用, hour, minute, second为-1时, 则会忽略该参数
        /// </summary>
        public static CancellationTokenSource AtWithTryAsync(int hour, int minute, int second, Func<Task> method)
        {
            var token = new CancellationTokenSource();
            Task.Factory.StartNew(async () =>
            {
                while (token.IsCancellationRequested == false)
                {
                    var now = DateTime.Now;
                    if (hour != -1 && now.Hour != hour)
                    {
                        await Task.Delay(1000, token.Token);
                        continue;
                    }

                    if (minute != -1 && now.Minute != minute)
                    {
                        await Task.Delay(1000, token.Token);
                        continue;
                    }

                    if (second != -1 && now.Second != second)
                    {
                        await Task.Delay(1000, token.Token);
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
                    
                    await Task.Delay(1000, token.Token);
                }
            }, token.Token);
            return token;
        }
    }
}