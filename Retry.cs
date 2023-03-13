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
        /// 结尾为True直接返回
        /// </summary>
        /// <param name="count"></param>
        /// <param name="delay"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool IsTrue(int count, int delay, Func<bool> method)
        {
            for (var i = 0; i < count; i++)
            {
                try
                {
                    var result = method();
                    if (result)
                        return true;
                }
                catch (Exception e)
                {
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            return false;
        }

        /// <summary>
        /// 结尾为True直接返回
        /// </summary>
        /// <param name="count"></param>
        /// <param name="delay"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static async Task<bool> IsTrue(int count, int delay, Func<Task<bool>> method)
        {
            for (var i = 0; i < count; i++)
            {
                try
                {
                    var result = await method();
                    if (result)
                        return true;
                }
                catch (Exception e)
                {
                    if (delay > 0)
                        await Task.Delay(delay);
                }
            }

            return false;
        }
    }
}