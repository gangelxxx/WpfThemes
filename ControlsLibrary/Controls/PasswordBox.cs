using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ControlsLibrary.Helpers;

namespace ControlsLibrary.Controls
{
    public class PasswordBox : Control
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(PasswordBox),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox && e.NewValue is string newValue && !string.IsNullOrEmpty(newValue) 
                && passwordBox._passwordBoxTextBox != null && passwordBox._passwordBoxWin != null
                && passwordBox._text != newValue)
            {
                passwordBox._passwordBoxTextBox.Text = newValue;
                passwordBox._passwordBoxWin.Password = newValue;
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty EyeBackgoundColorProperty =
            DependencyProperty.Register(nameof(EyeBackgoundColor), typeof(Brush), typeof(PasswordBox),
                new PropertyMetadata(null));

        public Brush EyeBackgoundColor
        {
            get { return (Brush)GetValue(EyeBackgoundColorProperty); }
            set { SetValue(EyeBackgoundColorProperty, value); }
        }

        #region Private fields

        private ActiveControl _activeControl;
        private string _text;
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

        public void SetFocus()
        {
            Dispatcher.Invoke(() =>
            {
                if (_activeControl == ActiveControl.TextBox)
                {
                    _passwordBoxTextBox.Focus();
                    Keyboard.Focus(_passwordBoxTextBox);
                }
                else
                {
                    _passwordBoxWin.Focus();
                    Keyboard.Focus(_passwordBoxWin);
                }
            }, DispatcherPriority.Send);
        }

        #endregion

        #region Private methods

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            //            KPasswordBoxStyle
            Style = (Style)FindResource(new ComponentResourceKey(typeof(PasswordBox), "KPasswordBoxStyle"));
        }

        public override void OnApplyTemplate()
        {
            GetParts();

            if (Visibility == Visibility.Collapsed)
                throw new Exception("Visibility is \"Collapsed\" the style not will be loaded");

            _passwordBoxTextBox.Text = Text;
            _passwordBoxWin.Password = Text;

            _pasPasswordBoxButton.Click += BtnOnClick;
            _passwordBoxTextBox.TextChanged += PasswordBoxTextBoxOnTextChanged;
            _passwordBoxWin.PasswordChanged += PasswordBoxWinOnPasswordChanged;
        }

        private void GetParts()
        {
            _activeControl = ActiveControl.PasswordBox;
            _passwordBoxTextBox = this.FindNameEx<TextBox>("NPasswordBoxViewedText");
            _passwordBoxWin = this.FindNameEx<System.Windows.Controls.PasswordBox>("NPasswordBoxWin");
            _pasPasswordBoxButton = this.FindNameEx<CheckBox>("NPasswordBoxButton");
        }

        private void PasswordBoxWinOnPasswordChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var text = (sender as System.Windows.Controls.PasswordBox)?.Password;
            UpdateText(text);
        }

        private void PasswordBoxTextBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var text = (sender as System.Windows.Controls.TextBox)?.Text;
            UpdateText(text);
        }

        private void UpdateText(string text)
        {
            if (!string.IsNullOrEmpty(text) && text != _text)
            {
                _text = text;
                Text = text;
            }
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