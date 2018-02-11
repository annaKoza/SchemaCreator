using SchemaCreator.Designer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SchemaCreator.Designer.Controls
{
    public class ResizeThumb : Thumb
    {
        private DesignerItem _designerItem;
        private DesignerPanel _designerPanel;
        private double angle;
        private Point _transformOrigin;

        static ResizeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ResizeThumb), new FrameworkPropertyMetadata(typeof(ResizeThumb)));
        }

        public ResizeThumb()
        {
            DragDelta += ResizeThumb_DragDelta;
            Loaded += ResizeThumb_Loaded;
            DragStarted += ResizeThumb_DragStarted;
        }

        private IEnumerable<DesignerItem> _selectedDesignerItems;

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            _selectedDesignerItems = _designerPanel.SelectionService.SelectedItems.OfType<DesignerItem>();
        }

        private void ResizeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            _designerItem = DataContext as DesignerItem;
            _designerPanel = _designerItem.FindParent<DesignerPanel>();

            if (_designerItem == null || _designerPanel == null || !(_designerPanel is ISelectionPanel))
                throw new ArgumentException("Resize Thumb could not be loaded!");
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_designerItem.IsSelected)
            {
                double dragDeltaVertical, dragDeltaHorizontal;

                CalculateDragLimits(_selectedDesignerItems,
                                    out double minDeltaHorizontal, out double minDeltaVertical);

                foreach (var selectedItem in _selectedDesignerItems)
                {
                    _transformOrigin = selectedItem.RenderTransformOrigin;
                    angle = selectedItem.Angle * Math.PI / 180.0;

                    switch (VerticalAlignment)
                    {
                        case System.Windows.VerticalAlignment.Bottom:
                            dragDeltaVertical = Math.Min(-e.VerticalChange, minDeltaVertical);
                            Canvas.SetTop(selectedItem, Canvas.GetTop(selectedItem) + (_transformOrigin.Y * dragDeltaVertical * (1 - Math.Cos(-angle))));
                            Canvas.SetLeft(selectedItem, Canvas.GetLeft(selectedItem) - dragDeltaVertical * _transformOrigin.Y * Math.Sin(-angle));
                            selectedItem.Height -= dragDeltaVertical;
                            break;

                        case System.Windows.VerticalAlignment.Top:
                            dragDeltaVertical = Math.Min(e.VerticalChange, minDeltaVertical);
                            Canvas.SetTop(selectedItem, Canvas.GetTop(selectedItem) + dragDeltaVertical * Math.Cos(-angle) + (_transformOrigin.Y * dragDeltaVertical * (1 - Math.Cos(-angle))));
                            Canvas.SetLeft(selectedItem, Canvas.GetLeft(selectedItem) + dragDeltaVertical * Math.Sin(-angle) - (_transformOrigin.Y * dragDeltaVertical * Math.Sin(-angle)));
                            selectedItem.Height -= dragDeltaVertical;
                            break;

                        default:
                            break;
                    }

                    switch (HorizontalAlignment)
                    {
                        case System.Windows.HorizontalAlignment.Left:
                            dragDeltaHorizontal = Math.Min(e.HorizontalChange, minDeltaHorizontal);
                            Canvas.SetTop(selectedItem, Canvas.GetTop(selectedItem) + dragDeltaHorizontal * Math.Sin(angle) - _transformOrigin.X * dragDeltaHorizontal * Math.Sin(angle));
                            Canvas.SetLeft(selectedItem, Canvas.GetLeft(selectedItem) + dragDeltaHorizontal * Math.Cos(angle) + (_transformOrigin.X * dragDeltaHorizontal * (1 - Math.Cos(angle))));
                            selectedItem.Width -= dragDeltaHorizontal;
                            break;

                        case System.Windows.HorizontalAlignment.Right:
                            dragDeltaHorizontal = Math.Min(-e.HorizontalChange, minDeltaHorizontal);
                            Canvas.SetTop(selectedItem, Canvas.GetTop(selectedItem) - _transformOrigin.X * dragDeltaHorizontal * Math.Sin(angle));
                            Canvas.SetLeft(selectedItem, Canvas.GetLeft(selectedItem) + (dragDeltaHorizontal * _transformOrigin.X * (1 - Math.Cos(angle))));
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

        private void CalculateDragLimits(IEnumerable<DesignerItem> selectedItems,
            out double minDeltaHorizontal,
            out double minDeltaVertical)
        {
            minDeltaHorizontal = double.MaxValue;
            minDeltaVertical = double.MaxValue;

            foreach (DesignerItem item in selectedItems)
            {
                minDeltaVertical = Math.Min(minDeltaVertical, item.ActualHeight - item.MinHeight);
                minDeltaHorizontal = Math.Min(minDeltaHorizontal, item.ActualWidth - item.MinWidth);
            }
        }

        #endregion Helper methods
    }
}