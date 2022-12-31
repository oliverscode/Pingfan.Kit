using System;
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
                    catch (Exception e)
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
                    catch (Exception e)
                    {
                    }
                }
            });
        }
    }
}