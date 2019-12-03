#region

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using ThemesLib.InStat.Helpers;
using InputKey = System.Windows.Input.Key;

#endregion

namespace ThemesLib.InStat.Fitness.Controls
{
    public class KeyBind : TextBox
    {
        public event EventHandler<InputKey> KeyChanged;

        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register(nameof(Key), typeof(InputKey), typeof(KeyBind),
                new FrameworkPropertyMetadata(default(InputKey), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    KeyChangedCallback));

        private static void KeyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("KeyChangedCallback");
            if (d is KeyBind keyBind && e.NewValue is InputKey newValue)
            {
                keyBind.SetTextKey(newValue);
            }
        }

        private async void SetTextKey(Key key)
        {
            switch (key)
            {
                case Key.Enter:
                    break;
                case Key.F1:
                    Text = "F1";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.F2:
                    Text = "F2";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.F3:
                    Text = "F3";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.F4:
                    Text = "F4";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.F5:
                    Text = "F5";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.F6:
                    Text = "F6";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.F7:
                    Text = "F7";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.F8:
                    Text = "F9";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.F9:
                    Text = "F9";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.F10:
                    Text = "F10";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.F11:
                    Text = "F11";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.F12:
                    Text = "F12";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.Space:
                    Text = "Space";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.Back:
                    Text = "Back";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
                case Key.Delete:
                    Text = "Delete";
                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;

                default:
                    Text = Key.GetCharFromKey().ToString();

                    await Dispatcher.Yield();
                    OnKeyChanged(Key);
                    break;
            }
        }

        public InputKey Key
        {
            get => (InputKey) GetValue(KeyProperty);
            set => SetValue(KeyProperty, value);
        }

        public async void ShowSavedState()
        {
            Dispatcher.Invoke(() => { VisualStateManager.GoToState(this, "Saved", false); });
            await Task.Delay(400);
            Dispatcher.Invoke(() => { VisualStateManager.GoToState(this, "Normal", false); });
        }

        #region Construct

        static KeyBind()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyBind), new FrameworkPropertyMetadata(typeof(KeyBind)));
        }

        public KeyBind()
        {
            Key = Key.None;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style) FindResource("KKeyBindStyle");

            if (Key == InputKey.None)
            {
                Text = Key.None.ToString();
            }
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (Key == InputKey.None)
            {
                Text = "";
            }

            base.OnGotKeyboardFocus(e);
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (Key == InputKey.None)
            {
                Text = Key.None.ToString();
            }

            base.OnLostKeyboardFocus(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.System:
                    e.Handled = true;
                    return;

                case Key.Tab:
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LeftShift:
                case Key.RightShift:
                    e.Handled = true;
                    break;

                case Key.Escape:
                    Key = Key.None;
                    Text = Key.None.ToString();
                    Keyboard.ClearFocus();
                    break;

                case Key.Enter:
                    Key = e.Key;
                    Keyboard.ClearFocus();
                    break;

                default:
                    Text = "";
                    Key = e.Key;
                    e.Handled = true;
                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        #endregion


        protected virtual void OnKeyChanged(Key e)
        {
            KeyChanged?.Invoke(this, e);
        }
    }
}