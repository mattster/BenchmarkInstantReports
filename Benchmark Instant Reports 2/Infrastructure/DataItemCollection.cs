﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchmark_Instant_Reports_2.Infrastructure
{
    /// <summary>
    /// base class for my classes that will represent a collection of
    /// in-memory data objects;
    /// wrapping the list in this class for simplicity and usability in code
    /// </summary>
    /// <typeparam name="T">a class that represents an item to be used in this collection</typeparam>
    public class DataItemCollection<T> where T : class
    {
        private List<T> _items;

        public int Count { get { return _items.Count; } }


        public DataItemCollection()
        {
            _items = new List<T>();
        }


        public DataItemCollection(List<T> items)
        {
            _items = items;
        }


        public DataItemCollection(IEnumerable<T> items)
        {
            _items = items.ToList<T>();
        }


        public T Idx(int i)
        {
            try
            {
                return _items[i];
            }
            catch
            {
                return null;
            }
        }


        public T First()
        {
            try
            {
                return _items[0];
            }
            catch
            {
                return null;
            }
        }


        public void Sort(Func<T, object> predicate)
        {
            _items.OrderBy(predicate);
        }


        public void Add(T item)
        {
            _items.Add(item);
        }


        public void Add(List<T> items)
        {
            foreach (T item in items)
            {
                this.Add(item);
            }
        }


        public void Remove(T item)
        {
            _items.Remove(item);
        }


        public T GetItemWhere(Func<T, bool> predicate)
        {
            return _items.Where(predicate).FirstOrDefault();
        }


        public IEnumerable<T> GetItems()
        {
            return _items;
        }


        public IEnumerable<T> GetItems<TKey>(Func<T, TKey> keyselector)
        {
            return _items.OrderBy(keyselector);
        }


        public IEnumerable<T> GetItemsWhere(Func<T, bool> predicate)
        {
            return _items.Where(predicate);
        }


        public IEnumerable<T> GetItemsWhere<TKey>(Func<T, bool> predicate, Func<T, TKey> keyselector)
        {
            return GetItemsWhere(predicate).OrderBy(keyselector);
        }

        public void UpdateItemAtIndexWith(int index, T newitem)
        {
            _items[index] = newitem;
        }
    }
}