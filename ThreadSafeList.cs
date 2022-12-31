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
        
        public void AddRange(IEnumerable<T> collection)
        {
            lock (this)
            {
                base.AddRange(collection);
            }
        }
        
        public void AddRange(params T[] collection)
        {
            lock (this)
            {
                base.AddRange(collection);
            }
        }
        
        public void Add(T item)
        {
            lock (this)
            {
                base.Add(item);
            }
        }
        


        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (this)
            {
                base.CopyTo(array, arrayIndex);
            }
        }
        
        public bool Remove(T item)
        {
            lock (this)
            {
                return base.Remove(item);
            }
        }
        
        public int RemoveAll(Predicate<T> match)
        {
            lock (this)
            {
                return base.RemoveAll(match);
            }
        }
        
        public void RemoveAt(int index)
        {
            lock (this)
            {
                base.RemoveAt(index);
            }
        }
        
        public void RemoveRange(int index, int count)
        {
            lock (this)
            {
                base.RemoveRange(index, count);
            }
        }
        
        public void Insert(int index, T item)
        {
            lock (this)
            {
                base.Insert(index, item);
            }
        }
        
        public void InsertRange(int index, IEnumerable<T> collection)
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
        
        public void Reverse()
        {
            lock (this)
            {
                base.Reverse();
            }
        }
        
        public void Reverse(int index, int count)
        {
            lock (this)
            {
                base.Reverse(index, count);
            }
        }
        
        public void Sort()
        {
            lock (this)
            {
                base.Sort();
            }
        }
        
        public void Sort(Comparison<T> comparison)
        {
            lock (this)
            {
                base.Sort(comparison);
            }
        }
        
        public void Sort(IComparer<T> comparer)
        {
            lock (this)
            {
                base.Sort(comparer);
            }
        }
        
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            lock (this)
            {
                base.Sort(index, count, comparer);
            }
        }
        
        public void TrimExcess()
        {
            lock (this)
            {
                base.TrimExcess();
            }
        }
        

            
    }
}