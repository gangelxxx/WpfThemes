using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace VirtlistLib.Interface
{
    public interface IVirtualList<T> : IList<T>, IList, INotifyCollectionChanged, INotifyPropertyChanged where T : IVirtualListItem
    {
        IDataProvider<T> DataProvider { get; }
        void Update();
        void LoadFirstPage();
    }
}