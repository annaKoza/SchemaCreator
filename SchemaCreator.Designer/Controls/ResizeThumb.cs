using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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

            if (designerItem == null || designer == null || !designerItem.IsSelected ||
                !(designer is ISelectionPanel)) return;

            var selectedDesignerItems = designer.SelectionService.SelectedItems.OfType<DesignerItem>();

            CalculateDragLimits(selectedDesignerItems, out var minLeft, out var minTop,
                out var minDeltaHorizontal, out var minDeltaVertical);

            foreach (var item in selectedDesignerItems)
            {
                if (item == null) continue;
                double dragDeltaVertical;
                double scale;
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        dragDeltaVertical = Math.Min(-e.VerticalChange, minDeltaVertical);
                        scale = (item.ActualHeight - dragDeltaVertical) / item.ActualHeight;
                        DragBottom(scale, item, designer.SelectionService);
                        break;
                    case VerticalAlignment.Top:
                        dragDeltaVertical = Math.Min(Math.Max(-minTop, e.VerticalChange), minDeltaVertical);
                        scale = (item.ActualHeight - dragDeltaVertical) / item.ActualHeight;
                        DragTop(scale, item, designer.SelectionService);
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
                        DragLeft(scale, item, designer.SelectionService);
                        break;
                    case HorizontalAlignment.Right:
                        dragDeltaHorizontal = Math.Min(-e.HorizontalChange, minDeltaHorizontal);
                        scale = (item.ActualWidth - dragDeltaHorizontal) / item.ActualWidth;
                        DragRight(scale, item, designer.SelectionService);
                        break;
                }
            }
            e.Handled = true;
        }

        #region Helper methods

        private void DragLeft(double scale, DesignerItem item, SelectionService selectionService)
        {
            IEnumerable<DesignerItem> groupItems = selectionService.SelectedItems.Cast<DesignerItem>();

            var groupLeft = Canvas.GetLeft(item) + item.Width;
            foreach (var groupItem in groupItems)
            {
                var groupItemLeft = Canvas.GetLeft(groupItem);
                var delta = (groupLeft - groupItemLeft) * (scale - 1);
                Canvas.SetLeft(groupItem, groupItemLeft - delta);
                groupItem.Width = groupItem.ActualWidth * scale;
            }
        }

        private void DragTop(double scale, DesignerItem item, SelectionService selectionService)
        {
            IEnumerable<DesignerItem> groupItems = selectionService.SelectedItems.Cast<DesignerItem>();
            var groupBottom = Canvas.GetTop(item) + item.Height;
            foreach (var groupItem in groupItems)
            {
                var groupItemTop = Canvas.GetTop(groupItem);
                var delta = (groupBottom - groupItemTop) * (scale - 1);
                Canvas.SetTop(groupItem, groupItemTop - delta);
                groupItem.Height = groupItem.ActualHeight * scale;
            }
        }

        private void DragRight(double scale, DesignerItem item, SelectionService selectionService)
        {
            IEnumerable<DesignerItem> groupItems = selectionService.SelectedItems.Cast<DesignerItem>();

            var groupLeft = Canvas.GetLeft(item);
            foreach (var groupItem in groupItems)
            {
                var groupItemLeft = Canvas.GetLeft(groupItem);
                var delta = (groupItemLeft - groupLeft) * (scale - 1);

                Canvas.SetLeft(groupItem, groupItemLeft + delta);
                groupItem.Width = groupItem.ActualWidth * scale;
            }
        }

        private void DragBottom(double scale, DesignerItem item, SelectionService selectionService)
        {
            IEnumerable<DesignerItem> groupItems = selectionService.SelectedItems.Cast<DesignerItem>();
            var groupTop = Canvas.GetTop(item);
            foreach (var groupItem in groupItems)
            {
                var groupItemTop = Canvas.GetTop(groupItem);
                var delta = (groupItemTop - groupTop) * (scale - 1);

                Canvas.SetTop(groupItem, groupItemTop + delta);
                groupItem.Height = groupItem.ActualHeight * scale;
            }
        }

        private void CalculateDragLimits(IEnumerable<DesignerItem> selectedItems, out double minLeft, out double minTop, out double minDeltaHorizontal, out double minDeltaVertical)
        {
            minLeft = double.MaxValue;
            minTop = double.MaxValue;
            minDeltaHorizontal = double.MaxValue;
            minDeltaVertical = double.MaxValue;

            // drag limits are set by these parameters: canvas top, canvas left, minHeight, minWidth
            // calculate min value for each parameter for each item
            foreach (DesignerItem item in selectedItems)
            {
                var left = Canvas.GetLeft(item);
                var top = Canvas.GetTop(item);

                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                minDeltaVertical = Math.Min(minDeltaVertical, item.ActualHeight - item.MinHeight);
                minDeltaHorizontal = Math.Min(minDeltaHorizontal, item.ActualWidth - item.MinWidth);
            }
        }

        #endregion
    }
}

