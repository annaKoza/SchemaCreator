using SchemaCreator.Designer.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SchemaCreator.Designer.Controls
{
    public class ToolboxItem : ContentControl
    {
        private Point? dragStartPoint;
        
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            dragStartPoint = e.GetPosition(this);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed)
                dragStartPoint = null;

            if (!dragStartPoint.HasValue) return;
            var dataObject = new DragObject
            {
                DataContextType = DataContext?.GetType()
            };

            if (VisualTreeHelper.GetParent(this) is WrapPanel panel)
            {
                var scale = 1.5;
                dataObject.DesiredSize = new Size(panel.ItemWidth * scale, panel.ItemHeight * scale);
            }

            DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);
            e.Handled = true;
        }
    }
}