using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 循环封装
    /// </summary>
    public static class Loop
    {
        /// <summary>
        /// 错误事件
        /// </summary>
        public static event Action<Exception>? OnError; 
        
        /// <summary>
        /// 死循环运行一个方法
        /// </summary>
        public static Task Run(Action method)
        {
            return Task.Run(function: () =>
            {
                while (true)
                {
                    method();
                }
            });
        }

        /// <summary>
        /// 死循环运行一个方法
        /// </summary>
        public static Task RunWithTry(Action method)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        method();
                    }
                    catch(Exception e)
                    {
                        OnError?.Invoke(e);
                    }
                }
            });
        }

        /// <summary>
        /// 死循环运行一个方法
        /// </summary>
        public static Task RunAsync(Func<Task> method)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    await method();
                }
            });
        }

        /// <summary>
        /// 死循环运行一个方法
        /// </summary>
        public static Task RunAsyncWithTry(Func<Task> method)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await method();
                    }
                    catch(Exception e)
                    {
                        OnError?.Invoke(e);
                    }
                }
            });
        }

        /// <summary>
        /// 一直等待, 通常用于等待执行完成, 又不需要从流中读取数据
        /// </summary>
        public static void Wait()
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                }
            })
            {
                IsBackground = true
            };
            thread.Start();
            thread.Join();
        }

        /// <summary>
        /// 重复执行指定次数的方法
        /// </summary>
        public static void Run(int count, Action method)
        {
            for (var i = 0; i < count; i++)
            {
                method();
            }
        }

        /// <summary>
        /// 重复执行指定次数的方法, i从1开始
        /// </summary>
        public static void Run(int count, Action<int> method)
        {
            for (var i = 1; i <= count; i++)
            {
                method(i);
            }
        }

        /// <summary>
        /// 重复执行指定次数的方法
        /// </summary>
        public static async Task RunAsync(int count, Func<Task> method)
        {
            for (var i = 0; i < count; i++)
            {
                await method();
            }
        }

        /// <summary>
        /// 重复执行指定次数的方法, i从1开始
        /// </summary>
        public static async Task RunAsync(int count, Func<int, Task> method)
        {
            for (var i = 1; i <= count; i++)
            {
                await method(i);
            }
        }
    }
}