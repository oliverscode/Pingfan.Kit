using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Pingfan.Kit
{
    /// <summary>
    /// 封装的线程安全集合
    /// </summary>
    public class ThreadSafeList<T> : IList<T>
    {
        private readonly List<T> _list;
        private readonly ReaderWriterLockSlim _lock = new();

        /// <summary>
        /// 构造函数
        /// </summary>
        public ThreadSafeList()
        {
            _list = new List<T>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ThreadSafeList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ThreadSafeList(IEnumerable<T> list) : this()
        {
            foreach (var item in list)
            {
                _list.Add(item);
            }
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        public void Add(T item)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Add(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        public void AddRange(IEnumerable<T> items)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.AddRange(items);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }


        /// <summary>
        /// 添加元素
        /// </summary>
        public bool Remove(T item)
        {
            _lock.EnterWriteLock();
            try
            {
                return _list.Remove(item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        public int RemoveAll(Predicate<T> match)
        {
            _lock.EnterWriteLock();
            try
            {
                return _list.RemoveAll(match);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public void Sort()
            => Sort(0, Count, null);


        /// <summary>
        /// 排序
        /// </summary>
        public void Sort(IComparer<T>? comparer)
            => Sort(0, Count, comparer);

        /// <summary>
        /// 排序
        /// </summary>
        public void Sort(int index, int count, IComparer<T>? comparer)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Sort(index, count, comparer);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public void Sort(Comparison<T> comparison)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Sort(comparison);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }


        /// <summary>
        /// 是否包含元素
        /// </summary>
        public bool Contains(T item)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.Contains(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


        /// <summary>
        /// 查找元素
        /// </summary>
        public int IndexOf(T item)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.IndexOf(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


        /// <summary>
        /// 插入元素
        /// </summary>
        public void Insert(int index, T item)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Insert(index, item);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 移除指定索引的元素
        /// </summary>
        public void RemoveAt(int index)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.RemoveAt(index);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 获取或设置元素
        /// </summary>
        public T this[int index]
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _list[index];
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
                    _list[index] = value;
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
        public int Count
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _list.Count;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <summary>
        /// 复制
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _lock.EnterReadLock();
            try
            {
                _list.CopyTo(array, arrayIndex);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 是否存在元素
        /// </summary>
        public bool Exists(Predicate<T> match)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.Exists(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 查找元素
        /// </summary>
        public T? Find(Predicate<T> match)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.Find(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 查找所有元素
        /// </summary>
        public List<T> FindAll(Predicate<T> match)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.FindAll(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 查找元素索引
        /// </summary>
        public int FindIndex(Predicate<T> match)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.FindIndex(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 查找元素索引
        /// </summary>
        public T? FindLast(Predicate<T> match)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.FindLast(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 查找元素索引
        /// </summary>
        public int FindLastIndex(Predicate<T> match)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.FindLastIndex(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 遍历元素
        /// </summary>
        public void ForEach(Action<T> action)
        {
            _lock.EnterReadLock();
            try
            {
                _list.ForEach(action);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 获取元素
        /// </summary>
        public List<T> GetRange(int index, int count)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.GetRange(index, count);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.InsertRange(index, collection);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 查找元素索引
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int LastIndexOf(T item)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.LastIndexOf(item);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 移除元素
        /// </summary>
        public void RemoveRange(int index, int count)
        {
            _lock.EnterWriteLock();
            try
            {
                _list.RemoveRange(index, count);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 反转元素
        /// </summary>
        public void Reverse()
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Reverse();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 转成数组
        /// </summary>
        public T[] ToArray()
        {
            _lock.EnterReadLock();
            try
            {
                return _list.ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 移除空位置的的占用
        /// </summary>
        public void TrimExcess()
        {
            _lock.EnterWriteLock();
            try
            {
                _list.TrimExcess();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 是否所有元素都满足条件
        /// </summary>
        public bool TrueForAll(Predicate<T> match)
        {
            _lock.EnterReadLock();
            try
            {
                return _list.TrueForAll(match);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


        /// <summary>
        /// 清空所有元素
        /// </summary>
        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _list.Clear();
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
            _list.Clear();
            _lock.Dispose();
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            List<T> tempList;
            _lock.EnterReadLock();
            try
            {
                tempList = new List<T>(_list);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return tempList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}