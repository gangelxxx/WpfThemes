using System.Windows;
using ControlsLibrary.Windows;

namespace VideoStreamTagControlsLibrary.Controls
{
    public class LoginWindowControl : WindowTypeOne
    {
        public LoginWindowControl()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}