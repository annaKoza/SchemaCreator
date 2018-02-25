using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SchemaCreator.Designer.Adorners
{
    public class SelectionAdorner : Adorner
    {
        private Point? _startPoint;
        private Point? _endPoint;
        private ISelectionItem _selectionItem;
        private ISelectionPanel _designerPanel;
        private Canvas _designerCanvas;

        public SelectionAdorner(Canvas panel,
                                Point? dragStartPoint,
                                ISelectionPanel designerPanel,
                                ISelectionItem selectionItem) : base(panel)
        {
            _selectionItem = selectionItem;
            _designerCanvas = panel;
            _designerPanel = designerPanel;
            _startPoint = dragStartPoint;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                if(!IsMouseCaptured)
                    CaptureMouse();

                _endPoint = e.GetPosition(this);
                UpdateSelection();
                InvalidateVisual();
            } else
            {
                if(IsMouseCaptured) ReleaseMouseCapture();
            }

            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if(IsMouseCaptured) ReleaseMouseCapture();
            var adornerLayer = AdornerLayer.GetAdornerLayer(_designerCanvas);
            adornerLayer?.Remove(this);

            e.Handled = true;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawRectangle(Brushes.Transparent,
                                         null,
                                         new Rect(RenderSize));

            if(_startPoint.HasValue && _endPoint.HasValue)
                _selectionItem.DrawAdorner(drawingContext,
                                           _startPoint.Value,
                                           _endPoint.Value);
        }

        private void UpdateSelection()
        {
            _designerPanel.SelectionService.ClearSelection();

            var rubberBand = new Rect(_startPoint.Value, _endPoint.Value);
            foreach(Control item in _designerCanvas.Children)
            {
                var itemRect = VisualTreeHelper.GetDescendantBounds(item);
                var itemBounds = item.TransformToAncestor(_designerCanvas)
                    .TransformBounds(itemRect);

                if(!rubberBand.Contains(itemBounds)) continue;
                var di = (item as DesignerItem).DataContext as ISelectable;
                _designerPanel.SelectionService.AddToSelection(di);
            }
        }
    }
}