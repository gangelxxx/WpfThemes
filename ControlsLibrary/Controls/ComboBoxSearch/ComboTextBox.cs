using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ControlsLibrary.Controls.ComboBoxSearch
{
    public class ComboTextBox : TextBox
    {
        private readonly Timer _timerEndTextChanged;
        private readonly int _pauseTextChanged = 100;

        public event EventHandler StartTypeEvent;
        public event EventHandler<string> EndTypeEvent;
        public event EventHandler<Key> KeyEvent; 
        public event EventHandler SelectNextEvent;
        public event EventHandler SelectBackEvent;

        public ComboTextBox()
        {
            _timerEndTextChanged = new Timer(_pauseTextChanged) { AutoReset = false };
            _timerEndTextChanged.Elapsed += TimerEndTextChangedOnElapsed;

            this.AddHandler(CommandManager.ExecutedEvent, new RoutedEventHandler(PasteHandler), true);
        }

        public void CaretToEnd()
        {
            CaretIndex = Text.Length;
        }

        public void UnSelected()
        {
            Select(Text.Length, 0);
        }

        private void TimerEndTextChangedOnElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => { OnEndTypeEvent(Text); }, DispatcherPriority.Send);
            _isSendStartTypeEvent = false;
        }

        private void PasteHandler(object sender, RoutedEventArgs e)
        {
            if ((e as ExecutedRoutedEventArgs)?.Command != ApplicationCommands.Paste) return;

            if (e.Handled)
            {
                UpdateInputText();
            }
        }

        private void UpdateInputText()
        {
            OnStartTypeEvent();

            if (_timerEndTextChanged.Enabled)
            {
                _timerEndTextChanged.Stop();
                _timerEndTextChanged.Start();
            }
            else
            {
                _timerEndTextChanged.Start();
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs ev)
        {
            base.OnPreviewKeyDown(ev);

            switch (ev.Key)
            {
                case Key.Tab:
                {
                    return;
                }

                case Key.Down:
                {
                    OnSelectNextEvent();
                    return;
                }

                case Key.Up:
                {
                    OnSelectBackEvent();
                    return;
                }

                case Key.Enter:
                {
                    return;
                }

                default:
                    UpdateInputText();
//                    OnKeyEvent(ev.Key);
                    return;
            }

           

//            UpdateInputText();
        }

        protected virtual void OnEndTypeEvent(string str)
        {
            EndTypeEvent?.Invoke(this, str);
        }

        private bool _isSendStartTypeEvent = false;

        protected virtual void OnStartTypeEvent()
        {
            if (!_isSendStartTypeEvent)
            {
                _isSendStartTypeEvent = true;
                StartTypeEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnSelectNextEvent()
        {
            SelectNextEvent?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSelectBackEvent()
        {
            SelectBackEvent?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnKeyEvent(Key e)
        {
            KeyEvent?.Invoke(this, e);
        }
    }
}