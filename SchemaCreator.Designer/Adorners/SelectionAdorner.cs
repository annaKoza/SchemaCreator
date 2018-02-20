using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Interfaces;

namespace SchemaCreator.Designer.Adorners
{
    public class SelectionAdorner : Adorner
    {
        private Point? _startPoint;
        private Point? _endPoint;
        private Pen _rubberbandPen;
        private ISelectionPanel _designerPanel;
        private Canvas _designerCanvas;

        public SelectionAdorner(Canvas panel, Point? dragStartPoint, ISelectionPanel designerPanel)
            : base(panel)
        {
            _designerCanvas = panel;
            _designerPanel = designerPanel;
            _startPoint = dragStartPoint;
            _rubberbandPen = new Pen(Brushes.LightSlateGray, 1)
            {
                DashStyle = new DashStyle(new double[] { 2 }, 1)
            };
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                    CaptureMouse();

                _endPoint = e.GetPosition(this);
                UpdateSelection();
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured) ReleaseMouseCapture();
            }

            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (IsMouseCaptured) ReleaseMouseCapture();
            var adornerLayer = AdornerLayer.GetAdornerLayer(_designerCanvas);
            adornerLayer?.Remove(this);

            e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // without a background the OnMouseMove event would not be fired!
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (_startPoint.HasValue && _endPoint.HasValue)
                dc.DrawRectangle(Brushes.Transparent, _rubberbandPen, new Rect(_startPoint.Value, _endPoint.Value));
        }

        private void UpdateSelection()
        {
            _designerPanel.SelectionService.ClearSelection();

            var rubberBand = new Rect(_startPoint.Value, _endPoint.Value);
            foreach (Control item in _designerCanvas.Children)
            {
                var itemRect = VisualTreeHelper.GetDescendantBounds(item);
                var itemBounds = item.TransformToAncestor(_designerCanvas).TransformBounds(itemRect);

                if (!rubberBand.Contains(itemBounds)) continue;
                var di = item as DesignerItem;
                _designerPanel.SelectionService.AddToSelection(di);
            }
        }
    }
}

