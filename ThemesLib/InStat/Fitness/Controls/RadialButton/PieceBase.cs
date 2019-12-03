#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#endregion

namespace ThemesLib.InStat.Fitness.Controls.RadialButton
{
    public abstract class PieceBase : Control
    {
        #region Const

        private const string STATE_SELECTION_UNSELECTED = "Unselected";
        private const string STATE_SELECTION_SELECTED = "Selected";
        internal const string GROUP_SELECTION_STATES = "SelectionStates";

        #endregion

        #region Static

        public static readonly DependencyProperty ClientHeightProperty =
            DependencyProperty.Register(nameof(ClientHeight), typeof(double), typeof(PieceBase),
                new PropertyMetadata(0.0, OnSizeChanged));

        public static readonly DependencyProperty ClientWidthProperty =
            DependencyProperty.Register(nameof(ClientWidth), typeof(double), typeof(PieceBase),
                new PropertyMetadata(0.0, OnSizeChanged));

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register(nameof(SelectedBrush), typeof(Brush), typeof(PieceBase),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(PieceBase),
                new PropertyMetadata(false, OnIsSelectedChanged));

        public static readonly DependencyProperty IsClickedByUserProperty =
            DependencyProperty.Register(nameof(IsClickedByUser), typeof(bool), typeof(PieceBase),
                new PropertyMetadata(false));

        #endregion

        #region Public

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InternalOnApplyTemplate();
        }


        public double ClientHeight
        {
            get { return (double) GetValue(ClientHeightProperty); }
            set { SetValue(ClientHeightProperty, value); }
        }

        public double ClientWidth
        {
            get { return (double) GetValue(ClientWidthProperty); }
            set { SetValue(ClientWidthProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public bool IsClickedByUser
        {
            get { return (bool) GetValue(IsClickedByUserProperty); }
            set { SetValue(IsClickedByUserProperty, value); }
        }

        public Brush SelectedBrush
        {
            get { return (Brush) GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        #endregion

        #region Private methods

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pieceBase = d as PieceBase;
            pieceBase?.DrawGeometry(false);
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PieceBase source = (PieceBase) d;
            bool oldValue = (bool) e.OldValue;
            bool newValue = (bool) e.NewValue;
            source.OnIsSelectedPropertyChanged(oldValue, newValue);
        }

        private void InternalMousePressed()
        {
            SetValue(PieceBase.IsClickedByUserProperty, true);
        }

        private void InternalMouseMoved()
        {
            //SetValue(PieceBase.Is, true);
        }


        private void HandleMouseDown()
        {
            IsClickedByUser = true;
        }

        #endregion

        #region Protected Методы

        protected virtual void DrawGeometry(bool withAnimation = true)
        {}

        protected virtual void OnIsSelectedPropertyChanged(bool oldValue, bool newValue)
        {
            this.IsClickedByUser = false;
            VisualStateManager.GoToState(this, newValue ? STATE_SELECTION_SELECTED : STATE_SELECTION_UNSELECTED, true);
        }

        protected virtual void InternalOnApplyTemplate()
        {}

        protected void RegisterMouseEvents(UIElement slice)
        {
            if (slice != null)
            {
                slice.MouseLeftButtonUp += delegate { InternalMousePressed(); };

                slice.MouseMove += delegate { InternalMouseMoved(); };
            }
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            HandleMouseDown();
            e.Handled = true;
        }

        #endregion
    }
}