using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.UserControls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SchemaCreator.Designer.Controls
{
    public class DragThumb : Thumb
    {
        static DragThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragThumb),
                  new FrameworkPropertyMetadata(typeof(DragThumb)));
        }

        public DragThumb()
        {
            DragDelta += DragThumb_DragDelta;
        }

        private void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var designerItem = DataContext as DesignerItem;
            var designer = designerItem.FindParent<DesignerPanel>();

            if (!(designer is ISelectionPanel)) return;
            var maxLeft = designer.ActualWidth;
            var maxTop = designer.ActualHeight;

            var minLeft = double.MaxValue;
            var minTop = double.MaxValue;

            var designerItems = designer.SelectionService.SelectedItems.OfType<DesignerItem>();

            foreach (var item in designerItems)
            {
                var left = Canvas.GetLeft(item);
                var top = Canvas.GetTop(item);

                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);
            }

            var deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
            var deltaVertical = Math.Max(-minTop, e.VerticalChange);

            foreach (var item in designerItems)
            {
                var left = Canvas.GetLeft(item);
                var top = Canvas.GetTop(item);

                if (double.IsNaN(left)) left = 0;
                if (double.IsNaN(top)) top = 0;

                if (left + deltaHorizontal + item.Width >= maxLeft) deltaHorizontal = 0;
                if (top + deltaVertical + item.Height >= maxTop) deltaVertical = 0;
            }

            foreach (var item in designerItems)
            {
                var left = Canvas.GetLeft(item);
                var top = Canvas.GetTop(item);

                if (double.IsNaN(left)) left = 0;
                if (double.IsNaN(top)) top = 0;

                Canvas.SetLeft(item, left + deltaHorizontal);
                Canvas.SetTop(item, top + deltaVertical);
            }

            designer.InvalidateMeasure();
            e.Handled = true;
        }

    
    }
}