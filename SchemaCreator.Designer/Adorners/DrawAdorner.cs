using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SchemaCreator.Designer.AttachedProperties
{
    internal class DrawAdorner : Adorner
    {
        private Canvas _itemsPanel;
        private Point? _selectionStartPoint;
        private IDesignerViewModel _designerPanel;
        private Point? _endPoint;
        private IDrawableItem _drawableInstance;

        public DrawAdorner(Canvas itemsPanel, Point? selectionStartPoint, IDesignerViewModel designerPanel, IDrawableItem drawableItem) : base(itemsPanel)
        {
            _itemsPanel = itemsPanel;
            _drawableInstance = drawableItem;
            _selectionStartPoint = selectionStartPoint;
            _designerPanel = designerPanel;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                    CaptureMouse();
                var currentPosition = e.GetPosition(this);

                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    if (Math.Abs(currentPosition.X - _selectionStartPoint.Value.X) < Math.Abs(currentPosition.Y - _selectionStartPoint.Value.Y))
                        currentPosition.X = _selectionStartPoint.Value.X;
                    else
                        currentPosition.Y = _selectionStartPoint.Value.Y;
                }

                _endPoint = currentPosition;
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured) ReleaseMouseCapture();
            }

            e.Handled = true;
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if (IsMouseCaptured) ReleaseMouseCapture();
            var adornerLayer = AdornerLayer.GetAdornerLayer(_itemsPanel);
            adornerLayer?.Remove(this);

            if (_drawableInstance == null) return;

            _drawableInstance.Y2 = _endPoint.Value.Y;
            _drawableInstance.X2 = _endPoint.Value.X;

            _drawableInstance.Left = Math.Min(_drawableInstance.X1, _drawableInstance.X2);
            _drawableInstance.Top = Math.Min(_drawableInstance.Y1, _drawableInstance.Y2);

            var width = Math.Abs(_drawableInstance.X1 - _drawableInstance.X2);
            var height = Math.Abs(_drawableInstance.Y1 - _drawableInstance.Y2);

            CreateDrawableInstance(width, height);
            _designerPanel.AddItem(_drawableInstance);
            _designerPanel.SelectionService.SelectItem(_drawableInstance);
            
            e.Handled = true;
        }

        private void CreateDrawableInstance(double width, double height)
        {
            _drawableInstance.Width = width;
            _drawableInstance.Height = height;
            _drawableInstance.MinWidth = 10;
            _drawableInstance.MinHeight = 10;
            _drawableInstance.TransformOrigin = new Point(0.5, 0.5);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
            if (_selectionStartPoint.HasValue && _endPoint.HasValue)
            {
                dc.DrawLine(new Pen(new SolidColorBrush(Colors.Black), 2), _selectionStartPoint.Value, _endPoint.Value);
            }
        }
    }
}