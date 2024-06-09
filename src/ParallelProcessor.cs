using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pingfan.Kit
{
    /// <summary>
    /// 并行处理器, 支持动态添加数据
    /// </summary>
    public class ParallelProcessor<T> : IDisposable
    {
        private readonly BlockingCollection<T> _queue = new(new ConcurrentQueue<T>());
        private readonly Action<T>? _syncMethod;
        private readonly Func<T, Task>? _asyncMethod;
        private readonly int _threadCount;
        private readonly Task[] _tasks;


        /// <summary>
        /// 队列是否已经完成
        /// </summary>
        public bool IsCompleted => _queue.IsCompleted;

        /// <summary>
        /// 完成进度百分比
        /// </summary>
        public int ProgressPercent => (int)(_queue.Count * 100.0 / _queue.BoundedCapacity);


        /// <summary>
        /// 构造方法（同步回调）
        /// </summary>
        /// <param name="method">处理每个元素的同步回调</param>
        /// <param name="threadCount">线程数量，默认为处理器核心数的两倍</param>
        public ParallelProcessor(Action<T>? method, int threadCount = 0)
        {
            _syncMethod = method ?? throw new ArgumentNullException(nameof(method));
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
            _asyncMethod = callback ?? throw new ArgumentNullException(nameof(callback));
            _threadCount = threadCount <= 0 ? Environment.ProcessorCount * 2 : threadCount;
            _tasks = new Task[_threadCount];
            StartWorkers(true);
        }

        /// <summary>
        /// 添加一个元素到队列并进行处理
        /// </summary>
        public void Push(T item)
        {
            _queue.Add(item);
        }

        /// <summary>
        /// 添加一组元素到队列并进行处理
        /// </summary>
        /// <param name="items"></param>
        public void Push(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                _queue.Add(item);
            }
        }

        /// <summary>
        /// 清理队列
        /// </summary>
        public void Clear()
        {
            _queue.Dispose();
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
                            await _asyncMethod!(item);
                        }
                        else
                        {
                            _syncMethod!(item);
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

        /// <summary>
        /// 等待所有任务完成
        /// </summary>
        public void WaitAll()
        {
            Task.WaitAll(_tasks);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Clear();
            foreach (var task in _tasks)
            {
                task.Dispose();
            }
        }
    }
}