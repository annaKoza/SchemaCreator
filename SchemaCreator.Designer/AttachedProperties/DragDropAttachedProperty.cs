using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.UserControls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchemaCreator.Designer.AttachedProperties
{
    public static class DragDropAttachedProperty
    {
        public static readonly DependencyProperty DragDropTargetProperty = DependencyProperty.RegisterAttached
            ("DragDropTarget", typeof(Designer), typeof(DragDropAttachedProperty),
            new FrameworkPropertyMetadata(OnDragDropTargetChanged));

        private static void OnDragDropTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var designer = (e.NewValue as Designer);

            if (designer != null)
                designer.Drop += OnDesignerDrop;
        }

        private static  void OnDesignerDrop(object sender, DragEventArgs e)
        {
            ItemsPresenter itemsPresenter = (sender as Designer).GetVisualChild<ItemsPresenter>();
            Canvas itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as Canvas;

            var dataContext = (sender as Designer).DataContext;
            if (!(dataContext is IDesignerViewModel)) throw new ArgumentException("datacontext must implement IDesignerViewModel interface");

            if (e.Data.GetData(typeof(DragObject)) is DragObject dragObject && dragObject.DataContextType != null)
            {
                if (Activator.CreateInstance(dragObject.DataContextType) is BaseDesignerItemViewModel element)
                {
                    Point position = e.GetPosition(itemsPanel);

                    if (dragObject.DesiredSize.HasValue)
                    {
                        Size desiredSize = dragObject.DesiredSize.Value;
                        element.Width = desiredSize.Width;
                        element.Height = desiredSize.Height;

                        element.Left = Math.Max(0, position.X - element.Width / 2);
                        element.Top = Math.Max(0, position.Y - element.Height / 2);
                    }
                    else
                    {
                        element.Left = Math.Max(0, position.X);
                        element.Top = Math.Max(0, position.Y);
                    }
                    
                    (dataContext as IDesignerViewModel).AddItem(element);
                }
            }

            e.Handled = true;
        }

        public static void SetDragDropTarget(UIElement element, Designer value)
        {
            element.SetValue(DragDropTargetProperty, value);
        }
        public static Designer GetDragDropTarget(UIElement element)
        {
            return (Designer)element.GetValue(DragDropTargetProperty);
        }
    }
}
