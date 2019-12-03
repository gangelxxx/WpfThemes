using System.Windows;
using System.Windows.Controls;
using ControlsLibrary.Helpers;

namespace ControlsLibrary.Windows
{
    public class WindowTypeOne : Window
    {
        private Button _buttonClose;

        static WindowTypeOne()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowTypeOne), new FrameworkPropertyMetadata(typeof(WindowTypeOne)));
        }

        public WindowTypeOne()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            WindowState = WindowState.Normal;
            ResizeMode = ResizeMode.NoResize;

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style)FindResource(new ComponentResourceKey(typeof(WindowTypeOne), "KWindowTypeOneStyle"));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            GetParts();

            _buttonClose.Click += ButtonCloseOnClick;
        }

        private void GetParts()
        {
            _buttonClose = this.FindNameEx<Button>("PART_ButtonClose");
        }

        private void ButtonCloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}