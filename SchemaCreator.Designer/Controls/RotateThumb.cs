using SchemaCreator.Designer.Adorners;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SchemaCreator.Designer.Controls
{
    internal class RotateThumb : Thumb
    {
        private Canvas _canvas;
        private DesignerItem _designerItem;
        private Adorner adorner;
        private Point centerPoint;
        private double initialAngle;
        private Vector startVector;

        static RotateThumb() => DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RotateThumb),
                new FrameworkPropertyMetadata(typeof(RotateThumb)));

        public RotateThumb()
        {
            DragDelta += OnDragDelta;
            DragStarted += OnDragStarted;
            DragCompleted += OnDragCompleted;
        }

        private void OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            if(adorner == null)
            {
                return;
            }

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(_canvas);
            adornerLayer?.Remove(adorner);

            adorner = null;
        }

        public void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            Point currentPoint = Mouse.GetPosition(_canvas);
            Vector deltaVector = Point.Subtract(currentPoint, centerPoint);

            double angle = Vector.AngleBetween(startVector, deltaVector);

            _designerItem.Angle = initialAngle + Math.Round(angle, 0);
            _designerItem.InvalidateMeasure();
        }

        public void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            _designerItem = DataContext as DesignerItem;

            _canvas = VisualTreeHelper.GetParent(_designerItem) as Canvas;

            if(_canvas != null)
            {
                centerPoint = _designerItem.TranslatePoint(
                    new Point(_designerItem.Width *
                    _designerItem.RenderTransformOrigin.X,
                              _designerItem.Height *
                    _designerItem.RenderTransformOrigin.Y),
                              _canvas);

                Point startPoint = Mouse.GetPosition(_canvas);
                startVector = Point.Subtract(startPoint, centerPoint);

                initialAngle = _designerItem.Angle;
            }
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(_canvas);
            if(adornerLayer != null)
            {
                adorner = new RotateAdorner(_designerItem);
                adornerLayer.Add(adorner);
            }
        }
    }
}