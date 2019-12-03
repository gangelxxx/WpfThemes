using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlsLibrary.Controls
{
    public class ScrollViewerTypeOne : ScrollViewer
    {
        static ScrollViewerTypeOne()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollViewerTypeOne), new FrameworkPropertyMetadata(typeof(ScrollViewerTypeOne)));
        }

        public ScrollViewerTypeOne()
        {
            Loaded += OnLoaded;
        }
            
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style)FindResource(new ComponentResourceKey(typeof(ScrollViewerTypeOne), "ScrollViewerTypeOneStyle"));
        }

        public static readonly DependencyProperty ThumbColorProperty =
            DependencyProperty.Register(nameof(ThumbColor), typeof(Brush), typeof(ScrollViewerTypeOne),
                new PropertyMetadata(null));

        public Brush ThumbColor
        {
            get { return (Brush)GetValue(ThumbColorProperty); }
            set { SetValue(ThumbColorProperty, value); }
        }

        public static readonly DependencyProperty RepeatbuttonallBackgroundColorProperty =
            DependencyProperty.Register(nameof(RepeatbuttonallBackgroundColor), typeof(Brush), typeof(ScrollViewerTypeOne),
                new PropertyMetadata(null));

        public Brush RepeatbuttonallBackgroundColor
        {
            get { return (Brush)GetValue(RepeatbuttonallBackgroundColorProperty); }
            set { SetValue(RepeatbuttonallBackgroundColorProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }
    }
}