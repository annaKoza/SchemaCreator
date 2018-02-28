using SchemaCreator.Designer.Adorners;
using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace SchemaCreator.Designer.Controls
{
    public class ResizeThumb : Thumb
    {
        private DesignerItem _designerItem;
        private DesignerPanel _designerPanel;
        private Canvas _canvas;
        private double _angle;
        private Point _transformOrigin;

        static ResizeThumb()
        {
        }

        public ResizeThumb()
        {
            DragDelta += ResizeThumb_DragDelta;
            Loaded += ResizeThumb_Loaded;
            DragStarted += ResizeThumb_DragStarted;
            DragCompleted += ResizeThumb_DragCompleted;
        }

        private void ResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (adorner == null)
            {
                return;
            }

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(_canvas);
            adornerLayer?.Remove(adorner);

            adorner = null;
        }

        private IEnumerable<IDesignerItem> _selectedDesignerItems;
        private SizeAdorner adorner;

        private void ResizeThumb_DragStarted(object sender,
                                             DragStartedEventArgs e)
        {
            _selectedDesignerItems =
            ((_designerPanel.DataContext) as ISelectionPanel).SelectionService.SelectedItems.OfType<IDesignerItem>();
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(_canvas);
            if (adornerLayer != null)
            {
                adorner = new SizeAdorner(_designerItem);
                adornerLayer.Add(adorner);
            }
        }

        private void ResizeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            _designerItem = DataContext as DesignerItem;
            _designerPanel = _designerItem.GetVisualParent<DesignerPanel>();
            _canvas = _designerItem.GetVisualParent<Canvas>() as Canvas;
            if (_designerItem == null || _designerPanel == null)
                throw new ArgumentException("Resize Thumb could not be loaded!");
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_designerItem.IsSelected)
            {
                double dragDeltaVertical, dragDeltaHorizontal;

                CalculateDragLimits(_selectedDesignerItems,
                                    out double minDeltaHorizontal,
                                    out double minDeltaVertical);

                foreach (var selectedItem in _selectedDesignerItems)
                {
                    _transformOrigin = selectedItem.TransformOrigin;
                    _angle = selectedItem.Angle * Math.PI / 180.0;

                    switch (VerticalAlignment)
                    {
                        case VerticalAlignment.Bottom:
                            dragDeltaVertical = Math.Min(-e.VerticalChange,
                                                         minDeltaVertical);
                            selectedItem.Top += (_transformOrigin.Y *
                                dragDeltaVertical *
                                (1 -
                                    Math.Cos(-_angle)));
                            selectedItem.Left -= dragDeltaVertical *
                                _transformOrigin.Y *
                                Math.Sin(-_angle);
                            selectedItem.Height -= dragDeltaVertical;
                            break;

                        case VerticalAlignment.Top:
                            dragDeltaVertical = Math.Min(e.VerticalChange,
                                                         minDeltaVertical);
                            selectedItem.Top = selectedItem.Top +
                                dragDeltaVertical *
                                Math.Cos(-_angle) +
                                (_transformOrigin.Y *
                                    dragDeltaVertical *
                                    (1 -
                                        Math.Cos(-_angle)));
                            selectedItem.Left = selectedItem.Left +
                                dragDeltaVertical *
                                Math.Sin(-_angle) -
                                (_transformOrigin.Y *
                                    dragDeltaVertical *
                                    Math.Sin(-_angle));
                            selectedItem.Height -= dragDeltaVertical;
                            break;

                        default:
                            break;
                    }

                    switch (HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            dragDeltaHorizontal = Math.Min(e.HorizontalChange,
                                                           minDeltaHorizontal);
                            selectedItem.Top = selectedItem.Top +
                                dragDeltaHorizontal *
                                Math.Sin(_angle) -
                                _transformOrigin.X *
                                dragDeltaHorizontal *
                                Math.Sin(_angle);
                            selectedItem.Left = selectedItem.Left +
                                dragDeltaHorizontal *
                                Math.Cos(_angle) +
                                (_transformOrigin.X *
                                    dragDeltaHorizontal *
                                    (1 -
                                        Math.Cos(_angle)));
                            selectedItem.Width -= dragDeltaHorizontal;
                            break;

                        case HorizontalAlignment.Right:
                            dragDeltaHorizontal = Math.Min(-e.HorizontalChange,
                                                           minDeltaHorizontal);
                            selectedItem.Top -= _transformOrigin.X *
                                dragDeltaHorizontal *
                                Math.Sin(_angle);
                            selectedItem.Left += (dragDeltaHorizontal *
                                _transformOrigin.X *
                                (1 -
                                    Math.Cos(_angle)));
                            selectedItem.Width -= dragDeltaHorizontal;
                            break;

                        default:
                            break;
                    }
                }
            }

            e.Handled = true;
        }

        #region Helper methods

        private void CalculateDragLimits(IEnumerable<IDesignerItem> selectedItems,
            out double minDeltaHorizontal,
            out double minDeltaVertical)
        {
            minDeltaHorizontal = double.MaxValue;
            minDeltaVertical = double.MaxValue;

            foreach (IDesignerItem item in selectedItems)
            {
                minDeltaVertical = Math.Min(minDeltaVertical,
                                            item.Height -
                    item.MinHeight);
                minDeltaHorizontal = Math.Min(minDeltaHorizontal,
                                              item.Width -
                    item.MinWidth);
            }
        }

        #endregion Helper methods
    }
}