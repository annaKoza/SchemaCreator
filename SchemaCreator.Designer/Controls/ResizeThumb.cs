using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
    //        DefaultStyleKeyProperty.OverrideMetadata(
    //            typeof(ResizeThumb), new FrameworkPropertyMetadata(typeof(ResizeThumb)));
        }

        public ResizeThumb()
        {
            DragDelta += ResizeThumb_DragDelta;
            Loaded += ResizeThumb_Loaded;
            DragStarted += ResizeThumb_DragStarted;
        }

        private IEnumerable<IDesignerItem> _selectedDesignerItems;

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            _selectedDesignerItems = ((_designerPanel.DataContext) as ISelectionPanel).SelectionService.SelectedItems.OfType<IDesignerItem>();
        }

        private void ResizeThumb_Loaded(object sender, RoutedEventArgs e)
        {
            _designerItem = DataContext as DesignerItem;
            _designerPanel = _designerItem.FindParent<DesignerPanel>();

            if (_designerItem == null || _designerPanel == null)
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
                    _transformOrigin = selectedItem.TransformOrigin;
                    angle = selectedItem.Angle * Math.PI / 180.0;

                    switch (VerticalAlignment)
                    {
                        case VerticalAlignment.Bottom:
                            dragDeltaVertical = Math.Min(-e.VerticalChange, minDeltaVertical);
                            selectedItem.Top += (_transformOrigin.Y * dragDeltaVertical * (1 - Math.Cos(-angle)));
                            selectedItem.Left -= dragDeltaVertical * _transformOrigin.Y * Math.Sin(-angle);
                            selectedItem.Height -= dragDeltaVertical;
                            break;

                        case VerticalAlignment.Top:
                            dragDeltaVertical = Math.Min(e.VerticalChange, minDeltaVertical);
                            selectedItem.Top = selectedItem.Top + dragDeltaVertical * Math.Cos(-angle) + (_transformOrigin.Y * dragDeltaVertical * (1 - Math.Cos(-angle)));
                            selectedItem.Left = selectedItem.Left + dragDeltaVertical * Math.Sin(-angle) - (_transformOrigin.Y * dragDeltaVertical * Math.Sin(-angle));
                            selectedItem.Height -= dragDeltaVertical;
                            break;

                        default:
                            break;
                    }

                    switch (HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            dragDeltaHorizontal = Math.Min(e.HorizontalChange, minDeltaHorizontal);
                            selectedItem.Top = selectedItem.Top + dragDeltaHorizontal * Math.Sin(angle) - _transformOrigin.X * dragDeltaHorizontal * Math.Sin(angle);
                            selectedItem.Left = selectedItem.Left + dragDeltaHorizontal * Math.Cos(angle) + (_transformOrigin.X * dragDeltaHorizontal * (1 - Math.Cos(angle)));
                            selectedItem.Width -= dragDeltaHorizontal;
                            break;

                        case HorizontalAlignment.Right:
                            dragDeltaHorizontal = Math.Min(-e.HorizontalChange, minDeltaHorizontal);
                            selectedItem.Top -= _transformOrigin.X * dragDeltaHorizontal * Math.Sin(angle);
                            selectedItem.Left += (dragDeltaHorizontal * _transformOrigin.X * (1 - Math.Cos(angle)));
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
                minDeltaVertical = Math.Min(minDeltaVertical, item.Height - item.MinHeight);
                minDeltaHorizontal = Math.Min(minDeltaHorizontal, item.Width - item.MinWidth);
            }
        }

        #endregion Helper methods
    }
}