using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    public class Retry
    {
        /// <summary>
        /// 间隔1秒一直重试1个任务
        /// </summary>
        /// <param name="method"></param>
        public static void Run(Action method, Action<Exception> errorCallback = null)
        {
            Run(int.MaxValue, 1000, method, errorCallback);
        }

        /// <summary>
        /// 间隔1秒一直重试1个任务
        /// </summary>
        /// <param name="method"></param>
        public static async Task Run(Func<Task> method, Action<Exception> errorCallback = null)
        {
            await Run(int.MaxValue, 1000, method, errorCallback);
        }

        /// <summary>
        /// 抛出异常就重试指定任务
        /// </summary>
        /// <param name="count">重试次数</param>
        /// <param name="delay">失败后等待毫秒数</param>
        /// <param name="method"></param>
        /// <exception cref="Exception"></exception>
        public static void Run(int count, int delay, Action method, Action<Exception> errorCallback = null)
        {
            Exception exception = null;
            for (var i = 0; i < count; i++)
            {
                try
                {
                    method();
                    return;
                }
                catch (Exception e)
                {
                    exception = e;
                    errorCallback?.Invoke(e);
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            throw exception;
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
        public static T Run<T>(int count, int delay, Func<T> method, Action<Exception> errorCallback = null)
        {
            Exception exception = null;
            for (var i = 0; i < count; i++)
            {
                try
                {
                    return method();
                }
                catch (Exception e)
                {
                    exception = e;
                    errorCallback?.Invoke(e);
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            throw exception;
        }

        /// <summary>
        /// 抛出异常就重试指定任务
        /// </summary>
        /// <param name="count">重试次数</param>
        /// <param name="delay">失败后等待毫秒数</param>
        /// <param name="method"></param>
        /// <exception cref="Exception"></exception>
        public static async Task Run(int count, int delay, Func<Task> method, Action<Exception> errorCallback = null)
        {
            Exception exception = null;
            for (var i = 0; i < count; i++)
            {
                try
                {
                    await method();
                    return;
                }
                catch (Exception e)
                {
                    exception = e;
                    errorCallback?.Invoke(e);
                    if (delay > 0)
                        await Task.Delay(delay);
                }
            }

            throw exception;
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
        public static async Task<T> Run<T>(int count, int delay, Func<Task<T>> method,
            Action<Exception> errorCallback = null)
        {
            Exception exception = null;
            for (var i = 0; i < count; i++)
            {
                try
                {
                    return await method();
                }
                catch (Exception e)
                {
                    exception = e;
                    errorCallback?.Invoke(e);
                    if (delay > 0)
                        await Task.Delay(delay);
                }
            }

            throw exception;
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
                        default:
                            break;
                    }

                    Thread.Sleep(delay);
                }

                return RetryKind.Wait;
            }
            catch (Exception e)
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
        public static async Task<RetryKind> Until(int count, int delay, Func<Task> executeMethod, Func<int, RetryKind> reTryMethod)
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
                        default:
                            break;
                    }

                    await Task.Delay(delay);
                }

                return RetryKind.Wait;
            }
            catch (Exception e)
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
        public static async Task<RetryKind> Until(int count, int delay, Action executeMethod, Func<int, Task<RetryKind>> reTryMethod)
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
                        default:
                            break;
                    }

                    await Task.Delay(delay);
                }

                return RetryKind.Wait;
            }
            catch (Exception e)
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
        public static async Task<RetryKind> Until(int count, int delay,  Func<Task> executeMethod, Func<int, Task<RetryKind>> reTryMethod)
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
                        default:
                            break;
                    }

                    await Task.Delay(delay);
                }

                return RetryKind.Wait;
            }
            catch (Exception e)
            {
                return RetryKind.Exception;
            }
        }
    }


    public enum RetryKind
    {
        // 完成
        Complete,

        // 重试
        Retry,

        // 异常
        Exception,

        // 正常等待
        Wait,
    }
}