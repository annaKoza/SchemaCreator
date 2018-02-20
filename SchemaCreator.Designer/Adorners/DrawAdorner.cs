using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using SchemaCreator.Designer.Controls;

namespace SchemaCreator.Designer.AttachedProperties
{
    internal class DrawAdorner : Adorner
    {
        private Canvas _itemsPanel;
        private Point? _selectionStartPoint;
        private ISelectionPanel _designerPanel;
        private Point? _endPoint;

        public DrawAdorner(Canvas itemsPanel, Point? selectionStartPoint, ISelectionPanel designerPanel) : base(itemsPanel)
        {
            _itemsPanel = itemsPanel;
            _selectionStartPoint = selectionStartPoint;
            _designerPanel = designerPanel;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                    CaptureMouse();

                _endPoint = e.GetPosition(this);
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured) ReleaseMouseCapture();
            }

            e.Handled = false;
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if (IsMouseCaptured) ReleaseMouseCapture();
            var adornerLayer = AdornerLayer.GetAdornerLayer(_itemsPanel);
            adornerLayer?.Remove(this);

           e.Handled = false;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // without a background the OnMouseMove event would not be fired!
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
            if (_selectionStartPoint.HasValue && _endPoint.HasValue) {
               dc.DrawLine(new Pen(new SolidColorBrush(Colors.Black), 2), _selectionStartPoint.Value, _endPoint.Value);
            }
        }

    }
}