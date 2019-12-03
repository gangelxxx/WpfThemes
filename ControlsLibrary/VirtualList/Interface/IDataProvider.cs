using System.Collections.Generic;
using System.Threading.Tasks;
using ControlsLibrary.VirtualList.Interface;

namespace VirtlistLib.Interface
{
    public interface IDataProvider<T> where T : VirtualListItem
    {
        Task InitAsync();
        int Count { get; }
        Task<IList<VirtualListItem>> GetPageAsync(int index, int count);
        Task<bool> ContainsAsync(string keyword);
    }
}