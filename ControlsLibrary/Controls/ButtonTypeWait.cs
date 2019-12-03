using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ColorsLib.InStat;

namespace ControlsLibrary.Controls
{
    public class ButtonTypeWait : Button
    {
        public static readonly DependencyProperty MouseOverColorProperty =
            DependencyProperty.Register(nameof(MouseOverColor), typeof(Brush), typeof(ButtonTypeWait),
                new FrameworkPropertyMetadata((object)null,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public Brush MouseOverColor
        {
            get => (Brush)GetValue(MouseOverColorProperty);
            set => SetValue(MouseOverColorProperty, value);
        }

        public static readonly DependencyProperty MousePressedColorProperty =
            DependencyProperty.Register(nameof(MousePressedColor), typeof(Brush), typeof(ButtonTypeWait),
                new FrameworkPropertyMetadata((object)null,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public Brush MousePressedColor
        {
            get => (Brush)GetValue(MousePressedColorProperty);
            set => SetValue(MousePressedColorProperty, value);
        }


        static ButtonTypeWait()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonTypeWait), new FrameworkPropertyMetadata(typeof(ButtonTypeWait)));
        }

        public ButtonTypeWait()
        {
            
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            

        }
    }
}