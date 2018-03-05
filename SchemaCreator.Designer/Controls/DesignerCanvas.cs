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

        public static readonly DependencyProperty SnapGridOffsetProperty =
            DependencyProperty.Register("SnapGridOffset",
                                        typeof(Point),
                                        typeof(DesignerCanvas),
                                        new FrameworkPropertyMetadata(new Point(5,
                                                                                5),
                                                                      FrameworkPropertyMetadataOptions.AffectsRender,
                                                                      new PropertyChangedCallback(OnIsSnapGidOffsetChanged)));

        private void SetOffset(Point point)
        {
            mod = point.Y;
            if(mod < _pointZero)
            {
                if(mod >= 75)
                    _divider = 64; else if(mod < 75 && mod >= 50)
                    _divider = 32; else
                    _divider = 16;
            } else if(mod > _pointZero)
            {
                for(int i = 1; i <= _maxOffset / _pointZero; i++)
                {
                    if(_pointZero * i <= mod && mod < _pointZero * (i + 1))
                        _divider = _pointZero * i;
                }
            } else if(mod == _pointZero)
                _divider = 128;
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

        private int _maxOffset = 500;
        private int _pointZero = 100;
        private int _divider = 128;
        private double mod;

        static DesignerCanvas() => DefaultStyleKeyProperty.OverrideMetadata(
        typeof(DesignerCanvas),
        new FrameworkPropertyMetadata(typeof(DesignerCanvas)));

        public DesignerCanvas()
        {
            lightPen = new Pen(new SolidColorBrush(Colors.Red), 0.5);
            darkPen = new Pen(new SolidColorBrush(Colors.Green), 0.7);
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
            lightPen = new Pen(new SolidColorBrush(lightColor), 0.5 / mod / 100);
            darkPen = new Pen(new SolidColorBrush(darkColor), 1 / mod / 100);
        }

        protected override void OnRender(DrawingContext dc)
        {
            lightPen = new Pen(new SolidColorBrush(Colors.Red),
                               0.5 /
                (mod / 100));
            darkPen = new Pen(new SolidColorBrush(Colors.Green),
                              1 / (mod / 100));

            var offset = ActualHeight / _divider;

            double rows = ActualHeight;
            double columns = ActualWidth;
            int alternate;

            //Draw the horizontal lines
            var x = new Point(0, 0.5);
            var y = new Point(ActualWidth, 0.5);

            for(int i = 0; i <= rows / offset; i++)
            {
                alternate = i % 4 == 0 ? 0 : 1;
                dc.DrawLine(alternate == 0 ? lightPen : darkPen, x, y);
                x.Offset(0, offset);
                y.Offset(0, offset);
            }

            //Draw the vertical lines
            x = new Point(0.5, 0);
            y = new Point(0.5, ActualHeight);

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