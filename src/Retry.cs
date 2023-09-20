using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    public class Retry
    {
        public static event Action<Exception> OnError;

        /// <summary>
        /// 间隔1秒一直重试1个任务
        /// </summary>
        public static void Run(Action method)
        {
            Run(int.MaxValue, 1000, method);
        }

        /// <summary>
        /// 间隔1秒一直重试1个任务
        /// </summary>
        public static async Task RunAsync(Func<Task> method)
        {
            await Run(int.MaxValue, 1000, method);
        }

        /// <summary>
        /// 抛出异常就重试指定任务
        /// </summary>
        /// <param name="count">重试次数</param>
        /// <param name="delay">失败后等待毫秒数</param>
        /// <param name="method"></param>
        /// <exception cref="Exception"></exception>
        public static void Run(int count, int delay, Action method)
        {
            for (var i = 0; i < count; i++)
            {
                try
                {
                    method();
                    return;
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e);
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            // 抛出未完成的异常
            throw new Exception("Retry failed");
        }

        /// <summary>
        /// 抛出异常就重试指定任务
        /// </summary>
        /// <param name="count">重试次数</param>
        /// <param name="delay">失败后等待毫秒数</param>
        /// <param name="method"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T Run<T>(int count, int delay, Func<T> method)
        {
            for (var i = 0; i < count; i++)
            {
                try
                {
                    return method();
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e);
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            // 抛出未完成的异常
            throw new Exception("Retry failed");
        }

        /// <summary>
        /// 抛出异常就重试指定任务
        /// </summary>
        /// <param name="count">重试次数</param>
        /// <param name="delay">失败后等待毫秒数</param>
        /// <param name="method"></param>
        /// <exception cref="Exception"></exception>
        public static async Task RunAsync(int count, int delay, Func<Task> method)
        {
            for (var i = 0; i < count; i++)
            {
                try
                {
                    await method();
                    return;
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e);
                    if (delay > 0)
                        await Task.Delay(delay);
                }
            }

            // 抛出未完成的异常
            throw new Exception("Retry failed");
        }

        /// <summary>
        /// 抛出异常就重试指定任务
        /// </summary>
        /// <param name="count">重试次数</param>
        /// <param name="delay">失败后等待毫秒数</param>
        /// <param name="method"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<T> Run<T>(int count, int delay, Func<Task<T>> method)
        {
            for (var i = 0; i < count; i++)
            {
                try
                {
                    return await method();
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e);
                    if (delay > 0)
                        await Task.Delay(delay);
                }
            }

            // 抛出未完成的异常
            throw new Exception("Retry failed");
        }


        /// <summary>
        /// 失败重试
        /// </summary>
        /// <param name="count">执行总次数</param>
        /// <param name="delay">每次等待多久</param>
        /// <param name="executeMethod">执行的方法</param>
        /// <param name="reTryMethod">判断超时的方法</param>
        /// <returns></returns>
        public static RetryKind Until(int count, int delay, Action executeMethod, Func<int, RetryKind> reTryMethod)
        {
            try
            {
                executeMethod();

                for (var i = 0; i < count; i++)
                {
                    var result = reTryMethod(i + 1);
                    switch (result)
                    {
                        case RetryKind.Complete:
                            return RetryKind.Complete;
                        case RetryKind.Exception:
                            return RetryKind.Exception;
                        case RetryKind.Retry:
                            executeMethod();
                            break;
                        case RetryKind.Wait:
                            break;
                    }

                    Thread.Sleep(delay);
                }

                return RetryKind.Wait;
            }
            catch (Exception)
            {
                return RetryKind.Exception;
            }
        }


        /// <summary>
        /// 失败重试
        /// </summary>
        /// <param name="count">执行总次数</param>
        /// <param name="delay">每次等待多久</param>
        /// <param name="executeMethod">执行的方法</param>
        /// <param name="reTryMethod">判断超时的方法</param>
        /// <returns></returns>
        public static async Task<RetryKind> UntilAsync(int count, int delay, Func<Task> executeMethod,
            Func<int, RetryKind> reTryMethod)
        {
            try
            {
                await executeMethod();

                for (var i = 0; i < count; i++)
                {
                    var result = reTryMethod(i + 1);
                    switch (result)
                    {
                        case RetryKind.Complete:
                            return RetryKind.Complete;
                        case RetryKind.Exception:
                            return RetryKind.Exception;
                        case RetryKind.Retry:
                            await executeMethod();
                            break;
                        case RetryKind.Wait:
                            break;
                    }

                    await Task.Delay(delay);
                }

                return RetryKind.Wait;
            }
            catch (Exception)
            {
                return RetryKind.Exception;
            }
        }

        /// <summary>
        /// 失败重试
        /// </summary>
        /// <param name="count">执行总次数</param>
        /// <param name="delay">每次等待多久</param>
        /// <param name="executeMethod">执行的方法</param>
        /// <param name="reTryMethod">判断超时的方法</param>
        /// <returns></returns>
        public static async Task<RetryKind> Until(int count, int delay, Action executeMethod,
            Func<int, Task<RetryKind>> reTryMethod)
        {
            try
            {
                executeMethod();

                for (var i = 0; i < count; i++)
                {
                    var result = await reTryMethod(i + 1);
                    switch (result)
                    {
                        case RetryKind.Complete:
                            return RetryKind.Complete;
                        case RetryKind.Exception:
                            return RetryKind.Exception;
                        case RetryKind.Retry:
                            executeMethod();
                            break;
                        case RetryKind.Wait:
                            break;
                    }

                    await Task.Delay(delay);
                }

                return RetryKind.Wait;
            }
            catch (Exception)
            {
                return RetryKind.Exception;
            }
        }

        /// <summary>
        /// 失败重试
        /// </summary>
        /// <param name="count">执行总次数</param>
        /// <param name="delay">每次等待多久</param>
        /// <param name="executeMethod">执行的方法</param>
        /// <param name="reTryMethod">判断超时的方法</param>
        /// <returns></returns>
        public static async Task<RetryKind> Until(int count, int delay, Func<Task> executeMethod,
            Func<int, Task<RetryKind>> reTryMethod)
        {
            try
            {
                await executeMethod();

                for (var i = 0; i < count; i++)
                {
                    var result = await reTryMethod(i + 1);
                    switch (result)
                    {
                        case RetryKind.Complete:
                            return RetryKind.Complete;
                        case RetryKind.Exception:
                            return RetryKind.Exception;
                        case RetryKind.Retry:
                            await executeMethod();
                            break;
                        case RetryKind.Wait:
                            break;
                    }

                    await Task.Delay(delay);
                }

                return RetryKind.Wait;
            }
            catch (Exception)
            {
                return RetryKind.Exception;
            }
        }
    }


    public enum RetryKind
    {
        /// <summary>
        /// 完成
        /// </summary>
        Complete,

        /// <summary>
        /// 重试执行方法
        /// </summary>
        Retry,

        /// <summary>
        /// 发生异常
        /// </summary>
        Exception,

        /// <summary>
        /// 正常等待
        /// </summary>
        Wait,
    }
}