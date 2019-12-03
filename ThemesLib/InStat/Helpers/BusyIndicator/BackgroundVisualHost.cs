#region

using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

#endregion

namespace ThemesLib.InStat.Helpers.BusyIndicator
{
    public delegate Visual CreateContentFunction();

    public class BackgroundVisualHost : FrameworkElement
    {
        #region Other members

        protected override int VisualChildrenCount
        {
            get { return _hostVisual != null ? 1 : 0; }
        }

        protected override System.Collections.IEnumerator LogicalChildren
        {
            get
            {
                if (_hostVisual != null)
                    yield return _hostVisual;
            }
        }

        #endregion

        #region Static

        public static readonly DependencyProperty IsContentShowingProperty = DependencyProperty.Register(
            nameof(IsContentShowing),
            typeof(bool),
            typeof(BackgroundVisualHost),
            new FrameworkPropertyMetadata(false, OnIsContentShowingChanged));

        public static readonly DependencyProperty CreateContentProperty = DependencyProperty.Register(
            nameof(CreateContent),
            typeof(CreateContentFunction),
            typeof(BackgroundVisualHost),
            new FrameworkPropertyMetadata(OnCreateContentChanged));

        #endregion

        #region Private fields

        private HostVisual _hostVisual = null;
        private ThreadedVisualHelper _threadedHelper = null;

        #endregion

        #region Public

        public bool IsContentShowing
        {
            get { return (bool) GetValue(IsContentShowingProperty); }
            set { SetValue(IsContentShowingProperty, value); }
        }

        public CreateContentFunction CreateContent
        {
            get { return (CreateContentFunction) GetValue(CreateContentProperty); }
            set { SetValue(CreateContentProperty, value); }
        }

        #endregion

        #region Private methods

        private static void OnIsContentShowingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BackgroundVisualHost bvh = (BackgroundVisualHost) d;

            if (bvh.CreateContent != null)
            {
                if ((bool) e.NewValue)
                {
                    bvh.CreateContentHelper();
                }
                else
                {
                    bvh.HideContentHelper();
                }
            }
        }

        private static void OnCreateContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BackgroundVisualHost bvh = (BackgroundVisualHost) d;

            if (bvh.IsContentShowing)
            {
                bvh.HideContentHelper();
                if (e.NewValue != null)
                    bvh.CreateContentHelper();
            }
        }

        private void CreateContentHelper()
        {
            _threadedHelper = new ThreadedVisualHelper(CreateContent, SafeInvalidateMeasure);
            _hostVisual = _threadedHelper.HostVisual;
        }

        private void SafeInvalidateMeasure()
        {
            Dispatcher.BeginInvoke(new Action(InvalidateMeasure), DispatcherPriority.Loaded);
        }

        private void HideContentHelper()
        {
            if (_threadedHelper != null)
            {
                _threadedHelper.Exit();
                _threadedHelper = null;
                InvalidateMeasure();
            }
        }

        #endregion

        #region Protected Методы

        protected override Visual GetVisualChild(int index)
        {
            if (_hostVisual != null && index == 0)
                return _hostVisual;

            throw new IndexOutOfRangeException(nameof(index));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_threadedHelper != null)
                return _threadedHelper.DesiredSize;

            return base.MeasureOverride(availableSize);
        }

        #endregion

        #region  Class

        private class ThreadedVisualHelper
        {
            #region Private fields

            private Dispatcher Dispatcher { get; set; }
            private readonly CreateContentFunction _createContent;
            private readonly HostVisual _hostVisual = null;
            private readonly Action _invalidateMeasure;

            private readonly AutoResetEvent _sync =
                new AutoResetEvent(false);

            #endregion

            #region Construct

            public ThreadedVisualHelper(
                CreateContentFunction createContent,
                Action invalidateMeasure)
            {
                _hostVisual = new HostVisual();
                _createContent = createContent;
                _invalidateMeasure = invalidateMeasure;

                Thread backgroundUi = new Thread(CreateAndShowContent);
                backgroundUi.SetApartmentState(ApartmentState.STA);
                backgroundUi.Name = "BusyHostThread";
                backgroundUi.IsBackground = true;
                backgroundUi.Start();

                _sync.WaitOne();
            }

            #endregion

            #region Public

            public void Exit()
            {
                Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send);
            }

            public HostVisual HostVisual
            {
                get { return _hostVisual; }
            }

            public Size DesiredSize { get; private set; }

            #endregion

            #region Private methods

            private void CreateAndShowContent()
            {
                try
                {
                    Dispatcher = Dispatcher.CurrentDispatcher;
                    VisualTargetPresentationSource source =
                        new VisualTargetPresentationSource(_hostVisual);
                    _sync.Set();
                    source.RootVisual = _createContent();
                    DesiredSize = source.DesiredSize;
                    _invalidateMeasure();

                    Dispatcher.Run();
                    source.Dispose();
                }
                catch
                {}
            }

            #endregion
        }

        #endregion
    }
}