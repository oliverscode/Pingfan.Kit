using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 并行处理器
    /// </summary>
    public class ParallelProcessor<T>
    {
        private readonly BlockingCollection<T> _queue = new BlockingCollection<T>(new ConcurrentQueue<T>());
        private readonly Action<T> _syncCallback;
        private readonly Func<T, Task> _asyncCallback;
        private readonly int _threadCount;
        private readonly Task[] _tasks;

        /// <summary>
        /// 构造方法（同步回调）
        /// </summary>
        /// <param name="callback">处理每个元素的同步回调</param>
        /// <param name="threadCount">线程数量，默认为处理器核心数的两倍</param>
        public ParallelProcessor(Action<T> callback, int threadCount = 0)
        {
            _syncCallback = callback ?? throw new ArgumentNullException(nameof(callback));
            _threadCount = threadCount <= 0 ? Environment.ProcessorCount * 2 : threadCount;
            _tasks = new Task[_threadCount];
            StartWorkers(false);
        }

        /// <summary>
        /// 构造方法（异步回调）
        /// </summary>
        /// <param name="callback">处理每个元素的异步回调</param>
        /// <param name="threadCount">线程数量，默认为处理器核心数的两倍</param>
        public ParallelProcessor(Func<T, Task> callback, int threadCount = 0)
        {
            _asyncCallback = callback ?? throw new ArgumentNullException(nameof(callback));
            _threadCount = threadCount <= 0 ? Environment.ProcessorCount * 2 : threadCount;
            _tasks = new Task[_threadCount];
            StartWorkers(true);
        }

        /// <summary>
        /// 添加一个元素到队列并进行处理
        /// </summary>
        public void Add(T item)
        {
            _queue.Add(item);
        }

        /// <summary>
        /// 开始工作线程
        /// </summary>
        private void StartWorkers(bool isAsync)
        {
            for (int i = 0; i < _threadCount; i++)
            {
                _tasks[i] = Task.Run(async () =>
                {
                    foreach (var item in _queue.GetConsumingEnumerable())
                    {
                        if (isAsync)
                        {
                            await _asyncCallback(item);
                        }
                        else
                        {
                            _syncCallback(item);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 等待所有任务完成
        /// </summary>
        public Task WhenAll()
        {
            return Task.WhenAll(_tasks);
        }
    }
}
