using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace SchemaCreator.Designer.Controls
{
    internal class RotateThumb : Thumb
    {
        private DesignerItem designerItem;
        private Canvas canvas;
        private Point centerPoint;
        private Vector startVector;
        private double initialAngle;

        static RotateThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RotateThumb), new FrameworkPropertyMetadata(typeof(RotateThumb)));
        }

        public void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            designerItem = DataContext as DesignerItem;

            if (designerItem != null)
            {
                canvas = VisualTreeHelper.GetParent(designerItem) as Canvas;

                if (canvas != null)
                {
                    centerPoint = designerItem.TranslatePoint(
                        new Point(designerItem.Width * designerItem.RenderTransformOrigin.X,
                                  designerItem.Height * designerItem.RenderTransformOrigin.Y),
                                  canvas);

                    Point startPoint = Mouse.GetPosition(canvas);
                    startVector = Point.Subtract(startPoint, centerPoint);

                    initialAngle = designerItem.Angle;
                }
            }
        }

        public void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            Point currentPoint = Mouse.GetPosition(canvas);
            Vector deltaVector = Point.Subtract(currentPoint, centerPoint);

            double angle = Vector.AngleBetween(startVector, deltaVector);

            designerItem.Angle = initialAngle + Math.Round(angle, 0);
            designerItem.InvalidateMeasure();
        }

        public RotateThumb()
        {
            DragDelta += OnDragDelta;
            DragStarted += OnDragStarted;
        }
    }
}