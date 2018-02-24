using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.UserControls;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace SchemaCreator.Designer.Controls
{
    public class DragThumb : Thumb
    {
        private DesignerPanel _designer;
        private RotateTransform _rotateTransform;
        private DesignerItem _designerItem;
        private IEnumerable<BaseDesignerItemViewModel> _selectedItems;

        static DragThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragThumb),
                  new FrameworkPropertyMetadata(typeof(DragThumb)));
        }

        public DragThumb()
        {
            Loaded += DragThumb_Loaded;
            DragDelta += DragThumb_DragDelta;
            DragStarted += DragThumb_DragStarted;
            MouseLeftButtonDown += OnLeftButtonDown;
        }

        private void OnLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void DragThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            _selectedItems = ((_designer.DataContext) as DesignerViewModel).SelectionService.SelectedItems.OfType<BaseDesignerItemViewModel>();
            _rotateTransform = _designerItem.RenderTransform as RotateTransform;
        }

        private void DragThumb_Loaded(object sender, RoutedEventArgs e)
        {
            _designerItem = DataContext as DesignerItem;
            _designer = _designerItem.FindParent<DesignerPanel>();
        }

        private void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!_designerItem.IsSelected) return;
            foreach (var designerItem in _selectedItems)
            {
                var dragDelta = new Point(e.HorizontalChange, e.VerticalChange);
                if (_rotateTransform != null)
                {
                    dragDelta = _rotateTransform.Transform(dragDelta);
                }

                designerItem.Left += dragDelta.X;
                designerItem.Top += dragDelta.Y;
            }
            e.Handled = true;
        }
    }
}