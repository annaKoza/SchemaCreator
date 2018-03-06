using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SchemaCreator.Designer.Adorners
{
    public class SelectionAdorner : BaseAdorner
    {
        Point? _endPoint;
        public IDesignerViewModel DesignerPanel { get; set; }
        public Point? SelectionStartPoint { get; set; }
        public IBaseChooseAbleItem DrawableItem { get; set; }
        public Canvas ItemsPanel { get; set; }
        internal SelectionAdorner(Canvas itemsPanel, Point? selectionStartPoint, IDesignerViewModel designerPanel, IBaseChooseAbleItem drawableItem) : base(itemsPanel)
        {
            ItemsPanel = itemsPanel;
            DrawableItem = drawableItem;
            SelectionStartPoint = selectionStartPoint;
            DesignerPanel = designerPanel;
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
            }
            else if(IsMouseCaptured) ReleaseMouseCapture();

            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if(IsMouseCaptured) ReleaseMouseCapture();
            var adornerLayer = AdornerLayer.GetAdornerLayer(ItemsPanel);
            adornerLayer?.Remove(this);

            e.Handled = true;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawRectangle(Brushes.Transparent,
                                         null,
                                         new Rect(RenderSize));

            if(SelectionStartPoint.HasValue && _endPoint.HasValue)
                DrawableItem.DrawAdorner(drawingContext,
                                           SelectionStartPoint.Value,
                                           _endPoint.Value);
        }

        private void UpdateSelection()
        {
            DesignerPanel.SelectionService.ClearSelection();

            var rubberBand = new Rect(SelectionStartPoint.Value, _endPoint.Value);
            foreach(Control item in (ItemsPanel as Canvas).Children)
            {
                var itemRect = VisualTreeHelper.GetDescendantBounds(item);
                var itemBounds = item.TransformToAncestor(ItemsPanel)
                    .TransformBounds(itemRect);

                if(!rubberBand.Contains(itemBounds)) continue;
                var di = (item as DesignerItem).DataContext as ISelectable;
                DesignerPanel.SelectionService.AddToSelection(di);
            }
        }
    }
}