#region

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#endregion

namespace ThemesLib.InStat.Helpers.BusyIndicator
{
    [StyleTypedProperty(Property = "BusyStyle", StyleTargetType = typeof(Control))]
    public class BusyDecorator : Decorator
    {
        #region Other members

        protected override int VisualChildrenCount
        {
            get { return Child != null ? 2 : 1; }
        }

        protected override System.Collections.IEnumerator LogicalChildren
        {
            get
            {
                if (Child != null)
                    yield return Child;

                yield return _busyHost;
            }
        }

        #endregion

        #region Static

        public static readonly DependencyProperty IsBusyIndicatorShowingProperty = DependencyProperty.Register(
            nameof(IsBusyIndicatorShowing),
            typeof(bool),
            typeof(BusyDecorator),
            new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty BusyStyleProperty =
            DependencyProperty.Register(
                nameof(BusyStyle),
                typeof(Style),
                typeof(BusyDecorator),
                new FrameworkPropertyMetadata(OnBusyStyleChanged));

        public static readonly DependencyProperty BusyHorizontalAlignmentProperty = DependencyProperty.Register(
            nameof(BusyHorizontalAlignment),
            typeof(HorizontalAlignment),
            typeof(BusyDecorator),
            new FrameworkPropertyMetadata(HorizontalAlignment.Center));

        public static readonly DependencyProperty BusyVerticalAlignmentProperty = DependencyProperty.Register(
            nameof(BusyVerticalAlignment),
            typeof(VerticalAlignment),
            typeof(BusyDecorator),
            new FrameworkPropertyMetadata(VerticalAlignment.Center));

        #endregion

        #region Private fields

        private readonly BackgroundVisualHost _busyHost = new BackgroundVisualHost();

        #endregion

        #region Construct

        static BusyDecorator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(BusyDecorator),
                new FrameworkPropertyMetadata(typeof(BusyDecorator)));
        }

        public BusyDecorator()
        {
            AddLogicalChild(_busyHost);
            AddVisualChild(_busyHost);

            SetBinding(_busyHost, IsBusyIndicatorShowingProperty, BackgroundVisualHost.IsContentShowingProperty);
            SetBinding(_busyHost, BusyHorizontalAlignmentProperty, BackgroundVisualHost.HorizontalAlignmentProperty);
            SetBinding(_busyHost, BusyVerticalAlignmentProperty, BackgroundVisualHost.VerticalAlignmentProperty);
        }

        #endregion

        #region Public

        public bool IsBusyIndicatorShowing
        {
            get { return (bool) GetValue(IsBusyIndicatorShowingProperty); }
            set { SetValue(IsBusyIndicatorShowingProperty, value); }
        }

        public Style BusyStyle
        {
            get { return (Style) GetValue(BusyStyleProperty); }
            set { SetValue(BusyStyleProperty, value); }
        }

        public HorizontalAlignment BusyHorizontalAlignment
        {
            get { return (HorizontalAlignment) GetValue(BusyHorizontalAlignmentProperty); }
            set { SetValue(BusyHorizontalAlignmentProperty, value); }
        }

        public VerticalAlignment BusyVerticalAlignment
        {
            get { return (VerticalAlignment) GetValue(BusyVerticalAlignmentProperty); }
            set { SetValue(BusyVerticalAlignmentProperty, value); }
        }

        #endregion

        #region Private methods

        private static void OnBusyStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BusyDecorator bd = (BusyDecorator) d;
            Style nVal = (Style) e.NewValue;
            bd._busyHost.CreateContent = () => new Control {Style = nVal};
        }

        private void SetBinding(DependencyObject obj, DependencyProperty source, DependencyProperty target)
        {
            Binding b = new Binding();
            b.Source = this;
            b.Path = new PropertyPath(source);
            BindingOperations.SetBinding(obj, target, b);
        }

        #endregion

        #region Protected Методы

        protected override System.Windows.Media.Visual GetVisualChild(int index)
        {
            if (Child != null)
            {
                switch (index)
                {
                    case 0:
                        return Child;

                    case 1:
                        return _busyHost;
                }
            }
            else if (index == 0)
                return _busyHost;

            throw new IndexOutOfRangeException("index");
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size ret = new Size(0, 0);
            if (Child != null)
            {
                Child.Measure(constraint);
                ret = Child.DesiredSize;
            }

            _busyHost.Measure(constraint);

            return new Size(Math.Max(ret.Width, _busyHost.DesiredSize.Width), Math.Max(ret.Height, _busyHost.DesiredSize.Height));
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Size ret = new Size(0, 0);
            if (Child != null)
            {
                Child.Arrange(new Rect(arrangeSize));
                ret = Child.RenderSize;
            }

            _busyHost.Arrange(new Rect(arrangeSize));

            return new Size(Math.Max(ret.Width, _busyHost.RenderSize.Width), Math.Max(ret.Height, _busyHost.RenderSize.Height));
        }

        #endregion
    }
}