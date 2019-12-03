using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using ControlsLibrary.VirtualList.Interface;

namespace VirtlistLib.Interface
{
    public interface IVirtualList<T> : IList<T>, IList, INotifyCollectionChanged, INotifyPropertyChanged where T : VirtualListItem
    {
        Action OwnerUpdateAction { get; set; }

        IDataProvider<T> DataProvider { get; }
        Task Update();
        Task LoadFirstPage();
    }
}