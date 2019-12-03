using System.Collections.Generic;
using System.Threading.Tasks;
using ControlsLibrary.VirtualList.Interface;
using VirtlistLib.Interface;

namespace WpfComboVirtCollection.DataProviders
{
    public class AbstractProvider<T> : IDataProvider<T> where T: VirtualListItem
    {
        public Task InitAsync()
        {
            throw new System.NotImplementedException();
        }

        public int Count { get; }
        public Task<IList<VirtualListItem>> GetPageAsync(int index, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ContainsAsync(string msg)
        {
            throw new System.NotImplementedException();
        }

       
    }
} 