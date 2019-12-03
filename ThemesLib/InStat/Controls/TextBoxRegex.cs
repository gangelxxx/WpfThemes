#region

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

#endregion

namespace ThemesLib.InStat.Controls
{
    public class TextBoxRegex : TextBox
    {
        #region Construct

        static TextBoxRegex()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxRegex), new FrameworkPropertyMetadata(typeof(TextBoxRegex)));
        }

        public TextBoxRegex()
        {
            Loaded += OnLoaded;
        }

        #endregion

        #region Static

        public static readonly DependencyProperty IsShowTipInBoxProperty =
        DependencyProperty.Register(nameof(IsShowTipInBox), typeof(bool), typeof(TextBoxRegex),
        new PropertyMetadata(false));

        public static readonly DependencyProperty PatternProperty =
            DependencyProperty.Register(nameof(Pattern), typeof(string), typeof(TextBoxRegex),
               new PropertyMetadata(""));

        #endregion

        #region Private methods

        private string _id = Guid.NewGuid().ToString();
        private string _text;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style)FindResource(new ComponentResourceKey(typeof(TextBoxRegex), "TextBoxRegexStyle"));

            // Show the ToolTip text in TextBox
            ShowTipInBox();
        }

        private async void ShowTipInBox()
        {
            if (string.IsNullOrEmpty(_text) && IsShowTipInBox && !IsFocused)
                await Dispatcher.InvokeAsync(() => { VisualStateManager.GoToState(this, "ShowTipInBox", false); }, DispatcherPriority.Send);
        }

        private async void HideTipInBox()
        {
            if (string.IsNullOrEmpty(_text) && IsShowTipInBox)
                await Dispatcher.InvokeAsync(() => { VisualStateManager.GoToState(this, "HideTipInBox", false); }, DispatcherPriority.Send);
        }

        #endregion

        #region Protected Методы

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            HideTipInBox();
            _text = Text;
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
         {
             var reg = new Regex(Pattern);
             if (!reg.IsMatch(e.Text))
             {
                 e.Handled = true;
                 return;
             }

             base.OnTextInput(e);
         }
        
         protected override void OnGotFocus(RoutedEventArgs e)
         {
             base.OnGotFocus(e);
             HideTipInBox();
        }
        
         protected override void OnLostFocus(RoutedEventArgs e)
         {
             base.OnLostFocus(e);
             ShowTipInBox();
        }

        #endregion

        #region Public

        public bool IsShowTipInBox
        {
            get => (bool)GetValue(IsShowTipInBoxProperty);
            set => SetValue(IsShowTipInBoxProperty, value);
        }
        
        public string Pattern
        {
            get => (string)GetValue(PatternProperty);
            set => SetValue(PatternProperty, value);
        }

        #endregion
    }
}