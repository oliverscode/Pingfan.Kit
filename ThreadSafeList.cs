using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Pingfan.Kit
{
    /// <summary>
    /// 线程安全的List, ConcurrentBag的包装
    /// </summary>
    public class ThreadSafeList<T> : List<T>
    {
        public ThreadSafeList()
        {
        }

        public ThreadSafeList(int capacity) : base(capacity)
        {
        }

        public ThreadSafeList(IEnumerable<T> collection) : base(collection)
        {
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            lock (this)
            {
                base.AddRange(collection);
            }
        }

        public new void AddRange(params T[] collection)
        {
            lock (this)
            {
                base.AddRange(collection);
            }
        }

        public new void Add(T item)
        {
            lock (this)
            {
                base.Add(item);
            }
        }


        public new void CopyTo(T[] array, int arrayIndex)
        {
            lock (this)
            {
                base.CopyTo(array, arrayIndex);
            }
        }

        public new bool Remove(T item)
        {
            lock (this)
            {
                return base.Remove(item);
            }
        }

        public new int RemoveAll(Predicate<T> match)
        {
            lock (this)
            {
                return base.RemoveAll(match);
            }
        }

        public new void RemoveAt(int index)
        {
            lock (this)
            {
                base.RemoveAt(index);
            }
        }

        public new void RemoveRange(int index, int count)
        {
            lock (this)
            {
                base.RemoveRange(index, count);
            }
        }

        public new void Insert(int index, T item)
        {
            lock (this)
            {
                base.Insert(index, item);
            }
        }

        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            lock (this)
            {
                base.InsertRange(index, collection);
            }
        }

        public void InsertRange(int index, params T[] collection)
        {
            lock (this)
            {
                base.InsertRange(index, collection);
            }
        }

        public new void Reverse()
        {
            lock (this)
            {
                base.Reverse();
            }
        }

        public new void Reverse(int index, int count)
        {
            lock (this)
            {
                base.Reverse(index, count);
            }
        }

        public new void Sort()
        {
            lock (this)
            {
                base.Sort();
            }
        }

        public new void Sort(Comparison<T> comparison)
        {
            lock (this)
            {
                base.Sort(comparison);
            }
        }

        public new void Sort(IComparer<T> comparer)
        {
            lock (this)
            {
                base.Sort(comparer);
            }
        }

        public new void Sort(int index, int count, IComparer<T> comparer)
        {
            lock (this)
            {
                base.Sort(index, count, comparer);
            }
        }

        public new void TrimExcess()
        {
            lock (this)
            {
                base.TrimExcess();
            }
        }
    }
}