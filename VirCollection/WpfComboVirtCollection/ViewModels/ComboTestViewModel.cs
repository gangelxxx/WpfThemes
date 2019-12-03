using System.Collections.ObjectModel;
using ControlsLibrary.VirtualList.Interface;
using VirtlistLib;
using VirtlistLib.Interface;
using WpfComboVirtCollection.Data;
using WpfComboVirtCollection.DataProviders;

namespace WpfComboVirtCollection.ViewModels
{
    public class ComboTestViewModel
    {
        public ComboTestViewModel()
        {
            var provider = new TestDataProvider();
            Users = new VirtualList<VirtualListItem>(provider, 50);

//            Users = new ObservableCollection<string>();
//
//            for (int i = 0; i < 1000000; i++)
//            {
//                Users.Add(i.ToString());
//            }
        }

        public VirtualList<VirtualListItem> Users { get; }
//        public ObservableCollection<string> Users { get; }
    }
}