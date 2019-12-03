using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ThemesLib.InStat.Helpers;

namespace ThemesLib.InStat.Fitness.Controls
{
    public delegate void DragCompletedDelegate(DateTime value);

    public class VideoSlider : Control
    {
        #region Events

        public event DragStartedEventHandler OnDragStarted;
        public event DragCompletedDelegate OnDragCompleted;
        public event EventHandler<DateTime> ValueChanged;
        public event RoutedEventHandler TrackIncreasePreviewMouseDown;
        public event RoutedEventHandler TrackDecreasePreviewMouseDown;

        #endregion

        #region Static

        public static readonly DependencyProperty EndTimeProperty =
            DependencyProperty.Register(nameof(EndTime), typeof(DateTime), typeof(VideoSlider),
                new PropertyMetadata(new DateTime()));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(DateTime), typeof(VideoSlider),
                new PropertyMetadata(new DateTime(), PropertyChangedCallback));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(VideoSlider),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(VideoSlider),
                new PropertyMetadata(100.0));

        #endregion

        #region Private fields

        private Slider _slider = null;

        #endregion

        #region Construct

        static VideoSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoSlider), new FrameworkPropertyMetadata(typeof(VideoSlider)));
        }

        public VideoSlider()
        {
            Loaded += OnLoaded;
        }

        #endregion

        #region Public

        public DateTime EndTime
        {
            get { return (DateTime)GetValue(EndTimeProperty); }
            set { SetValue(EndTimeProperty, value); }
        }

        public DateTime Value
        {
            get { return (DateTime)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        #endregion

        #region Private methods

        private static void PropertyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
        {
            VideoSlider vs = obj as VideoSlider;
            var slider = vs?.FindChildByName<Slider>("NSlider");
            if (slider != null)
            {
                DateTime beginDateTime = new DateTime();
                double t = (vs.Value - beginDateTime).TotalMilliseconds;
                double e = (vs.EndTime - beginDateTime).TotalMilliseconds;
                double sv = t * 100 / e;
                if (Math.Abs(slider.Value - sv) > 0)
                {
                    slider.Value = sv;
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _slider = this.FindChildByName<Slider>("NSlider");
            if (_slider != null)
            {
                _slider.Minimum = Minimum;
                _slider.Maximum = Maximum;
                _slider.ValueChanged += SliderOnValueChanged;
                _slider.PreviewMouseLeftButtonDown += SliderOnPreviewMouseLeftButtonDown;

                _slider.IsMoveToPointEnabled = true;

                var thumb = _slider.FindChild<Thumb>();
                thumb.DragCompleted += ThumbOnDragCompleted;
                thumb.DragStarted += ThumbOnDragStarted;
                thumb.DragDelta += ThumbOnDragDelta;

                
                var track = _slider.FindChild<Track>();
                track.DecreaseRepeatButton.PreviewMouseDown += DecreaseRepeatButtonOnPreviewMouseDown;
                track.IncreaseRepeatButton.PreviewMouseDown += IncreaseRepeatButtonOnPreviewMouseDown;
            }
        }

        private void SliderOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void ThumbOnDragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;
            TextBlock textBlock = thumb.FindChild<TextBlock>("NValueTextBlock");

            var videoSlider = _slider.FindVisualParent<VideoSlider>();

            DateTime beginDateTime = new DateTime();
            DateTime endDateTime = videoSlider.EndTime;

            double val = ((endDateTime - beginDateTime).TotalMilliseconds * _slider.Value) / 100;
            DateTime resTime = beginDateTime + TimeSpan.FromMilliseconds(val);

            videoSlider.Value = resTime;

            textBlock.Text = resTime.ToString("HH:mm:ss");
        }

        private void IncreaseRepeatButtonOnPreviewMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            OnTrackIncreasePreviewMouseDown(mouseButtonEventArgs);
        }

        private void DecreaseRepeatButtonOnPreviewMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            OnTrackDecreasePreviewMouseDown(mouseButtonEventArgs);
        }

        private void SliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> ev)
        {
            Slider slider = sender as Slider;

            DateTime beginDateTime = new DateTime();
            DateTime endDateTime = EndTime;

            if (slider != null)
            {
                double val = (endDateTime - beginDateTime).TotalMilliseconds * slider.Value / 100;
                DateTime resTime = beginDateTime + TimeSpan.FromMilliseconds(val);

                Value = resTime;
                OnValueChanged(resTime);
            }
        }

        private void ThumbOnDragStarted(object sender, DragStartedEventArgs e)
        {
            OnDragStarted?.Invoke(this, e);
        }

        private void ThumbOnDragCompleted(object sender, DragCompletedEventArgs dragCompletedEventArgs)
        {
            OnDragCompleted?.Invoke(Value);
        }
        #endregion

        protected virtual void OnValueChanged(DateTime e)
        {
            ValueChanged?.Invoke(this, e);
        }

        protected virtual void OnTrackIncreasePreviewMouseDown(RoutedEventArgs e)
        {
            TrackIncreasePreviewMouseDown?.Invoke(this, e);
        }

        protected virtual void OnTrackDecreasePreviewMouseDown(RoutedEventArgs e)
        {
            TrackDecreasePreviewMouseDown?.Invoke(this, e);
        }

    }
}