#region

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using VirtlistLib.Helpers;
using VirtlistLib.Interface;

#endregion

namespace VirtlistLib
{
    public class VirtualList<T> : IVirtualList<T> where T : IVirtualListItem
    {
        #region Events

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private fields

        private readonly ConcurrentDictionary<int, Page<T>> _pages = new ConcurrentDictionary<int, Page<T>>();
        private readonly int _pageSize;
        private readonly long _pageTimeout = 3000;
        private long _count;
        private readonly SynchronizationContext _synchronizationContext;

        #endregion

        #region Construct

        public VirtualList(IDataProvider<T> dataProvider, int pageSize)
        {
            _synchronizationContext = SynchronizationContext.Current;
            _pageSize = pageSize;
            _count = 0;

            DataProvider = dataProvider;
        }

        #endregion
            
        #region Public

        public IDataProvider<T> DataProvider { get; }

        public void Update()
        {
            _pages.Clear();
            Interlocked.Exchange(ref _count, 0);

            LoadFirstPage();
        }

        public async void LoadFirstPage()
        {
            if (_pages.IsEmpty)
                await AddPageAsync(0);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        public void Add(T item)
        {
            throw new System.NotImplementedException();
        }

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            return true;
        }

        public void Clear()
        {
            _pages.Clear();
        }

        public int IndexOf(object value)
        {
            return -1;
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        public virtual int Count
        {
            get => (int) _count;
            protected set => _count = value;
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

        private async Task AddPageAsync(int pageIndex)
        {
            var count = _count;

//            CleanUpPages();

            var page = new Page<T>(pageIndex, _pageSize, DataProvider);
            await page.Load();
            await Task.Delay(10);

            if (page.Count == 0)
            {
                return;
            }

            _pages.TryAdd(pageIndex, page);
            count += page.Count;

            Interlocked.Exchange(ref _count, count);
            _synchronizationContext.Send(Loaded, null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnCollectionReset()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void Loaded(object state)
        {
            OnCollectionReset();
        }

        #endregion

        #region Protected Методы

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        #endregion

        #region Other members

        public T this[int index]
        {
            get
            {
                int pageIndex = index / _pageSize;
                int pageOffset = index % _pageSize;

                RequestPage(pageIndex);

                if (pageOffset > _pageSize / 2 && pageIndex < Count / _pageSize)
                    RequestPage(pageIndex + 1);

                if (pageOffset < _pageSize / 2 && pageIndex > 0)
                    RequestPage(pageIndex - 1);

                if (_pages.Count > 0)
                {
                    if (!_pages.ContainsKey(pageIndex))
                        return default(T);

                    if (_pages[pageIndex].State == PageStateEnum.Loaded)
                    {
                        return _pages[pageIndex].GetItem(index);
                    }
                }

                return default(T);
            }

            set => throw new System.NotImplementedException();
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }

        #endregion

        private void CleanUpPages()
        {
            bool del = false;
            var keys = new List<int>(_pages.Keys);
            foreach (int key in keys)
            {
                if ((DateTime.Now - _pages[key].TouchTime).TotalMilliseconds > _pageTimeout)
                {
                    _pages.TryRemove(key, out var page);
                    page.Clear();
                    del = true;
                }
            }

            if (del)
                Debug.WriteLine(string.Join(",", _pages.Select(x => x.Value.Index.ToString()).ToArray()));
        }

        private async void RequestPage(int pageIndex)
        {
            if (!_pages.ContainsKey(pageIndex))
            {
                await AddPageAsync(pageIndex);
            }
            else
            {
                _pages[pageIndex].Touch();
            }
        }
    }
}