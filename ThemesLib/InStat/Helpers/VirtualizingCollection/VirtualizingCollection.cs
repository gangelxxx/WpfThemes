using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

namespace ThemesLib.InStat.Helpers.VirtualizingCollection
{
    public abstract class VirtualizingCollection<T> : IList<T>, IList where T : IVirtualItem
    {
        #region Private fields

        private readonly IVirtualListProvider<T> _virtualListProvider;

        private readonly Dictionary<int, IList<T>> _pages = new Dictionary<int, IList<T>>();

        private readonly int _pageSize = 100;

        private readonly long _pageTimeout = 10000;
        private readonly Dictionary<int, DateTime> _pageTouchTimes = new Dictionary<int, DateTime>();

        private int _count = -1;

        #endregion

        #region Construct

        public VirtualizingCollection(IVirtualListProvider<T> virtualListProvider, int pageSize, int pageTimeout)
        {
            _virtualListProvider = virtualListProvider;

            if (pageSize > 0)
            {
                _pageSize = pageSize;
            }

            if (pageTimeout > 0)
            {
                _pageTimeout = pageTimeout;
            }
        }

        #endregion

        #region Public

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            return false;
        }

        public void Clear()
        {
            _pages.Clear();
            _pageTouchTimes.Clear();
            _virtualListProvider.Clear();
            LoadCount();
        }


        public int IndexOf(T item)
        {
            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public void Refresh(string searchLine)
        {
            _pages.Clear();
            _pageTouchTimes.Clear();
            _virtualListProvider.Refresh(searchLine);

            LoadCount();
        }

        public void CleanUpPages()
        {
            List<int> keys = new List<int>(_pageTouchTimes.Keys);
            foreach (int key in keys)
            {
                if (key != 0 && (DateTime.Now - _pageTouchTimes[key]).TotalMilliseconds > PageTimeout)
                {
                    _pages.Remove(key);
                    _pageTouchTimes.Remove(key);
                }
            }
        }


        public IVirtualListProvider<T> VirtualListProvider
        {
            get { return _virtualListProvider; }
        }


        public int PageSize
        {
            get { return _pageSize; }
        }

        public long PageTimeout
        {
            get { return _pageTimeout; }
        }

        public virtual int Count
        {
            get
            {
                if (_count == -1)
                {
                    LoadCount();
                }

                return _count;
            }
            protected set { _count = value; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        #endregion

        #region Private methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        bool IList.Contains(object value)
        {
            return Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Protected Методы

        protected virtual void PopulatePage(int pageIndex, IList<T> page)
        {
            if (_pages.ContainsKey(pageIndex))
                _pages[pageIndex] = page;
        }

        protected virtual void RequestPage(int pageIndex)
        {
            if (!_pages.ContainsKey(pageIndex) && !_pageTouchTimes.ContainsKey(pageIndex))
            {
                _pages.Add(pageIndex, null);
                _pageTouchTimes.Add(pageIndex, DateTime.Now);
                LoadPage(pageIndex);
            }
            else
            {
                _pageTouchTimes[pageIndex] = DateTime.Now;
            }
        }

        protected virtual void LoadCount()
        {
            Count = FetchCount();
        }

        protected virtual void LoadPage(int pageIndex)
        {
            PopulatePage(pageIndex, FetchPage(pageIndex));
        }

        protected IList<T> FetchPage(int pageIndex)
        {
            return VirtualListProvider.FetchRange(pageIndex * PageSize, PageSize);
        }

        protected int FetchCount()
        {
            return VirtualListProvider.FetchCount();
        }

        #endregion

        #region Other members

        public event EventHandler LoadCompleted;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public T this[int index]
        {
            get
            {
                int pageIndex = index / PageSize;
                int pageOffset = index % PageSize;

                RequestPage(pageIndex);

                if (pageOffset > PageSize / 2 && pageIndex < Count / PageSize)
                    RequestPage(pageIndex + 1);

                if (pageOffset < PageSize / 2 && pageIndex > 0)
                    RequestPage(pageIndex - 1);

                CleanUpPages();

                if (_pages.Count > 0)
                {
                    if (!_pages.ContainsKey(pageIndex))
                        return default(T);

                    if (_pages[pageIndex] == null)
                        return default(T);

                    if (_pages[pageIndex].Count > pageOffset)
                    {
                        return _pages[pageIndex][pageOffset];
                    }
                }

                return default(T);
            }

            set { throw new NotSupportedException(); }
        }

       

        object IList.this[int index]
        {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }

        #endregion


        protected virtual void OnLoadCompleted()
        {
            LoadCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
    }
}