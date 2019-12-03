using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using Timer = System.Timers.Timer;

namespace ThemesLib.InStat.Fitness.Controls
{
    public delegate Task<List<object>> UpdateItemsDelegate(string name, CancellationToken ct);

    public delegate bool EqualsObjsDelegate(object a, object b);

    public class PComboBox : ComboBox
    {
        #region Private fields

        private CancellationTokenSource _cts = new CancellationTokenSource();
        private TextBox _editableTextBox;
        private TextBox _topEditableTextBox;
        private Timer _typeTimer;

        #endregion

        #region Construct

        static PComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PComboBox), new FrameworkPropertyMetadata(typeof(PComboBox)));
        }

        public PComboBox()
        {
            Loaded += OnLoaded;
        }

        #endregion

        #region Public

        public UpdateItemsDelegate UpdateItems { get; set; }

        public EqualsObjsDelegate EqualsObjs { get; set; }

        #endregion

        #region Private methods

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ApplyTemplate();

            IsEditable = true;
            IsTextSearchEnabled = false;

            _editableTextBox = Template.FindName("PART_EditableTextBox", this) as TextBox;
            _topEditableTextBox = Template.FindName("PART_TopEditableTextBox", this) as TextBox;

            if (_editableTextBox == null) throw new NullReferenceException(nameof(_editableTextBox));
            if (_topEditableTextBox == null) throw new NullReferenceException(nameof(_topEditableTextBox));

            _topEditableTextBox.Tag = "Selection";

            _topEditableTextBox.PreviewKeyDown += TopEditableTextBoxOnPreviewKeyDown;
            _topEditableTextBox.TextChanged += TopEditableTextBoxOnTextChanged;

            _typeTimer = new Timer() { AutoReset = false, Interval = 500 };
            _typeTimer.Elapsed += TypeTimerOnElapsed;
        }

        private void TopEditableTextBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            if (sender is TextBox)
            {
                _editableTextBox.Text = (sender as TextBox).Text;
            }
        }

        private void TopEditableTextBoxOnPreviewKeyDown(object sender, KeyEventArgs ev)
        {
            if (ev.Key == Key.Return ||
                ev.Key == Key.Tab
                )
                return;

            if (ev.Key == Key.Enter)
            {
                IsDropDownOpen = false;
                return;
            }

            if (ev.Key == Key.Down)
            {
                if (SelectedItem == null)
                {
                    SelectedIndex = 0;
                }
                else
                {
                    SelectedIndex++;
                }
                return;
            }

            if (ev.Key == Key.Up)
            {
                if (SelectedItem == null)
                {
                    SelectedIndex = 0;
                }
                else
                {
                    SelectedIndex--;
                }
                return;
            }

            _typeTimer.Stop();
            _typeTimer.Start();
            _topEditableTextBox.Tag = "Typed";
        }

        private void TypeTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Dispatcher.Invoke(async () =>
            {
                if (_topEditableTextBox.Text.Length > 1)
                {
                    if ((string)_topEditableTextBox.Tag == "Typed")
                    {
                        IsDropDownOpen = true;

                        ShowBusyState();

                        _cts.Cancel();
                        _cts = new CancellationTokenSource();

                        var token = _cts.Token;
                        var searchTerm = _topEditableTextBox.Text.ToLowerInvariant();

                        try
                        {
                            if (UpdateItems != null)
                            {
                                List<object> list = ListContains(await UpdateItems(searchTerm, token), searchTerm);

                                var r = Items.Count - list.Count;
                                if (r > 0)
                                {
                                    int start = Items.Count - 1;
                                    int end = Items.Count - 1 - r;
                                    for (int i = start; i > end; i--)
                                    {
                                        token.ThrowIfCancellationRequested();
                                        Items.RemoveAt(i);
                                    }
                                }

                                int len = 4;
                                for (int i = 0; i < list.Count; i += len)
                                {
                                    try
                                    {
                                        var i1 = i;
                                        await Dispatcher.InvokeAsync(() =>
                                        {
                                            for (int j = i1; j < i1 + len && j < list.Count; j++)
                                            {
                                                _cts.Token.ThrowIfCancellationRequested();

                                                if (j > Items.Count - 1)
                                                {
                                                    Items.Add(list[j]);
                                                }
                                                else if (EqualsObjs != null)
                                                {
                                                    object oldTeam = Items[j];
                                                    object newTeam = list[j];

                                                    if (!EqualsObjs(oldTeam, newTeam))
                                                    {
                                                        Items[j] = newTeam;
                                                    }
                                                }
                                            }
                                        }, DispatcherPriority.Render, _cts.Token);
                                    }
                                    catch (OperationCanceledException)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        catch (OperationCanceledException)
                        { }

                        HideBusyState();

                        if (Items.Count == 1)
                        {
                            SelectedIndex = 0;
                        }
                    }
                }
                else
                {
                    IsDropDownOpen = false;
                }
            });
        }

        private List<object> ListContains(List<object> list, string str)
        {
            var res = list.AsParallel().Where(x => x.ToString().ToLowerInvariant().Contains(str)).ToList();
            return res;
        }

        private void ShowBusyState()
        {
            var toggleButton = Template.FindName("ToggleButton", this) as ToggleButton;
            VisualStateManager.GoToState(toggleButton, "BusyState", false);
            Dispatcher.Yield();
        }

        private void HideBusyState()
        {
            Dispatcher.Yield();
            var toggleButton = Template.FindName("ToggleButton", this) as ToggleButton;
            VisualStateManager.GoToState(toggleButton, "UnBusyState", false);
        }

        #endregion

        #region Protected Методы

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (SelectedItem != null)
            {
                _topEditableTextBox.Tag = "Selection";
                _topEditableTextBox.Text = SelectedItem.ToString();
            }

            base.OnSelectionChanged(e);
        }

        #endregion
    }
}