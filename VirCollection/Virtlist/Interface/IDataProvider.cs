using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtlistLib.Interface
{
    public interface IDataProvider<T> where T : IVirtualListItem
    {
        Task InitAsync();
        int Count { get; }
        Task<IList<IVirtualListItem>> GetPageAsync(int index, int count);
        Task<bool> ContainsAsync(string keyword);
    }
}