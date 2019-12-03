using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ControlsLibrary.VirtualList.Interface;
using VirtlistLib.Interface;

namespace ControlsLibrary.Controls.ComboBoxSearch
{
    public class ComboTextBlock : TextBlock
    {
        private readonly DependencyPropertyDescriptor _dependency = null;
        private readonly GradientStop _animGradient = null;

        public ComboTextBlock()
        {
            _dependency = DependencyPropertyDescriptor.FromProperty(TextBlock.TagProperty, typeof(TextBlock));
            _dependency.AddValueChanged(this, (s, a) =>
            {
                if (s is TextBlock textBlock && textBlock.Tag is VirtualListItem item)
                {
                    if (string.IsNullOrEmpty(item.LeftText) && string.IsNullOrEmpty(item.ColorText) && string.IsNullOrEmpty(item.RightText))
                    {
                        textBlock.Inlines.Add(new Run(item.ToString()));
                    }

                    if (!string.IsNullOrEmpty(item.LeftText))
                    {
                        textBlock.Inlines.Add(new Run(item.LeftText));
                    }

                    if (!string.IsNullOrEmpty(item.ColorText))
                    {
                        textBlock.Inlines.Add(new Run(item.ColorText) {Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 119, 119)) });
                    }

                    if (!string.IsNullOrEmpty(item.RightText))
                    {
                        textBlock.Inlines.Add(new Run(item.RightText));
                    }

                    if (_animGradient != null)
                    {
                        _animGradient.BeginAnimation(GradientStop.OffsetProperty, null);
                        Background = new SolidColorBrush(Colors.White);
                    }
                }
            });

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Inlines.Count == 0)
            {
                GradientStopCollection gradientStopCollection = new GradientStopCollection();
                gradientStopCollection.Add(new GradientStop(Colors.White, 0));
                gradientStopCollection.Add(new GradientStop(Colors.White, 1));
                var animGradient = new GradientStop(Color.FromArgb(100, 180, 180, 180), 0.0);
                gradientStopCollection.Add(animGradient);

                LinearGradientBrush gradient = new LinearGradientBrush(gradientStopCollection, new Point(0.5, 0), new Point(0.5, 1));
                Background = gradient;

                var animation = new DoubleAnimation(1, TimeSpan.FromSeconds(2))
                {
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true
                };

                animGradient.BeginAnimation(GradientStop.OffsetProperty, animation);
            }
        }
    }
}