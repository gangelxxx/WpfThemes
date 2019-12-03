#region

using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

#endregion

namespace ControlsLibrary.Controls
{
    public class WaitComboBox : ComboBox, INotifyPropertyChanged
    {
        #region Construct

        static WaitComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitComboBox), new FrameworkPropertyMetadata(typeof(WaitComboBox)));
        }

        public WaitComboBox()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        #endregion

        #region Const

        private const int TIMER_INTERVAL_CLICK = 500;
        private const int TIMER_START_ITEMSSOURCE = 2000;

        #endregion

        #region Events

        public event EventHandler LoadCompleted;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Static

        public static readonly DependencyProperty SelectedItemStringProperty =
            DependencyProperty.Register(nameof(SelectedItemString), typeof(string), typeof(WaitComboBox),
                new FrameworkPropertyMetadata(default(string), SeletedByItemStringChangedCallback));

        #endregion

        #region Fields

        #region Private fields

        private string _currentText;
        private bool _isKeyDown = false;

        private bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value == _isBusy) return;

                _isBusy = value;

                if (_isBusy)
                    BusyTextBoxState();
                else
                    NormalState();
            }
        }

        private Action<string> _refreshItemsSource;
        private TextBox _targetTextBox;

        private int _tempSelectedIdx = -1;
        private Timer _timer;
        private Timer _timerWaitingBusy;
        private Timer _timerWaitItemsSource;
        private bool _isBusy;

        #endregion

        public string SelectedItemString
        {
            get => (string) GetValue(SelectedItemStringProperty);
            set => SetValue(SelectedItemStringProperty, value);
        }

        public new object SelectedItem
        {
            get
            {
                object res = null;

                Dispatcher.Invoke(() =>
                {
                    if (SelectedIndex >= 0 && Items.Count > 0 && SelectedIndex < Items.Count)
                    {
                        res = Items[this.SelectedIndex];
                    }
                }, DispatcherPriority.Normal);

                return res;
            }
            set
            {
                Dispatcher.Invoke(() =>
                {
                    for (var i = 0; i < this.Items.Count; i++)
                    {
                        var item = this.Items[i];
                        if (item == value)
                        {
                            this.SelectedIndex = i;
                        }
                    }
                }, DispatcherPriority.Normal);

                OnPropertyChanged();
            }
        }

        #endregion

        #region Private static methods

        private static void SeletedByItemStringChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WaitComboBox waitCombo)
            {
                UpdateTextBox(waitCombo, e.NewValue.ToString());
            }
        }

        private static void UpdateTextBox(WaitComboBox waitCombo, string str, bool isRefresh = true)
        {
            if (waitCombo?._targetTextBox == null || waitCombo._timer == null || str == null || waitCombo._currentText == str) return;

            waitCombo._targetTextBox.Text = str;
            waitCombo._targetTextBox.Select(0, waitCombo._targetTextBox.Text.Length);

            if (isRefresh)
                waitCombo._timer.Start();
        }

        #endregion

        #region Private methods

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style) FindResource(new ComponentResourceKey(typeof(WaitComboBox), "KWaitComboBox"));
            Refresh();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _timer?.Dispose();
            _timerWaitItemsSource?.Dispose();
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
            //Если время не вышло
            if (_timer.Enabled)
            {
                _timer.Stop();
                _timer.Start();
            }
            else
            {
                _timer.Start();
            }
        }

        private void TargetTextBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            _currentText = _targetTextBox.Text;
        }

        private void TargetTextBoxOnPreviewKeyDown(object sender, KeyEventArgs ev)
        {
            _isKeyDown = true;

            if (_isBusy)
            {
                ev.Handled = true;
                return;
            }

            switch (ev.Key)
            {
                case Key.Down:
                {
                    if (!IsDropDownOpen)
                        IsDropDownOpen = true;
                    else
                        SelectedIndex += 1;
                    return;
                }

                case Key.Up:
                {
                    if (!IsDropDownOpen)
                        IsDropDownOpen = true;
                    else
                        SelectedIndex -= 1;
                    return;
                }

                case Key.Enter:
                {
                    return;
                }
            }


            UpdateInputText();
        }

        private void TimerOnTickAsync(object sender, EventArgs e)
        {
            IsBusy = true;
            _refreshItemsSource.Invoke(_currentText);
        }

        private void GetRefreshFunc(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var method = obj.GetType().GetMethod("Refresh", new[] {typeof(string)});
            _refreshItemsSource =
                (Action<string>) Delegate.CreateDelegate(typeof(Action<string>), obj, method ?? throw new InvalidOperationException());
        }

        private void SetEventCollectionChanged(object obj, string eventName, string methodName)
        {
            var eventInfo = obj.GetType().GetEvent(eventName);

            MethodInfo methodInfo =
                typeof(WaitComboBox).GetMethod(methodName,
                    BindingFlags.NonPublic | BindingFlags.Instance);

            if (methodInfo == null)
            {
                throw new Exception(methodName + " not found");
            }

            MethodInfo addHandler = eventInfo.GetAddMethod();
            addHandler.Invoke(obj, new object[]
            {
                Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo)
            });
        }

        private void OnVirtualListLoadCompleted(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (!string.IsNullOrEmpty(_currentText) && Items.Count > 0 && _isKeyDown)
                    IsDropDownOpen = true;

                _isKeyDown = false;

                if (!string.IsNullOrEmpty(_currentText) && Items.Count == 1)
                {
                    SelectedIndex = 0;
                    IsDropDownOpen = false;
                }

                IsBusy = false;
                OnLoadCompleted();
            });
        }

        private void TimerWaitItemsSourceOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (Items.Count == 0)
            {
                IsBusy = false;
            }
        }

        private void BusyTextBoxState()
        {
            Dispatcher.Invoke(() => { VisualStateManager.GoToState(this, "Busy", false); }, DispatcherPriority.Send);
        }

        private void NormalState()
        {
            Dispatcher.Invoke(() => { VisualStateManager.GoToState(this, "Normal", false); }, DispatcherPriority.Send);
        }

        #endregion

        #region Public

        public async void Refresh()
        {
            if (_refreshItemsSource != null)
            {
                IsBusy = true;
                await Task.Run(() => { _refreshItemsSource.Invoke(_currentText); });
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _timer = new Timer();
            _timer.AutoReset = false;
            _timer.Interval = TIMER_INTERVAL_CLICK;
            _timer.Elapsed += TimerOnTickAsync;
            _timer.Enabled = false;

            _timerWaitItemsSource = new Timer();
            _timerWaitItemsSource.AutoReset = false;
            _timerWaitItemsSource.Interval = TIMER_START_ITEMSSOURCE;
            _timerWaitItemsSource.Elapsed += TimerWaitItemsSourceOnElapsed;
            _timerWaitItemsSource.Enabled = false;
            _timerWaitItemsSource.Start();

            _timerWaitingBusy = new Timer();
            _timerWaitingBusy.AutoReset = false;
            _timerWaitingBusy.Interval = TIMER_START_ITEMSSOURCE;
            _timerWaitingBusy.Elapsed += TimerWaitItemsSourceOnElapsed;
            _timerWaitingBusy.Enabled = false;
            _timerWaitingBusy.Start();

            _targetTextBox = (TextBox) Template.FindName("PART_EditableTextBox2", this);
            _targetTextBox.PreviewKeyDown += TargetTextBoxOnPreviewKeyDown;
            _targetTextBox.TextChanged += TargetTextBoxOnTextChanged;
            _targetTextBox.AddHandler(CommandManager.ExecutedEvent, new RoutedEventHandler(PasteHandler), true);

            UpdateTextBox(this, SelectedItemString);
        }

        public new bool Focus()
        {
            var res = false;
            Dispatcher.Invoke(() =>
            {
                if (_targetTextBox == null)
                {
                    res = base.Focus();
                }

                res = _targetTextBox.Focus();
            });

            return res;
        }

        #endregion

        #region Protected Методы

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);

            GetRefreshFunc(newValue);
            SetEventCollectionChanged(newValue, "LoadCompleted", nameof(OnVirtualListLoadCompleted));

            IsBusy = true;
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (_tempSelectedIdx < 0 && SelectedIndex > 0)
            {
                _tempSelectedIdx = SelectedIndex;
            }
            else if (SelectedIndex < 0)
            {
                SelectedIndex = _tempSelectedIdx;
                _tempSelectedIdx = -1;
            }

            UpdateTextBox(this, SelectedItem?.ToString(), false);

            base.OnSelectionChanged(e);
        }

        protected virtual void OnLoadCompleted()
        {
            LoadCompleted?.Invoke(this, EventArgs.Empty);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}