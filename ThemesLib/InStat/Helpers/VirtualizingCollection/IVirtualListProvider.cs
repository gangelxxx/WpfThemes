using System;
using System.Collections.Generic;
using ThemesLib.InStat.Providers;

namespace ThemesLib.InStat.Helpers.VirtualizingCollection
{
    public interface IVirtualListProvider<T> where T : IVirtualItem
    {
        List<T> MainList { get; }

        IList<T> FetchRange(int startIndex, int count);
        int FetchCount();
        void Clear();
        void Refresh(string searchLine);
        void CreateMainList();
    }
}