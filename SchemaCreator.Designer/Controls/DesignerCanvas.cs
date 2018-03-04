using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchemaCreator.Designer.Controls
{
    public class DesignerCanvas : Canvas
    {
        public Point SnapGridOffset
        {
            get { return (Point)GetValue(SnapGridOffsetProperty); }
            set { SetValue(SnapGridOffsetProperty, value); }
        }
        public static readonly DependencyProperty SnapGridOffsetProperty =
            DependencyProperty.Register("SnapGridOffset", typeof(Point), typeof(DesignerCanvas), new FrameworkPropertyMetadata(new Point(5,5), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnIsSnapGidOffsetChanged)));

        private int basexOffset = 5;
        private int baseyOffset = 5;
        private void SetOffset(Point point)
        {
            var modx = (int)point.X / 10 % 5;
            var mody = (int)point.Y / 10 % 5;

            xOffset = 55 - (int)point.X/10 - modx;
            yOffset = 55 - (int)point.Y/10 - mody;
        }
        private static void OnIsSnapGidOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DesignerCanvas canvas)) return;
            var value = (Point)e.NewValue;
            if (value.Y != 0.0 && value.X != 0.0)
                canvas.SetOffset(value);
        }

        public static readonly DependencyProperty IsSnapGidVisibleProperty =
            DependencyProperty.Register("IsSnapGidVisible", typeof(bool), typeof(DesignerCanvas), new FrameworkPropertyMetadata(
                true, 
                FrameworkPropertyMetadataOptions.AffectsRender, 
                new PropertyChangedCallback(OnIsSnapGidVisibleChanged)));
        private Pen darkPen;
        private Pen lightPen;
        private int xOffset = 5;
        private int yOffset = 5;

        static DesignerCanvas() => DefaultStyleKeyProperty.OverrideMetadata(
        typeof(DesignerCanvas),
        new FrameworkPropertyMetadata(typeof(DesignerCanvas)));

        public DesignerCanvas()
        {
            lightPen = new Pen(new SolidColorBrush(Colors.Red), 0.5);
            darkPen = new Pen(new SolidColorBrush(Colors.Green), 1);
        }

        private static void OnIsSnapGidVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DesignerCanvas canvas)) return;
            var value = (bool)e.NewValue;
            if (value)
                canvas.SetGridColor(Colors.Green, Colors.Green);
            else
                canvas.SetGridColor(Colors.Transparent, Colors.Transparent);
           
        }

        private void SetGridColor(Color lightColor, Color darkColor)
        {
            lightPen = new Pen(new SolidColorBrush(lightColor), 0.5);
            darkPen = new Pen(new SolidColorBrush(darkColor), 1);
        }

        protected override void OnRender(DrawingContext dc)
        {
            
            // TODO: yOffset and XOffset will depends on Zoom size;
            int rows = (int)(ActualHeight);
            int columns = (int)(ActualWidth);
            int alternate = yOffset == 5 ? yOffset : 1;
           
            int j = 0;
            //Draw the horizontal lines        
            Point x = new Point(0, 0.5);
            Point y = new Point(ActualWidth, 0.5);

            for (int i = 0; i <= rows / yOffset; i++, j++)
            {
                dc.DrawLine(j % alternate == 0 ? lightPen : darkPen, x, y);
                x.Offset(0, yOffset);
                y.Offset(0, yOffset);
            }

            j = 0;
            //Draw the vertical lines        
            x = new Point(0.5, 0);
            y = new Point(0.5, ActualHeight);

            for (int i = 0; i <= columns / xOffset; i++, j++)
            {
                dc.DrawLine(j % alternate == 0 ? lightPen : darkPen, x, y);
                x.Offset(xOffset, 0);
                y.Offset(xOffset, 0);
            }
        }

        public Size GetSnapGridTileSize()
        {
            return new Size(xOffset, yOffset);
        }
        
        public bool IsSnapGidVisible
        {
            get { return (bool)GetValue(IsSnapGidVisibleProperty); }
            set { SetValue(IsSnapGidVisibleProperty, value); }
        }
    }
}
