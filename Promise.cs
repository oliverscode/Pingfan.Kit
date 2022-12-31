using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    public class Promise<T>
    {
        private readonly Task<T> _task = null;

        public Promise(Action<Action<T>> resolve)
        {
            _task = Task.Run<T>(async () =>
            {
                var cancel = new CancellationTokenSource();
                var result = default(T);
                var p = new Action<T>((o) =>
                {
                    result = o;
                    cancel.Cancel();
                });
                resolve(p);
                //等待被调用
                while (cancel.IsCancellationRequested == false)
                {
                    await Task.Delay(1);
                }

                return result;
            });
        }

        public Promise(Func<Action<T>, Task> resolve)
        {
            _task = Task.Run<T>(async () =>
            {
                var cancel = new CancellationTokenSource();
                var result = default(T);
                var p = new Action<T>((o) =>
                {
                    result = o;
                    cancel.Cancel();
                });
                await resolve(p);
                //等待被调用
                while (cancel.IsCancellationRequested == false)
                {
                    await Task.Delay(1);
                }

                return result;
            });
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return _task.GetAwaiter();
        }

        public void OnCompleted(Action continuation)
        {
        }
    }
}