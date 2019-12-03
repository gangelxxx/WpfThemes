using ControlsLibrary.VirtualList.Interface;
using VirtlistLib.Interface;

namespace VideoStreamTagControlsLibrary.Providers.LoginWindow
{
    public class User : VirtualListItem
    {
        public User(int idx, string leftText, object tag = null, string colorText = "", string rightText = "") : base(idx, leftText, tag, colorText, rightText)
        { }
    }
}