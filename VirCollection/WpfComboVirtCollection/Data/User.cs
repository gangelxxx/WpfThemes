
using ControlsLibrary.VirtualList.Interface;
using VirtlistLib;
using VirtlistLib.Interface;

namespace WpfComboVirtCollection.Data
{
    public class User : VirtualListItem
    {
        public User(int idx, string leftText, object tag = null, string colorText = "", string rightText = "") : base(idx, leftText, tag, colorText, rightText)
        { }
    }
}