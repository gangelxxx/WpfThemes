using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ControlsLibrary.Controls
{
    public class IntUpDown : TextBox
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(int), typeof(IntUpDown), new PropertyMetadata(default(int), ValuePropertyChanged));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static IntUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntUpDown), new FrameworkPropertyMetadata(typeof(IntUpDown)));
        }

        public IntUpDown()
        {
            Loaded += OnLoaded;
            TextChanged += OnTextChanged;
            PreviewTextInput += OnPreviewTextInput;
        }

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TextBox)d).Text = e.NewValue.ToString();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Array.TrueForAll(e.Text.ToCharArray(), x => x >= '0' && x <= '9');
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(Text, out var value))
            {
                Value = value;
            }
            else
            {
                Value = 0;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style)FindResource("KIntUpDownStyle");
        }

        public void Increase()
        {
            ++Value;
        }

        public void Decrease()
        {
            if (Value > 0)
                Value--;
        }
    }

    public class IntUpDownNegativeDefault : TextBox
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(int), typeof(IntUpDownNegativeDefault), new PropertyMetadata(-1, ValuePropertyChanged));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        static IntUpDownNegativeDefault()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntUpDownNegativeDefault), new FrameworkPropertyMetadata(typeof(IntUpDownNegativeDefault)));
        }

        public IntUpDownNegativeDefault()
        {
            Loaded += OnLoaded;
            TextChanged += OnTextChanged;
            PreviewTextInput += OnPreviewTextInput;
        }

        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TextBox)d).Text = e.NewValue.ToString();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Array.TrueForAll(e.Text.ToCharArray(), x => x >= '0' && x <= '9');
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(Text, out var value))
            {
                Value = value;
            }
            else
            {
                Value = 0;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style)FindResource("KIntUpDownNegativeDefaultStyle");
        }

        public void Increase()
        {
            ++Value;
        }

        public void Decrease()
        {
            if (Value > 0)
                Value--;
        }
    }
}