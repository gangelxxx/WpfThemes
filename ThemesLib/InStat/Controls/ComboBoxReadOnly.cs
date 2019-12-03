using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ThemesLib.InStat.Controls
{
    public class ComboBoxReadOnly : ComboBox
    {
        public static readonly DependencyProperty IsShowTipInBoxProperty =
            DependencyProperty.Register(nameof(IsShowTipInBox), typeof(bool), typeof(ComboBoxReadOnly),
                new FrameworkPropertyMetadata(false));

        /// <summary>
        /// 
        /// </summary>
        public bool IsShowTipInBox
        {
            get => (bool)GetValue(IsShowTipInBoxProperty);
            set => SetValue(IsShowTipInBoxProperty, value);
        }

        private Border _border;

        static ComboBoxReadOnly()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxReadOnly), new FrameworkPropertyMetadata(typeof(ComboBoxReadOnly)));
        }

        public ComboBoxReadOnly()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style) FindResource(new ComponentResourceKey(typeof(ComboBoxReadOnly), "KComboBoxReadOnly"));

            if (SelectedIndex < 0 && IsShowTipInBox)
                ShowPART_TipInBox();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
            base.OnMouseUp(e);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (SelectedIndex >= 0 && IsShowTipInBox)
                HidePART_TipInBox();
        }

        private async void ShowPART_TipInBox()
        {
            await Dispatcher.InvokeAsync(() => { VisualStateManager.GoToState(this, "ShowTipInBox", false); }, DispatcherPriority.Send);
        }

        private async void HidePART_TipInBox()
        {
            await Dispatcher.InvokeAsync(() => { VisualStateManager.GoToState(this, "HideTipInBox", false); }, DispatcherPriority.Send);
        }
    }
}