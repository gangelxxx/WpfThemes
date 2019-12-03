namespace ControlsLibrary.VirtualList.Interface
{
    public class VirtualListItem
    {
        public VirtualListItem(int idx, string leftText, object tag = null, string colorText = "", string rightText = "")
        {
            Idx = idx;
            LeftText = leftText;
            Tag = tag;
            ColorText = colorText;
            RightText = rightText;
        }

        public int Idx { get; }
        public string LeftText { get; private set; }
        public string ColorText { get; private set; }
        public string RightText { get; private set; }
        public object Tag { get; private set; }

        public override string ToString()
        {
            return LeftText + ColorText + RightText;
        }

        public void UpdateColorText(string leftText, string colorText, string rightText)
        {
            LeftText = leftText;
            ColorText = colorText;
            RightText = rightText;
        }
    }
}