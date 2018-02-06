using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Services;
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
        static ResizeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ResizeThumb), new FrameworkPropertyMetadata(typeof(ResizeThumb)));
        }

        public ResizeThumb()
        {
            DragDelta += ResizeThumb_DragDelta;
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var designerItem = this.DataContext as DesignerItem;
            var designer = designerItem.FindParent<DesignerPanel>();

            var designerPanelWidth = designer.ActualWidth;
            var designerPanelHeight = designer.ActualHeight;

            if (designerItem == null || designer == null || !designerItem.IsSelected ||
                !(designer is ISelectionPanel)) return;

            var selectedDesignerItems = designer.SelectionService.SelectedItems.OfType<DesignerItem>();

            CalculateDragLimits(selectedDesignerItems, out var minLeft, out var minTop,
                out var minDeltaHorizontal, out var minDeltaVertical, out var maxTop, out var maxLeft);

            foreach (var item in selectedDesignerItems)
            {
                if (item == null) continue;
                double dragDeltaVertical;
                double scale;
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        var maxVerticalChange = designerPanelHeight - maxTop;
                        dragDeltaVertical = Math.Min(-e.VerticalChange, minDeltaVertical);
                        if (maxVerticalChange > e.VerticalChange)
                            scale = (item.ActualHeight - dragDeltaVertical) / item.ActualHeight;
                        else
                            scale = 1;
                        DragBottom(scale, item, designerPanelHeight);
                        break;

                    case VerticalAlignment.Top:
                        dragDeltaVertical = Math.Min(Math.Max(-minTop, e.VerticalChange), minDeltaVertical);
                        scale = (item.ActualHeight - dragDeltaVertical) / item.ActualHeight;
                        DragTop(scale, item);
                        break;

                    case VerticalAlignment.Center:
                        break;

                    case VerticalAlignment.Stretch:
                        break;
                }
                double dragDeltaHorizontal;

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        dragDeltaHorizontal = Math.Min(Math.Max(-minLeft, e.HorizontalChange), minDeltaHorizontal);
                        scale = (item.ActualWidth - dragDeltaHorizontal) / item.ActualWidth;
                        DragLeft(scale, item);
                        break;

                    case HorizontalAlignment.Right:
                        var maxHorizontalChange = designerPanelWidth - maxLeft;
                        dragDeltaHorizontal = Math.Min(-e.HorizontalChange, minDeltaHorizontal);
                        if (maxHorizontalChange > e.HorizontalChange)
                            scale = (item.ActualWidth - dragDeltaHorizontal) / item.ActualWidth;
                        else
                            scale = 1;
                        DragRight(scale, item, designerPanelWidth);
                        break;
                }
            }
            e.Handled = true;
        }

        #region Helper methods

        private void DragLeft(double scale, DesignerItem item)
        {
            var itemPositionWithWidth = Canvas.GetLeft(item) + item.Width;
            var itemPosition = Canvas.GetLeft(item);

            var delta = (itemPositionWithWidth - itemPosition) * (scale - 1);
            Canvas.SetLeft(item, itemPosition - delta);
            item.Width = item.ActualWidth * scale;
        }

        private void DragTop(double scale, DesignerItem item)
        {
            var itemPositionWithHeight = Canvas.GetTop(item) + item.Height;
            var itemPosition = Canvas.GetTop(item);

            var delta = (itemPositionWithHeight - itemPosition) * (scale - 1);
            Canvas.SetTop(item, itemPosition - delta);
            item.Height = item.ActualHeight * scale;
        }

        private void DragRight(double scale, DesignerItem item, double maxHorizontalPosition)
        {
            var itemPosition = Canvas.GetLeft(item);
            var itemPositionWithWidth = Canvas.GetLeft(item) + item.Width;

            Canvas.SetLeft(item, itemPosition);
            item.Width = item.ActualWidth * scale;
        }

        private void DragBottom(double scale, DesignerItem item, double maxVerticalPosition)
        {
            var itemPosition = Canvas.GetTop(item);
            var itemPositionWithHeight = Canvas.GetTop(item) + item.Height;
            var delta = (itemPositionWithHeight - itemPosition) * (scale - 1);

            Canvas.SetTop(item, itemPosition);
            item.Height = item.ActualHeight * scale;
        }

        private void CalculateDragLimits(IEnumerable<DesignerItem> selectedItems, 
            out double minLeft,
            out double minTop,
            out double minDeltaHorizontal, 
            out double minDeltaVertical,
            out double maxTop,
            out double maxLeft)
        {
            minLeft = double.MaxValue;
            minTop = double.MaxValue;
            maxLeft = double.MinValue;
            maxTop = double.MinValue;
            minDeltaHorizontal = double.MaxValue;
            minDeltaVertical = double.MaxValue;
            

            foreach (DesignerItem item in selectedItems)
            {
                var left = Canvas.GetLeft(item);
                var top = Canvas.GetTop(item);

                var right = left + item.ActualWidth;
                var bottom = top + item.ActualHeight;

                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                maxLeft = double.IsNaN(left) ? 0 : Math.Max(right, maxLeft);
                maxTop = double.IsNaN(top) ? 0 : Math.Max(bottom, maxTop);
             

                minDeltaVertical = Math.Min(minDeltaVertical, item.ActualHeight - item.MinHeight);
                minDeltaHorizontal = Math.Min(minDeltaHorizontal, item.ActualWidth - item.MinWidth);
            }
        }

        #endregion Helper methods
    }
}