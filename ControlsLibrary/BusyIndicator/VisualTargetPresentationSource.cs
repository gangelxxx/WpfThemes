#region

using System;
using System.Windows;
using System.Windows.Media;

#endregion

namespace ControlsLibrary.BusyIndicator
{
    public class VisualTargetPresentationSource : PresentationSource
    {
        #region Private fields

        private readonly VisualTarget _visualTarget;

        #endregion

        #region Construct

        public VisualTargetPresentationSource(HostVisual hostVisual)
        {
            _visualTarget = new VisualTarget(hostVisual);
            AddSource();
        }

        #endregion

        #region Public

        public Size DesiredSize { get; private set; }

        public override Visual RootVisual
        {
            get { return _visualTarget.RootVisual; }
            set
            {
                Visual oldRoot = _visualTarget.RootVisual;
                _visualTarget.RootVisual = value;
                RootChanged(oldRoot, value);

                UIElement rootElement = value as UIElement;
                if (rootElement != null)
                {
                    rootElement.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    rootElement.Arrange(new Rect(rootElement.DesiredSize));

                    DesiredSize = rootElement.DesiredSize;
                }
                else
                    DesiredSize = new Size(0, 0);
            }
        }

        #endregion

        #region Protected Методы

        protected override CompositionTarget GetCompositionTargetCore()
        {
            return _visualTarget;
        }

        #endregion

        #region Dispose

        private bool _isDisposed = false;

        public override bool IsDisposed
        {
            get { return _isDisposed; }
        }

        internal void Dispose()
        {
            RemoveSource();
            _isDisposed = true;
        }

        #endregion
    }
}