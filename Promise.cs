using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 类似js的Promise
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Promise<T>
    {
        /// <summary>
        /// 获取结果
        /// </summary>
        public T Result => _task.Result;

        private readonly Task<T> _task;

        /// <summary>
        /// 类似js的Promise
        /// </summary>
        /// <param name="resolve"></param>
        public Promise(Action<Action<T>> resolve)
        {
            var tcs = new TaskCompletionSource<T>();
            var p = new Action<T>((o) => tcs.SetResult(o));
            resolve(p);
            _task = tcs.Task;
        }

        /// <summary>
        /// 类似js的Promise
        /// </summary>
        /// <param name="resolve"></param>
        public Promise(Func<Action<T>, Task> resolve)
        {
            var tcs = new TaskCompletionSource<T>();
            var p = new Action<T>((o) => tcs.SetResult(o));
            _task = Task.Run(async () =>
            {
                await resolve(p).ConfigureAwait(false);
                await tcs.Task.ConfigureAwait(false);
                return await tcs.Task.ConfigureAwait(false);
            });
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return _task.GetAwaiter();
        }

    }
}
