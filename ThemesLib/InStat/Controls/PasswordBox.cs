using System;
using System.Windows;
using System.Windows.Controls;
using ThemesLib.InStat.Helpers;

namespace ThemesLib.InStat.Controls
{
    public class PasswordBox : Control
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(PasswordBox),
                new PropertyMetadata(""));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #region Private fields

        private ActiveControl _activeControl;

        private CheckBox _pasPasswordBoxButton;
        private TextBox _passwordBoxTextBox;
        private System.Windows.Controls.PasswordBox _passwordBoxWin;

        #endregion

        #region Construct

        static PasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(typeof(PasswordBox)));
        }

        public PasswordBox()
        {
            Loaded += OnLoaded;
        }

        #endregion

        #region Private methods

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            //            KPasswordBoxStyle
            Style = (Style)FindResource(new ComponentResourceKey(typeof(PasswordBox), "KPasswordBoxStyle"));

            if (Visibility == Visibility.Collapsed)
                throw new Exception("Visibility is \"Collapsed\" the style not will be loaded");

             _activeControl = ActiveControl.PasswordBox;
             _passwordBoxTextBox = this.FindChildByName<TextBox>("NPasswordBoxViewedText");
             _passwordBoxWin = this.FindChildByName<System.Windows.Controls.PasswordBox>("NPasswordBoxWin");
             _pasPasswordBoxButton = this.FindChildByName<CheckBox>("NPasswordBoxButton");
            
             if (_passwordBoxTextBox == null)
                 throw new NullReferenceException("Could not find NPasswordBoxViewedText");
             if (_passwordBoxWin == null)
                 throw new NullReferenceException("Could not find NPasswordBoxWin");
             if (_pasPasswordBoxButton == null)
                 throw new NullReferenceException("Could not find NPasswordBoxButton");
            
             _pasPasswordBoxButton.Click += BtnOnClick;
             _passwordBoxTextBox.TextChanged += PasswordBoxTextBoxOnTextChanged;
             _passwordBoxWin.PasswordChanged += PasswordBoxWinOnPasswordChanged;
        }

        private void PasswordBoxWinOnPasswordChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            Text = (sender as System.Windows.Controls.PasswordBox)?.Password;
        }

        private void PasswordBoxTextBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            Text = (sender as System.Windows.Controls.TextBox)?.Text;
        }

        private void BtnOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_activeControl == ActiveControl.PasswordBox)
            {
                _activeControl = ActiveControl.TextBox;
                _passwordBoxTextBox.Text = _passwordBoxWin.Password;
                _passwordBoxWin.Visibility = Visibility.Collapsed;
                _passwordBoxTextBox.Visibility = Visibility.Visible;
            }
            else if (_activeControl == ActiveControl.TextBox)
            {
                _activeControl = ActiveControl.PasswordBox;
                _passwordBoxWin.Password = _passwordBoxTextBox.Text;
                _passwordBoxTextBox.Visibility = Visibility.Collapsed;
                _passwordBoxWin.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region Other members

        private enum ActiveControl
        {
            PasswordBox,
            TextBox
        }

        #endregion
    }
}