using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchemaCreator.Designer.Controls
{
    public class DesignerCanvas : Canvas
    {
        public Point SnapGridOffset
        {
            get => (Point)GetValue(SnapGridOffsetProperty);
            set => SetValue(SnapGridOffsetProperty, value);
        }

        private const double defaultTileSize = 10.0;
        private const int maxOffsetCheck = 10;
        public static readonly DependencyProperty SnapGridOffsetProperty =
            DependencyProperty.Register("SnapGridOffset",
                                        typeof(Point),
                                        typeof(DesignerCanvas),
                                        new FrameworkPropertyMetadata(new Point(1,
                                                                                1),
                                                                      FrameworkPropertyMetadataOptions.AffectsRender,
                                                                      new PropertyChangedCallback(OnIsSnapGidOffsetChanged)));

        private void SetOffset(Point point)
        {
            offset = point.Y;
            if (offset == _pointZero)
            {
                _divider = (int)(ActualHeight / defaultTileSize);
            }

            else if (offset < _pointZero)
            {
                var last = _pointZero;
                for (int i = 1; i < maxOffsetCheck; i++)
                {
                    if (offset > (_pointZero / i) && offset < last)
                    {
                        _divider = (int)(ActualHeight / (defaultTileSize * i));
                        last = (_pointZero / i);
                    }
                }
            }
            else if (offset > _pointZero)
            {
                for (int i = 1; i < maxOffsetCheck; i++)
                {
                    var last = _pointZero;
                    if (offset > _pointZero * (i + 2))
                    {
                        _divider = (int)(ActualHeight / (defaultTileSize / i));
                    }
                }
            }
        }

        private static void OnIsSnapGidOffsetChanged(DependencyObject d,
                                                     DependencyPropertyChangedEventArgs e)
        {
            if(!(d is DesignerCanvas canvas)) return;
            var value = (Point)e.NewValue;
            if(value.Y != 0.0 && value.X != 0.0)
                canvas.SetOffset(value);
        }

        public static readonly DependencyProperty IsSnapGidVisibleProperty =
            DependencyProperty.Register("IsSnapGidVisible",
                                        typeof(bool),
                                        typeof(DesignerCanvas),
                                        new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnIsSnapGidVisibleChanged)));

        private Pen darkPen;
        private Pen lightPen;
        
        private int _pointZero = 100;
        private int _divider;
        private double offset;

        static DesignerCanvas() => DefaultStyleKeyProperty.OverrideMetadata(
        typeof(DesignerCanvas),
        new FrameworkPropertyMetadata(typeof(DesignerCanvas)));

        public DesignerCanvas()
        {
            lightPen = new Pen(new SolidColorBrush(Colors.Red), 0.5);
            darkPen = new Pen(new SolidColorBrush(Colors.Green), 0.7);
            SnapsToDevicePixels = false;
            UseLayoutRounding = false;
        }

        private static void OnIsSnapGidVisibleChanged(DependencyObject d,
                                                      DependencyPropertyChangedEventArgs e)
        {
            if(!(d is DesignerCanvas canvas)) return;
            var value = (bool)e.NewValue;
            if(value)
                canvas.SetGridColor(Colors.Green, Colors.Green); else
                canvas.SetGridColor(Colors.Transparent, Colors.Transparent);
        }

        private void SetGridColor(Color lightColor, Color darkColor)
        {
            lightPen = new Pen(new SolidColorBrush(lightColor), 0.5 / offset / 100);
            darkPen = new Pen(new SolidColorBrush(darkColor), 1 / offset / 100);
        }

        protected override void OnRender(DrawingContext dc)
        {
            lightPen = new Pen(new SolidColorBrush(Colors.Red),
                               0.5 /
                (this.offset / 100));
            darkPen = new Pen(new SolidColorBrush(Colors.Green),
                              1 / (this.offset / 100));

            var offset = ActualHeight / _divider;

            double rows = ActualHeight;
            double columns = ActualWidth;
            int alternate;

            //Draw the horizontal lines
            var x = new Point(0, 0);
            var y = new Point(ActualWidth, 0);

            for(int i = 0; i <= rows / offset; i++)
            {
                alternate = i % 4 == 0 ? 0 : 1;
                dc.DrawLine(alternate == 0 ? lightPen : darkPen, x, y);
                x.Offset(0, offset);
                y.Offset(0, offset);
            }

            //Draw the vertical lines
            x = new Point(0, 0);
            y = new Point(0, ActualHeight);

            for(int i = 0; i <= columns / offset; i++)
            {
                alternate = i % 4 == 0 ? 0 : 1;
                dc.DrawLine(alternate == 0 ? lightPen : darkPen, x, y);
                x.Offset(offset, 0);
                y.Offset(offset, 0);
            }
        }

        public Size GetSnapGridTileSize() => new Size(ActualHeight / _divider,
                                                      ActualHeight /
            _divider);

        public bool IsSnapGidVisible
        {
            get => (bool)GetValue(IsSnapGidVisibleProperty);
            set => SetValue(IsSnapGidVisibleProperty, value);
        }
    }
}