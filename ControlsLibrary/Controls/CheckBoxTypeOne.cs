using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlsLibrary.Controls
{
    public class CheckBoxTypeOne : CheckBox
    {
        public static readonly DependencyProperty CheckColorProperty =
            DependencyProperty.Register(nameof(CheckColor), typeof(Brush), typeof(CheckBoxTypeOne),
                new FrameworkPropertyMetadata((object)null,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        public Brush CheckColor
        {
            get => (Brush)GetValue(CheckColorProperty);
            set => SetValue(CheckColorProperty, value);
        }

        #region Construct

        static CheckBoxTypeOne()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckBoxTypeOne), new FrameworkPropertyMetadata(typeof(CheckBoxTypeOne)));
        }

        public CheckBoxTypeOne()
        {
            Loaded += OnLoaded;
        }

        #endregion

        #region Private methods

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Style = (Style)FindResource(new ComponentResourceKey(typeof(CheckBoxTypeOne), "KCheckBoxTypeOneStyle"));
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //            GetParts();
        }

        private void GetParts()
        {
            throw new NotImplementedException();
        }
    }
}