using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Pingfan.Kit
{
    /// <summary>
    /// 封装的线程安全集合
    /// </summary>
    public class ThreadSafeList<T> : List<T>
    {
        private readonly ReaderWriterLockSlim _lock = new();

        /// <summary>
        /// 添加元素
        /// </summary>
        public new void Add(T item)
        {
            _lock.EnterWriteLock();
            try
            {
                base.Add(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }


        /// <summary>
        /// 添加元素
        /// </summary>
        public new bool Remove(T item)
        {
            _lock.EnterWriteLock();
            try
            {
                return base.Remove(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 是否包含元素
        /// </summary>
        public new bool Contains(T item)
        {
            _lock.EnterReadLock();
            try
            {
                return base.Contains(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


        /// <summary>
        /// 查找元素
        /// </summary>
        public new int IndexOf(T item)
        {
            _lock.EnterReadLock();
            try
            {
                return base.IndexOf(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


        /// <summary>
        /// 插入元素
        /// </summary>
        public new void Insert(int index, T item)
        {
            _lock.EnterWriteLock();
            try
            {
                base.Insert(index, item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 移除指定索引的元素
        /// </summary>
        public new void RemoveAt(int index)
        {
            _lock.EnterWriteLock();
            try
            {
                base.RemoveAt(index);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 获取或设置元素
        /// </summary>
        public new T this[int index]
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return base[index];
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
            set
            {
                _lock.EnterWriteLock();
                try
                {
                    base[index] = value;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }


        /// <summary>
        /// 获取元素数量
        /// </summary>
        public new int Count
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return base.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }



        /// <summary>
        /// 清空所有元素
        /// </summary>
        public new void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                base.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }


        
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            base.Clear();
            _lock.Dispose();
        }
    }
}