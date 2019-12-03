using System.Windows.Controls;

namespace ThemesLib.InStat.Controls
{
    public class ColorComboButtonTag
    {
        public readonly int _idxButton;
        public readonly int _idxStackPanel;
        public readonly Control _parent;

        public ColorComboButtonTag(int idxButton, int idxStackPanel, Control parent)
        {
            _idxButton = idxButton;
            _idxStackPanel = idxStackPanel;
            _parent = parent;
        }

        public int IdxButton => _idxButton;
        public int IdxStackPanel => _idxStackPanel;
        public Control Parent => _parent;
    }
}