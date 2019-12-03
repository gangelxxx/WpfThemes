using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ControlsLibrary.Helpers;
using ControlsLibrary.VirtualList.Interface;
using VirtlistLib.Interface;

namespace ControlsLibrary.Controls.ComboBoxSearch
{
    public class TextBoxSearches : Control
    {
        private ComboBox _comboBox;

        public static readonly DependencyProperty VirtListSourceProperty =
            DependencyProperty.Register(nameof(VirtListSource), typeof(IVirtualList<VirtualListItem>), typeof(TextBoxSearches),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, VirtListSourcePropertyChangedCallback));

        private static void VirtListSourcePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBoxSearches textBoxSearches && textBoxSearches._comboBox != null && e.NewValue is IVirtualList<VirtualListItem> virtualList)
            {
                textBoxSearches._comboBox.ItemsSource = virtualList;
                textBoxSearches.InitDataAsync();
            }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextBoxSearches),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, TextChangedCallback));

        private static async void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBoxSearches textBoxSearches && textBoxSearches._comboTextBox != null && e.NewValue is string text && textBoxSearches._comboTextBox.Text != text)
            {
                textBoxSearches._comboTextBox.Text = text;

                var res = await textBoxSearches.VirtListSource.DataProvider.ContainsAsync(text);
                await textBoxSearches.VirtListSource.Update();

                if (res)
                {
                    textBoxSearches._comboBox.SelectedIndex = 0;
                    textBoxSearches.UpdateSelectedItem();
                }
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(TextBoxSearches),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private static void SelectedItemPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBoxSearches textBoxSearches && textBoxSearches._comboBox != null && e.NewValue != null && textBoxSearches._comboBox.SelectedItem != e.NewValue)
            {
                textBoxSearches._comboBox.SelectedItem = e.NewValue;
            }
        }

        public object SelectedItem
        {
            get => this.GetValue(SelectedItemProperty);
            set => this.SetValue(SelectedItemProperty, value);
        }

        private void UpdateSelectedItem()
        {
            if (_comboBox.SelectedItem != SelectedItem)
            {
                SelectedItem = _comboBox.SelectedItem;
            }
        }

        private ComboTextBox _comboTextBox;
        private readonly ConcurrentStack<string> _tempTextStack = new ConcurrentStack<string>();

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public IVirtualList<VirtualListItem> VirtListSource
        {
            get => (IVirtualList<VirtualListItem>) GetValue(VirtListSourceProperty);
            set => SetValue(VirtListSourceProperty, value);
        }

        static TextBoxSearches()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxSearches), new FrameworkPropertyMetadata(typeof(TextBoxSearches)));
        }

        public event EventHandler EndInitEvent;

        public TextBoxSearches()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
              Style = (Style)FindResource(new ComponentResourceKey(typeof(TextBoxSearches), "KTextBoxSearchesStyle"));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            GetParts();

            _comboBox.IsTextSearchEnabled = false;
            _comboBox.MaxDropDownHeight = 160;
            _comboBox.ItemsSource = VirtListSource;
            _comboBox.Focusable = false;
            _comboBox.DropDownOpened += ComboBoxOnDropDownOpened;
            _comboBox.DropDownClosed += ComboBoxOnDropDownClosed;

            _comboTextBox.StartTypeEvent += ComboTextBoxOnStartTypeEvent;
            _comboTextBox.EndTypeEvent += ComboTextBoxOnEndTypeEvent;
            _comboTextBox.SelectNextEvent += ComboTextBoxOnSelectNextEvent;
            _comboTextBox.SelectBackEvent += ComboTextBoxOnSelectBackEvent;

            InitDataAsync();
        }

        private async void InitDataAsync()
        {
            if (VirtListSource == null) return;

            WaitState();

            await VirtListSource.DataProvider.InitAsync();
            await VirtListSource.LoadFirstPage();

            VirtListSource.OwnerUpdateAction = OwnerUpdateAction;

            NormalState();
            OnEndInitEvent();
        }

        private async void OwnerUpdateAction()
        {
            if (string.IsNullOrEmpty(_comboTextBox.Text)) return;

            WaitState();

            var res = await VirtListSource.DataProvider.ContainsAsync(_comboTextBox.Text);
            await VirtListSource.Update();

            _comboBox.IsDropDownOpen = res;
            _comboBox.SelectedItem = null;

            NormalState();
        }

        private void ComboBoxOnDropDownClosed(object sender, EventArgs e)
        {
            DropDownCloseBorderState();

            if (_comboBox.SelectedItem != null)
            {
                var res = _comboBox.SelectedItem.ToString();

                _comboTextBox.Text = res;
                _comboTextBox.CaretToEnd();

                Text = res;
                UpdateSelectedItem();
            }
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

        private void ComboTextBoxOnStartTypeEvent(object sender, EventArgs e)
        {
           
        }

        private bool _isLoading = false;
        private string _tempText = "";

        private void ComboTextBoxOnEndTypeEvent(object sender, string text)
        {
            _tempText = text;

            if (_isLoading)
            {
                return;
            }
            
            WaitState();

            SearchText(text);
        }

        private async void SearchText(string text)
        {
            var res = await VirtListSource.DataProvider.ContainsAsync(text);

            if (string.IsNullOrEmpty(_tempText) || _tempText.Length < 3)
            {
                res = false;
            }
            else if (text != _tempText)
            {
                SearchText(_tempText);
                return;
            }

            if (res)
            {
                await VirtListSource.Update();
            }

            _comboBox.IsDropDownOpen = res;
            _comboBox.SelectedItem = null;

            NormalState();
        }

        private void WaitState()
        {
            _isLoading = true;
            VisualStateManager.GoToState(_comboBox, "WaitInidicatorShowState", false);
        }

        private void NormalState()
        {
            _isLoading = false;
            VisualStateManager.GoToState(_comboBox, "WaitInidicatorHideState", false);
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

        protected virtual void OnEndInitEvent()
        {
            EndInitEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}