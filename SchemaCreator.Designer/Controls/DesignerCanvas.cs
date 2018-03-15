using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchemaCreator.Designer.Controls
{
    public class Range
    {
        public Range(int leftSideValue, int rightSideValue)
        {
            LeftSideValue = leftSideValue;
            RightSideValue = rightSideValue;
        }

        public int LeftSideValue { get; }
        public int RightSideValue { get; }
        public bool IsValueInRange(int value)
        {
            if (value >= LeftSideValue && value <= RightSideValue) return true;
            return false;
        }
    }
    public class Ranges
    {
        public Ranges(int startPoint, int endPoint, int intervalsInRange)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            IntervalsInRange = intervalsInRange;
        }

        public int startPoint { get; set; } = int.MinValue;
        public int endPoint { get; set; } = int.MaxValue;
        public int IntervalsInRange { get; set; }

        public int GetRangeNumberOfValue(int value, List<Range> ranges)
        {
            var number = 1;
            for (int i = 0; i < ranges.Count; i++)
            {
                if (ranges[i].IsValueInRange(value))
                    return number;
                else number++;
            }
            return 0;
        }
        public List<Range> GetRangeList()
        {
            var list = new List<Range>();
            var points = endPoint - startPoint;
            var rest = points % IntervalsInRange;
            var interval = points / IntervalsInRange;
            int point = startPoint;
            for (int i = 0; i < IntervalsInRange; i++)
            {
                if (i == IntervalsInRange - 1) list.Add(new Range(point, point + interval + rest));
                else list.Add(new Range(point, point + interval-1));
                point = point + interval;
                
            }
            return list;
        }
    
    }
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
            _divider = (int)(ActualHeight / defaultTileSize * r.GetRangeNumberOfValue((int)offset, ranges));

          
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
        Ranges r;
        List<Range> ranges;
        public DesignerCanvas()
        {
            lightPen = new Pen(new SolidColorBrush(Colors.Red), 0.5);
            darkPen = new Pen(new SolidColorBrush(Colors.Green), 0.7);
            SnapsToDevicePixels = false;
            UseLayoutRounding = false;
            r = new Ranges(25, 1200, 10);
            ranges =  r.GetRangeList();
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
                                                      ActualHeight / _divider);

        public bool IsSnapGidVisible
        {
            get => (bool)GetValue(IsSnapGidVisibleProperty);
            set => SetValue(IsSnapGidVisibleProperty, value);
        }
    }
}