using System.Windows;
using System.Windows.Controls;

namespace ControlsLibrary.Controls
{
    public class WaitInidicator : Control
    {
        static WaitInidicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitInidicator), new FrameworkPropertyMetadata(typeof(WaitInidicator)));
        }

        public WaitInidicator()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style)FindResource(new ComponentResourceKey(typeof(WaitInidicator), "KWaitIndicator1Style"));
        }
    }
}