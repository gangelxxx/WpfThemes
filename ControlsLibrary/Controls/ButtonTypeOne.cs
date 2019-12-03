using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ColorsLib.InStat;

namespace ControlsLibrary.Controls
{
    public class ButtonTypeOne : Button
    {
        public static readonly DependencyProperty MouseOverColorProperty =
            DependencyProperty.Register(nameof(MouseOverColor), typeof(Brush), typeof(ButtonTypeOne),
                new FrameworkPropertyMetadata((object)null,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public Brush MouseOverColor
        {
            get => (Brush)GetValue(MouseOverColorProperty);
            set => SetValue(MouseOverColorProperty, value);
        }

        public static readonly DependencyProperty MousePressedColorProperty =
            DependencyProperty.Register(nameof(MousePressedColor), typeof(Brush), typeof(ButtonTypeOne),
                new FrameworkPropertyMetadata((object)null,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public Brush MousePressedColor
        {
            get => (Brush)GetValue(MousePressedColorProperty);
            set => SetValue(MousePressedColorProperty, value);
        }


        static ButtonTypeOne()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonTypeOne), new FrameworkPropertyMetadata(typeof(ButtonTypeOne)));
        }

        public ButtonTypeOne()
        {
            Loaded += OnLoaded;
        }

        public void WaitState()
        {
            Dispatcher.Invoke(() => { VisualStateManager.GoToState(this, "WaitInidicatorShowState", false); }, DispatcherPriority.Send);
        }

        public void NormalState()
        {
            Dispatcher.Invoke(() => { VisualStateManager.GoToState(this, "WaitInidicatorHideState", false); }, DispatcherPriority.Send);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            NormalState();

        }
    }
}