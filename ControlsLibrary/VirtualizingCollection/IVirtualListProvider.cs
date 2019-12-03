using System.Collections.Generic;

namespace ControlsLibrary.VirtualizingCollection
{
    public interface IVirtualListProvider<T> where T : IVirtualItem
    {
        IList<T> FetchRange(int startIndex, int count);
        int FetchCount();
        void Clear();
        void Refresh(string searchLine);
        void CreateMainList();
    }
}