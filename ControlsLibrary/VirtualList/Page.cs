#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlsLibrary.VirtualList.Interface;
using VirtlistLib.Helpers;
using VirtlistLib.Interface;

#endregion

namespace VirtlistLib
{
    public class Page<T> : IDisposable where T : VirtualListItem
    {
        #region Private fields

        private readonly int _size;
        private readonly IDataProvider<T> _dataProvider;
        private readonly int _index;
        public PageStateEnum State { get; private set; } = PageStateEnum.Clean;
        public DateTime TouchTime { get; set; }

        #endregion

        #region Construct

        public Page(int index, int size, IDataProvider<T> dataProvider)
        {
            _index = index;
            _size = size;

            _dataProvider = dataProvider;

            Touch();
        }

        #endregion

        #region Public

        public void Touch()
        {
            TouchTime = DateTime.Now;
        }

        public async Task Load()
        {
            if (Items == null)
            {
                State = PageStateEnum.Loading;

                var find = await _dataProvider.GetPageAsync(_index * _size, _size);
                if (find != null)
                {
                    Items = find.Select(x => (T)x).ToList();
                }
                
                State = PageStateEnum.Loaded;
            }
        }

        public void Clear()
        {
            Items?.Clear();
            Items = null;
            State = PageStateEnum.Clean;
        }

        public int Count => Items?.Count ?? 0;

        public T GetItem(int index)
        {
            if (Items == null)
                return default(T);

            var i = index % _size;

            return i < Items.Count ? Items[i] : default(T);
        }

        public override string ToString()
        {
            return "page:" + Index;
        }

        public int Index => _index;
        public IList<T> Items { get; private set; }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Clear();
        }

        #endregion
    }
}