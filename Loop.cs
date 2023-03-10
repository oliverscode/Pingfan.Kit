using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    public class Loop
    {
        /// <summary>
        /// 死循环运行一个方法
        /// </summary>
        /// <param name="action"></param>
        public static Task Run(Action action)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    action();
                }
            });
        }

        /// <summary>
        /// 死循环运行一个方法
        /// </summary>
        /// <param name="action"></param>
        public static Task RunWithTry(Action action)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        action();
                    }
                    catch
                    {
                    }
                }
            });
        }


        /// <summary>
        /// 死循环运行一个方法
        /// </summary>
        /// <param name="action"></param>
        public static Task Run(Func<Task> action)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    await action();
                }
            });
        }

        /// <summary>
        /// 死循环运行一个方法
        /// </summary>
        /// <param name="action"></param>
        public static Task RunWithTry(Func<Task> action)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await action();
                    }
                    catch
                    {
                    }
                }
            });
        }

        /// <summary>
        /// 一直等待, 通常用于控制台程序等待执行完成, 又不需要从流中读取数据
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
        /// <param name="count"></param>
        /// <param name="method"></param>
        public static void Run(int count, Action<int> method)
        {
            for (var i = 1; i <= count; i++)
            {
                method(i);
            }
        }

        public static async Task Run(int count, Func<Task> method)
        {
            for (var i = 0; i < count; i++)
            {
                await method();
            }
        }

        /// <summary>
        /// 重复执行指定次数的方法, i从1开始
        /// </summary>
        /// <param name="count"></param>
        /// <param name="method"></param>
        public static async Task Run(int count, Func<int, Task> method)
        {
            for (var i = 1; i <= count; i++)
            {
                await method(i);
            }
        }
    }
}