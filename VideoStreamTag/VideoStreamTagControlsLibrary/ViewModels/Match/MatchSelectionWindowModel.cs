using ControlsLibrary.VirtualizingCollection;

namespace VideoStreamTagControlsLibrary.ViewModels.Match
{
    public class MatchSelectionWindowModel<T> where T : IVirtualItem
    {
        private AsyncVirtualizingCollection<T> _autoComboSource;

        public AsyncVirtualizingCollection<T> AutoComboSource
        {
            get => _autoComboSource;

            set
            {
                if (value != null)
                {
                    _autoComboSource = value;
                }
                else
                {
                    _autoComboSource?.Clear();
                }
            }
        }
    }
}