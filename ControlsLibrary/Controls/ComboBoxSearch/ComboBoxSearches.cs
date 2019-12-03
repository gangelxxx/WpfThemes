using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ControlsLibrary.Helpers;
using ControlsLibrary.VirtualList.Interface;
using VirtlistLib.Interface;

namespace ControlsLibrary.Controls.ComboBoxSearch
{
    public class ComboBoxSearches : Control
    {
        private ComboBox _comboBox;

        public static readonly DependencyProperty VirtListSourceProperty =
            DependencyProperty.Register(nameof(VirtListSource), typeof(IVirtualList<VirtualListItem>), typeof(ComboBoxSearches),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, PropertyChangedCallback));

        private ComboTextBox _comboTextBox;

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBoxSearches asyncComboBox && asyncComboBox._comboBox != null && e.NewValue is IVirtualList<VirtualListItem> virtualList)
            {
                asyncComboBox._comboBox.ItemsSource = virtualList;
            }
        }

        public IVirtualList<VirtualListItem> VirtListSource
        {
            get { return (IVirtualList<VirtualListItem>) GetValue(VirtListSourceProperty); }
            set { SetValue(VirtListSourceProperty, value); }
        }

        static ComboBoxSearches()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxSearches), new FrameworkPropertyMetadata(typeof(ComboBoxSearches)));
        }

        public ComboBoxSearches()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
              Style = (Style)FindResource(new ComponentResourceKey(typeof(ComboBoxSearches), "KAsyncComboBoxStyle"));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            GetParts();

            InitAsync();
        }

        private async void InitAsync()
        {
            WaitState();

            await VirtListSource.DataProvider.InitAsync();
            await VirtListSource.LoadFirstPage();

            _comboBox.IsTextSearchEnabled = false;
            _comboBox.MaxDropDownHeight = 160;
            _comboBox.ItemsSource = VirtListSource;
            _comboBox.Focusable = false;
            _comboBox.SelectionChanged += ComboBoxOnSelectionChanged;
            _comboBox.DropDownOpened += ComboBoxOnDropDownOpened;
            _comboBox.DropDownClosed += ComboBoxOnDropDownClosed;

            _comboTextBox.StartTypeEvent += ComboTextBoxOnStartTypeEvent;
            _comboTextBox.EndTypeEvent += ComboTextBoxOnEndTypeEvent;
            _comboTextBox.SelectNextEvent += ComboTextBoxOnSelectNextEvent;
            _comboTextBox.SelectBackEvent += ComboTextBoxOnSelectBackEvent;

            NormalState();
        }

        private void ComboBoxOnDropDownClosed(object sender, EventArgs e)
        {
            DropDownCloseBorderState();
        }

        private void ComboBoxOnDropDownOpened(object sender, EventArgs e)
        {
            DropDownOpenBorderState();
        }

        private void ComboTextBoxOnSelectBackEvent(object sender, EventArgs e)
        {
            if(_comboBox.SelectedIndex > -1)
                _comboBox.SelectedIndex = _comboBox.SelectedIndex - 1;
        }

        private void ComboTextBoxOnSelectNextEvent(object sender, EventArgs e)
        {
            if (!_comboBox.IsDropDownOpen)
            {
                _comboBox.IsDropDownOpen = true;
            }

            if (_comboBox.SelectedIndex < VirtListSource.DataProvider.Count)
            {
                _comboBox.SelectedIndex = _comboBox.SelectedIndex + 1;
            }
        }

        private void ComboBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_comboBox.SelectedItem != null)
            {
                _comboTextBox.Text = _comboBox.SelectedItem.ToString();
                _comboTextBox.CaretToEnd();
            }
        }

        private void ComboTextBoxOnStartTypeEvent(object sender, EventArgs e)
        {
            WaitState();
        }

        private async void ComboTextBoxOnEndTypeEvent(object sender, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                _comboBox.IsDropDownOpen = false;
            }
            else
            {
                var res = await VirtListSource.DataProvider.ContainsAsync(text);
                await VirtListSource.Update();

                if (res)
                {
                    _comboBox.IsDropDownOpen = true;
                    _comboBox.SelectedItem = null;
                }
            }

            NormalState();
        }

        private void WaitState()
        {
            VisualStateManager.GoToState(_comboBox, "WaitInidicatorShowState", false);
//            Dispatcher.Invoke(() => { VisualStateManager.GoToState(_comboBox, "WaitInidicatorShowState", false); }, DispatcherPriority.Send);
        }

        private void NormalState()
        {
            VisualStateManager.GoToState(_comboBox, "WaitInidicatorHideState", false);
//            Dispatcher.Invoke(() => { VisualStateManager.GoToState(_comboBox, "WaitInidicatorHideState", false); }, DispatcherPriority.Send);
        }

        private void DropDownOpenBorderState()
        {
            VisualStateManager.GoToState(_comboBox, "DropDownOpenBorderState", false);
        }

        private void DropDownCloseBorderState()
        {
            VisualStateManager.GoToState(_comboBox, "DropDownCloseBorderState", false);
        }

        private void GetParts()
        {
            _comboBox = this.FindNameEx<ComboBox>("PART_ComboBox");
            _comboBox.ApplyTemplate();

            _comboTextBox = _comboBox.FindNameEx<ComboTextBox>("PART_MyEditableTextBox");
        }

    }
}