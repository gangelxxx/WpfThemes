#region

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

#endregion

namespace ThemesLib.InStat.Fitness.Controls.RadialButton
{
    public enum EasingFunc
    {
        Linear = 0,
        EaseInQuad = 1,
        EaseOutQuad = 2,
        EaseInOutQuad = 3,
        EaseInCubic = 4,
        EaseOutCubic = 5,
        EaseInOutCubic = 6,
        EaseInQuart = 7,
        EaseOutQuart = 8,
        EaseInExpo = 9,
        EaseOutExpo = 10
    }

    public class RadialPiece : PieceBase
    {
        #region Private fields

        private double _animationCounter;
        private double _animationStartValue;

        #endregion

        #region Construct

        static RadialPiece()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadialPiece), new FrameworkPropertyMetadata(typeof(RadialPiece)));
        }

        public RadialPiece()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Normal);
            _timer.Tick += timer_Tick;

            Loaded += RadialGaugePiece_Loaded;
        }

        #endregion

        #region Public

        public Geometry Geometry
        {
            get { return (Geometry) GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }

        public Geometry Mask
        {
            get { return (Geometry) GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }

        public Geometry MaskStartPoint
        {
            get { return (Geometry) GetValue(MaskStartPointProperty); }
            set { SetValue(MaskStartPointProperty, value); }
        }

        public Geometry BackgroundGeometry
        {
            get { return (Geometry) GetValue(BackgroundGeometryProperty); }
            set { SetValue(BackgroundGeometryProperty, value); }
        }

        public Geometry SelectionGeometry
        {
            get { return (Geometry) GetValue(SelectionGeometryProperty); }
            set { SetValue(SelectionGeometryProperty, value); }
        }

        public Geometry MouseOverGeometry
        {
            get { return (Geometry) GetValue(MouseOverGeometryProperty); }
            set { SetValue(MouseOverGeometryProperty, value); }
        }

        public Brush ProgressColor
        {
            get { return (Brush) GetValue(ProgressColorProperty); }
            set { SetValue(ProgressColorProperty, value); }
        }

        public Brush ProgressColorAnimation
        {
            get { return (Brush) GetValue(ProgressColorProperty); }
            set { SetValue(ProgressColorProperty, value); }
        }

        public Brush MaskColor
        {
            get { return (Brush) GetValue(MaskColorProperty); }
            set { SetValue(MaskColorProperty, value); }
        }

        public Brush MaskColorAnimation
        {
            get { return (Brush) GetValue(MaskColorAnimationProperty); }
            set { SetValue(MaskColorAnimationProperty, value); }
        }

        public Brush ProgressBackground
        {
            get { return (Brush) GetValue(ProgressBackgroundProperty); }
            set { SetValue(ProgressBackgroundProperty, value); }
        }

        public Brush ProgressBackgroundAnimation
        {
            get { return (Brush) GetValue(ProgressBackgroundAnimationProperty); }
            set { SetValue(ProgressBackgroundAnimationProperty, value); }
        }

        public double GapScale
        {
            get { return (double) GetValue(GapScaleProperty); }
            set { SetValue(GapScaleProperty, value); }
        }

        public double Value
        {
            get { return (double) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double AnimatedValue
        {
            get { return (double) GetValue(AnimatedValueProperty); }
            set { SetValue(AnimatedValueProperty, value); }
        }

        public double FormattedAnimatedValue
        {
            get { return (double) GetValue(FormattedAnimatedValueProperty); }
            set { SetValue(FormattedAnimatedValueProperty, value); }
        }

        #endregion

        #region Private methods

        private static void UpdatePie(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radialGaugePiece = d as RadialPiece;
            radialGaugePiece?.UpdatePie();
        }


        private static void OnAnimatedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radialGaugePiece = d as RadialPiece;
            radialGaugePiece?.UpdateFormattedValue();
        }

        private void RadialGaugePiece_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePie();
        }

        private void UpdatePie()
        {
            if (Math.Abs(AnimatedValue - Value) >= 0)
            {
                _animationCounter = 0;
                _animationStartValue = AnimatedValue;


                Geometry segmentPathDataMask = MaskSegment(GapScale);
                if (segmentPathDataMask != null)
                {
                    SetValue(MaskProperty, segmentPathDataMask);
                }

                Geometry segmentPathDataBackground = MaskBackgroundSegment(GetRadius(), GapScale, GetCenter());
                if (segmentPathDataMask != null)
                {
                    SetValue(BackgroundGeometryProperty, segmentPathDataBackground);
                }

                _timer.Interval = TimeSpan.FromMilliseconds(2);
                _timer.Start();
                Tick();
            }
        }

        private void Tick()
        {
            if (Math.Abs(AnimatedValue - Value) > 0)
            {
                double t = _animationCounter;
                double b = _animationStartValue;
                double c = Value - _animationStartValue;
                double d = 10;

                var f = GetFormula(EasingFunc.Linear, t, b, d, c);

                AnimatedValue = f < 0 ? 0 : f;
                DrawGeometry();
                _animationCounter++;
            }
            else
            {
                AnimatedValue = Value;
                DrawGeometry();
                _timer.Stop();
            }
        }

        private double GetFormula(EasingFunc animType, double t, double b, double d, double c)
        {
            switch (animType)
            {
                case EasingFunc.Linear:
                    return c * t / d + b;

                case EasingFunc.EaseInQuad:
                    return c * (t /= d) * t + b;

                case EasingFunc.EaseOutQuad:
                    return -c * (t = t / d) * (t - 2) + b;

                case EasingFunc.EaseInOutQuad:
                    if ((t /= d / 2) < 1) return c / 2 * t * t + b;
                    else return -c / 2 * (--t * (t - 2) - 1) + b;

                case EasingFunc.EaseInCubic:
                    return c * (t /= d) * t * t + b;

                case EasingFunc.EaseOutCubic:
                    return c * ((t = t / d - 1) * t * t + 1) + b;

                case EasingFunc.EaseInOutCubic:
                    if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
                    else return c / 2 * ((t -= 2) * t * t + 2) + b;

                case EasingFunc.EaseInQuart:
                    return c * (t /= d) * t * t * t + b;

                case EasingFunc.EaseOutQuart:
                    return -c * ((t = t / d - 1) * t * t * t - 1) + b;

                case EasingFunc.EaseInExpo:
                    if (Math.Abs(t) <= 0) return b;
                    else return c * Math.Pow(2, 10 * (t / d - 1)) + b;

                case EasingFunc.EaseOutExpo:
                    if (Math.Abs(t - d) <= 0) return b + c;
                    else return c * (-Math.Pow(2, -10 * t / d) + 1) + b;

                default:
                    return 0;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Tick();
        }

        private void UpdateFormattedValue()
        {
            SetValue(FormattedAnimatedValueProperty, AnimatedValue.ToString("F0"));
        }

        private Point GetCircumferencePoint(double angle, double radius, double centerx, double centery)
        {
            angle = angle - 90;

            double angleRad = Math.PI / 180.0 * angle;

            double x = centerx + radius * Math.Cos(angleRad);
            double y = centery + radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

        private Geometry LayoutSegment(double startAngle, double endAngle, double radius, double gapScale, Point center, bool isDoughnut)
        {
            try
            {
                if (startAngle > 360)
                {
                    return null;
                }
                if (endAngle > 360)
                {
                    return null;
                }

                if (endAngle >= 359)
                {
                    endAngle = 359;
                }

                double pieRadius = radius;
                double gapRadius = pieRadius * (Math.Abs(gapScale) <= 0 ? 0.25 : gapScale);

                Point a = GetCircumferencePoint(startAngle, pieRadius, center.X, center.Y);
                Point b = isDoughnut ? GetCircumferencePoint(startAngle, gapRadius, center.X, center.Y) : center;
                Point c = GetCircumferencePoint(endAngle, gapRadius, center.X, center.Y);
                Point d = GetCircumferencePoint(endAngle, pieRadius, center.X, center.Y);

                bool isReflexAngle = Math.Abs(endAngle - startAngle) > 180.0;

                PathSegmentCollection segments = new PathSegmentCollection();
                segments.Add(new LineSegment() {Point = b});

                if (isDoughnut)
                {
                    segments.Add(new ArcSegment()
                    {
                        Size = new Size(gapRadius, gapRadius),
                        Point = c,
                        SweepDirection = SweepDirection.Clockwise,
                        IsLargeArc = isReflexAngle
                    });
                }

                segments.Add(new LineSegment() {Point = d});
                segments.Add(new ArcSegment()
                {
                    Size = new Size(pieRadius, pieRadius),
                    Point = a,
                    SweepDirection = SweepDirection.Counterclockwise,
                    IsLargeArc = isReflexAngle
                });

                Path segmentPath = new Path()
                {
                    StrokeLineJoin = PenLineJoin.Round,
                    Stroke = new SolidColorBrush() {Color = Colors.Black},
                    StrokeThickness = 0.0d,
                    Data = new PathGeometry()
                    {
                        Figures = new PathFigureCollection()
                        {
                            new PathFigure()
                            {
                                IsClosed = true,
                                StartPoint = a,
                                Segments = segments
                            }
                        }
                    }
                };

                return segmentPath.Data;
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }

        private Geometry MaskSegment(double gapScale)
        {
            try
            {
                Point center = GetCenter();
                double aR = GetRadius();
                double bR = ClientGetRadius() - 1;
                double cR = aR * (Math.Abs(gapScale) <= 0 ? 0.25 : gapScale) + 1;

                Point aS = GetCircumferencePoint(0, aR, center.X, center.Y);
                Point bS = GetCircumferencePoint(0, bR, center.X, center.Y);
                Point cS = GetCircumferencePoint(0, cR, center.X, center.Y);
                Point aE = GetCircumferencePoint(359, aR, center.X, center.Y);
                Point bE = GetCircumferencePoint(359, bR, center.X, center.Y);
                Point cE = GetCircumferencePoint(359, cR, center.X, center.Y);

                PathSegmentCollection segA = new PathSegmentCollection();

                segA.Add(new ArcSegment()
                {
                    Size = new Size(aR, aR),
                    Point = aE,
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = true
                });

                PathSegmentCollection segB = new PathSegmentCollection();

                segB.Add(new ArcSegment()
                {
                    Size = new Size(bR, bR),
                    Point = bE,
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = true
                });

                PathSegmentCollection segC = new PathSegmentCollection();

                segC.Add(new ArcSegment()
                {
                    Size = new Size(cR, cR),
                    Point = cE,
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = true
                });

                Geometry circA = new PathGeometry()
                {
                    Figures = new PathFigureCollection()
                    {
                        new PathFigure()
                        {
                            IsClosed = true,
                            StartPoint = aS,
                            Segments = segA
                        }
                    }
                };

                Geometry circB = new PathGeometry()
                {
                    Figures = new PathFigureCollection()
                    {
                        new PathFigure()
                        {
                            IsClosed = true,
                            StartPoint = bS,
                            Segments = segB
                        }
                    }
                };


                Geometry circC = new PathGeometry()
                {
                    Figures = new PathFigureCollection()
                    {
                        new PathFigure()
                        {
                            IsClosed = true,
                            StartPoint = cS,
                            Segments = segC
                        }
                    }
                };

//                int x = 3;
//                Point np = new Point(aS.X - x, aS.Y + 1);
//                var ofs = aR * (1 - gapScale);
//
//                Geometry rect = new RectangleGeometry()
//                {
//                    Rect = new Rect(np, new Size(x * 2, ofs))
//                };


                Geometry g1 = new CombinedGeometry()
                {
                    GeometryCombineMode = GeometryCombineMode.Xor,
                    Geometry1 = circA,
                    Geometry2 = circB
                };

                Geometry g2 = new CombinedGeometry()
                {
                    GeometryCombineMode = GeometryCombineMode.Xor,
                    Geometry1 = g1,
                    Geometry2 = circC
                };

//                Geometry g3 = new CombinedGeometry()
//                {
//                    GeometryCombineMode = GeometryCombineMode.Union,
//                    Geometry1 = g2,
//                    Geometry2 = rect
//                };

                Path segmentPath = new Path()
                {
                    StrokeLineJoin = PenLineJoin.Round,
                    Stroke = new SolidColorBrush() {Color = Colors.Black},
                    StrokeThickness = 0.0d,
                    Data = g2
                };

                return segmentPath.Data;
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }

        private Geometry MaskStartPointSegment(double radius, double gapScale, Point center)
        {
            try
            {
                double aR = radius;
                Point aS = GetCircumferencePoint(0, aR, center.X, center.Y);

                int x = 3;
                Point np = new Point(aS.X - x, aS.Y + 1);
                var ofs = aR * (1 - gapScale);

                Geometry rect = new RectangleGeometry()
                {
                    Rect = new Rect(np, new Size(x * 2, ofs))
                };

                Path segmentPath = new Path()
                {
                    StrokeLineJoin = PenLineJoin.Round,
                    Stroke = new SolidColorBrush() {Color = Colors.Black},
                    StrokeThickness = 0.0d,
                    Data = rect
                };

                return segmentPath.Data;
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }

        private Geometry MaskBackgroundSegment(double radius, double gapScale, Point center)
        {
            try
            {
                double aR = radius;

                Geometry cir = new EllipseGeometry()
                {
                    Center = center,
                    RadiusX = aR,
                    RadiusY = aR
                };

                Path segmentPath = new Path()
                {
                    StrokeLineJoin = PenLineJoin.Round,
                    Stroke = new SolidColorBrush() {Color = Colors.Black},
                    StrokeThickness = 0.0d,
                    Data = cir
                };

                return segmentPath.Data;
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }

        private Point GetCenter()
        {
            return new Point(Width / 2, Height / 2);
        }

        private PathGeometry CloneDeep(PathGeometry pathGeometry)
        {
            var newPathGeometry = new PathGeometry();
            foreach (var figure in pathGeometry.Figures)
            {
                var newFigure = new PathFigure();
                newFigure.StartPoint = figure.StartPoint;

                foreach (var segment in figure.Segments)
                {
                    var segmentAsPolyLineSegment = segment as PolyLineSegment;
                    if (segmentAsPolyLineSegment != null)
                    {
                        var newSegment = new PolyLineSegment();
                        foreach (var point in segmentAsPolyLineSegment.Points)
                        {
                            newSegment.Points.Add(point);
                        }
                        newFigure.Segments.Add(newSegment);
                    }

                    var segmentAsLineSegment = segment as LineSegment;
                    if (segmentAsLineSegment != null)
                    {
                        var newSegment = new LineSegment();
                        newSegment.Point = segmentAsLineSegment.Point;
                        newFigure.Segments.Add(newSegment);
                    }

                    var segmentAsArcSegment = segment as ArcSegment;
                    if (segmentAsArcSegment != null)
                    {
                        var newSegment = new ArcSegment();
                        newSegment.Point = segmentAsArcSegment.Point;
                        newSegment.SweepDirection = segmentAsArcSegment.SweepDirection;
                        newSegment.RotationAngle = segmentAsArcSegment.RotationAngle;
                        newSegment.IsLargeArc = segmentAsArcSegment.IsLargeArc;
                        newSegment.Size = segmentAsArcSegment.Size;
                        newFigure.Segments.Add(newSegment);
                    }
                }
                newPathGeometry.Figures.Add(newFigure);
            }
            return newPathGeometry;
        }

        private double ClientGetRadius()
        {
            double result;
            if (ClientHeight < ClientWidth)
            {
                result = ClientHeight / 2;
            }
            else
            {
                result = ClientWidth / 2;
            }

            return result;
        }

        private double GetRadius()
        {
            double result;
            if (Height < Width)
            {
                result = Height / 2;
            }
            else
            {
                result = Width / 2;
            }

            return result;
        }

        #endregion

        #region Protected Методы

        protected override void InternalOnApplyTemplate()
        {
            _slice = GetTemplateChild("Slice") as Path;
            RegisterMouseEvents(_slice);
        }

        protected override void DrawGeometry(bool withAnimation = true)
        {
            try
            {
                if (ClientWidth <= 0.0)
                {
                    return;
                }
                if (ClientHeight <= 0.0)
                {
                    return;
                }

                double mStartpercent = 0;
                double mEndpercent = AnimatedValue;

                Point center = GetCenter();

                double startAngle = 360 / 100.0 * mStartpercent;
                double endAngle = 360 / 100.0 * mEndpercent;
                double radius = ClientGetRadius();


                if (endAngle > 0)
                {
                    Geometry segmentPathData = LayoutSegment(startAngle, endAngle, radius, GapScale, center, true);
                    if (segmentPathData != null)
                    {
                        SetValue(GeometryProperty, CloneDeep(segmentPathData as PathGeometry));
                    }

                    Geometry segmentPathMasStartPoint = MaskStartPointSegment(radius, GapScale, center);
                    if (segmentPathData != null)
                    {
                        SetValue(MaskStartPointProperty, segmentPathMasStartPoint);
                    }
                }
                else
                {
                    Geometry = null;
                    MaskStartPoint = null;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion

        #region Fields

        private Path _slice;
        private readonly DispatcherTimer _timer;

        public static readonly DependencyProperty GapScaleProperty =
            DependencyProperty.Register(nameof(GapScale), typeof(double), typeof(RadialPiece),
                new PropertyMetadata(0.90, null));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(RadialPiece),
                new PropertyMetadata(0.0, UpdatePie));

        public static readonly DependencyProperty AnimatedValueProperty =
            DependencyProperty.Register(nameof(AnimatedValue), typeof(double), typeof(RadialPiece),
                new PropertyMetadata(0.0, OnAnimatedValueChanged));

        public static readonly DependencyProperty FormattedAnimatedValueProperty =
            DependencyProperty.Register(nameof(FormattedAnimatedValue), typeof(string), typeof(RadialPiece),
                new PropertyMetadata(null));

        public static readonly DependencyProperty GeometryProperty =
            DependencyProperty.Register(nameof(Geometry), typeof(Geometry), typeof(RadialPiece),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.Register(nameof(Mask), typeof(Geometry), typeof(RadialPiece),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MaskStartPointProperty =
            DependencyProperty.Register(nameof(MaskStartPoint), typeof(Geometry), typeof(RadialPiece),
                new PropertyMetadata(null));

        public static readonly DependencyProperty BackgroundGeometryProperty =
            DependencyProperty.Register(nameof(BackgroundGeometry), typeof(Geometry), typeof(RadialPiece),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectionGeometryProperty =
            DependencyProperty.Register(nameof(SelectionGeometry), typeof(Geometry), typeof(RadialPiece),
                new PropertyMetadata(null));

        public static readonly DependencyProperty MouseOverGeometryProperty =
            DependencyProperty.Register(nameof(MouseOverGeometry), typeof(Geometry), typeof(RadialPiece),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ProgressColorProperty =
            DependencyProperty.Register(nameof(ProgressColor), typeof(Brush), typeof(RadialPiece),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent),
                    (o, args) => { (o as RadialPiece)?.SetValue(ProgressColorAnimationProperty, args.NewValue); }));

        public static readonly DependencyProperty ProgressColorAnimationProperty =
            DependencyProperty.Register(nameof(ProgressColorAnimation), typeof(Brush), typeof(RadialPiece),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), null));

        public static readonly DependencyProperty MaskColorProperty =
            DependencyProperty.Register(nameof(MaskColor), typeof(Brush), typeof(RadialPiece),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent),
                    (o, args) =>
                    {
                        (o as RadialPiece)?.SetValue(MaskColorAnimationProperty, args.NewValue);
                    }));

        public static readonly DependencyProperty MaskColorAnimationProperty =
            DependencyProperty.Register(nameof(MaskColorAnimation), typeof(Brush), typeof(RadialPiece),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), null));

        public static readonly DependencyProperty ProgressBackgroundProperty =
            DependencyProperty.Register(nameof(ProgressBackground), typeof(Brush), typeof(RadialPiece),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent),
                    (o, args) => { (o as RadialPiece)?.SetValue(ProgressBackgroundAnimationProperty, args.NewValue); }));

        public static readonly DependencyProperty ProgressBackgroundAnimationProperty =
            DependencyProperty.Register(nameof(ProgressBackgroundAnimation), typeof(Brush), typeof(RadialPiece),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent), null));

        #endregion Fields
    }
}