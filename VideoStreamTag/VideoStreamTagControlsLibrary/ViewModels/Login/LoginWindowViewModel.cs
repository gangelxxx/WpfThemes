using ControlsLibrary.VirtualList.Interface;
using VideoStreamTagControlsLibrary.Providers.LoginWindow;
using VirtlistLib;

namespace VideoStreamTagControlsLibrary.ViewModels.Login
{
    public class LoginWindowViewModel
    {
        public LoginWindowViewModel()
        {
            var provider = new TestDataProvider();
            Users = new VirtualList<VirtualListItem>(provider, 20);
        }

        public VirtualList<VirtualListItem> Users { get; }
    }
}