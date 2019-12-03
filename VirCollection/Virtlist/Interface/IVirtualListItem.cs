namespace VirtlistLib.Interface
{
    public interface IVirtualListItem
    {
        int Idx { get; }
        string LeftText { get; }
        string ColorText { get; }
        string RightText { get; }
        string ToString();

        void UpdateColorText(string leftText, string colorText, string rightText);
    }
}