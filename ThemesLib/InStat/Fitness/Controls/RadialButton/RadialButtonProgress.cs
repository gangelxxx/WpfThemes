#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#endregion

namespace ThemesLib.InStat.Fitness.Controls.RadialButton
{
    [ContentProperty("Content")]
    public class RadialButtonProgress : Control
    {
        #region Static

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(string), typeof(RadialButtonProgress),
                new PropertyMetadata("", null));

        public static readonly DependencyProperty GapScaleProperty =
            DependencyProperty.Register(nameof(GapScale), typeof(double), typeof(RadialButtonProgress),
                new PropertyMetadata(0.90, null));

        public static readonly DependencyProperty MaskOffsetProperty =
            DependencyProperty.Register(nameof(MaskOffset), typeof(double), typeof(RadialButtonProgress),
                new PropertyMetadata(0.0, null));

        public static readonly DependencyProperty MaskR1Property =
            DependencyProperty.Register(nameof(MaskR1), typeof(double), typeof(RadialButtonProgress),
                new PropertyMetadata(0.0, null));

        public static readonly DependencyProperty DispProperty =
            DependencyProperty.Register(nameof(Disp), typeof(int), typeof(RadialButtonProgress),
                new PropertyMetadata(2, null));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(RadialButtonProgress),
                new PropertyMetadata("", null));

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register(nameof(Percentage), typeof(double), typeof(RadialButtonProgress),
                new PropertyMetadata(0.0, null));

        public static readonly DependencyProperty ProgressColorProperty =
            DependencyProperty.Register(nameof(ProgressColor), typeof(Brush), typeof(RadialButtonProgress),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), null));

        public static readonly DependencyProperty MaskColorProperty =
            DependencyProperty.Register(nameof(MaskColor), typeof(Brush), typeof(RadialButtonProgress),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), null));

        public static readonly DependencyProperty ProgressBackgroundProperty =
            DependencyProperty.Register(nameof(ProgressBackground), typeof(Brush), typeof(RadialButtonProgress),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), null));

        #endregion

        #region Construct

        static RadialButtonProgress()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadialButtonProgress),
                new FrameworkPropertyMetadata(typeof(RadialButtonProgress)));
        }

        public RadialButtonProgress()
        {
            Loaded += OnLoaded;
        }

        #endregion

        #region Public

        public string Content
        {
            get { return (string) GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public double MaskOffset
        {
            get { return (double) GetValue(MaskOffsetProperty); }
            set { SetValue(MaskOffsetProperty, value); }
        }

        public double MaskR1
        {
            get { return (double) GetValue(MaskR1Property); }
            set { SetValue(MaskR1Property, value); }
        }

        public double GapScale
        {
            get { return (double) GetValue(GapScaleProperty); }
            set { SetValue(GapScaleProperty, value); }
        }

        public int Disp
        {
            get { return (int) GetValue(DispProperty); }
            set { SetValue(DispProperty, value); }
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public double Percentage
        {
            get { return (double) GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public Brush ProgressColor
        {
            get { return (Brush) GetValue(ProgressColorProperty); }
            set { SetValue(ProgressColorProperty, value); }
        }

        public Brush MaskColor
        {
            get { return (Brush) GetValue(MaskColorProperty); }
            set { SetValue(MaskColorProperty, value); }
        }

        public Brush ProgressBackground
        {
            get { return (Brush) GetValue(ProgressBackgroundProperty); }
            set { SetValue(ProgressBackgroundProperty, value); }
        }

        #endregion

        #region Private methods

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            MaskR1 = Disp - 10;
            MaskOffset = (ActualWidth - MaskR1) / 2;
        }

        #endregion
    }
}