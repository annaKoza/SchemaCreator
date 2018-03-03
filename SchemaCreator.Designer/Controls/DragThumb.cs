using SchemaCreator.Designer.Common.Extensions;
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
        private DesignerCanvas _designerCanvas;
        private DesignerPanel _designer;
        private RotateTransform _rotateTransform;
        private DesignerItem _designerItem;
        private IEnumerable<BaseDesignerItemViewModel> _selectedItems;
        private Size _gridSize;
        
        static DragThumb() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DragThumb),
                  new FrameworkPropertyMetadata(typeof(DragThumb)));

        public DragThumb()
        {
            Loaded += DragThumb_Loaded;
            DragDelta += DragThumb_DragDelta;
            DragStarted += DragThumb_DragStarted;
            MouseLeftButtonDown += OnLeftButtonDown;
        }

        private void OnLeftButtonDown(object sender, MouseButtonEventArgs e) => e.Handled =
            true;

        private void DragThumb_DragStarted(object sender,
                                           DragStartedEventArgs e)
        {
            _gridSize = _designerCanvas.GetSnapGridTileSize();
            _selectedItems = ((_designer.DataContext) as DesignerViewModel).SelectionService.SelectedItems.OfType<BaseDesignerItemViewModel>();
            _rotateTransform = _designerItem.RenderTransform as RotateTransform;
        }

        private void DragThumb_Loaded(object sender, RoutedEventArgs e)
        {
            _designerItem = DataContext as DesignerItem;
            _designer = _designerItem.GetVisualParent<DesignerPanel>();
            _designerCanvas = _designerItem.GetVisualParent<DesignerCanvas>();
        }

        private void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!_designerItem.IsSelected) return;
            MoveItem(e);
            e.Handled = true;
        }

        private void MoveItem(DragDeltaEventArgs e)
        {
            foreach (var designerItem in _selectedItems)
            {
                var dragDelta = new Point(e.HorizontalChange, e.VerticalChange);
                if (_rotateTransform != null)
                {
                    dragDelta = _rotateTransform.Transform(dragDelta);
                }

                if (_designer.SnapItemToGrid)
                {
                    double xSnapPosition = designerItem.Left.NearestFactor(_gridSize.Width) + dragDelta.X.NearestFactor(_gridSize.Width);
                    double ySnapPosition = designerItem.Top.NearestFactor(_gridSize.Width) + dragDelta.Y.NearestFactor(_gridSize.Height);

                    designerItem.Left = xSnapPosition;
                    designerItem.Top = ySnapPosition;
                }
                else
                {
                    designerItem.Left += dragDelta.X;
                    designerItem.Top += dragDelta.Y;
                }
            }
        }
    }
}