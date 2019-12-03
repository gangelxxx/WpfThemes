using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace VirCollectionConsole
{
    public class VirtList<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged where T : IVirtItem
    {
        private ConcurrentStack<Page<T>> _pageStack = new ConcurrentStack<Page<T>>();

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            _pageStack.Clear();
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

        public int Count { get; }
        public bool IsReadOnly { get; }
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

        private int PageSize = 100;

        public T this[int index]
        {
            get
            {
                int pageIndex = index / PageSize;
                int pageOffset = index % PageSize;

                var page = _pageStack.FirstOrDefault(x => x.Idx == pageIndex);

                if (page == null)
                {
                    page = new Page<T>();
                    _pageStack.Push(page);
                }

                var res = page.GetItem(pageOffset);

                return res;
            }
            set => throw new System.NotImplementedException();
        }

        private Page<T> CreatePage<T>(int pageIndex) where T : IVirtItem
        {
            throw new System.NotImplementedException();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}