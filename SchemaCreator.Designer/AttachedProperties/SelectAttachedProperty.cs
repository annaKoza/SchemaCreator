using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using SchemaCreator.Designer.Adorners;
using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.Services;

namespace SchemaCreator.Designer.AttachedProperties
{
    public static class SelectAttachedProperty
    {
        public static readonly DependencyProperty SelectTargetProperty = DependencyProperty.RegisterAttached
        ("SelectTarget",
            typeof(Designer),
            typeof(SelectAttachedProperty),
            new FrameworkPropertyMetadata(OnTargetChanged));

        public static void SetSelectTarget(UIElement element, Designer value)
        {
            element.SetValue(SelectTargetProperty, value);
        }
        public static Designer GetSelectTarget(UIElement element)
        {
            return (Designer)element.GetValue(SelectTargetProperty);
        }

        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var designer = (e.NewValue as Designer);

            if (designer == null) return;
            if (e.OldValue != null)
            {
                designer.Loaded -= OnDesignerLoaded;
                designer.MouseLeftButtonDown -= OnDesignerStartSelect;
                designer.MouseMove -= OnDesignerContinueSelect;
            }
            designer.Loaded += OnDesignerLoaded;
            designer.MouseLeftButtonDown += OnDesignerStartSelect;
            designer.MouseMove += OnDesignerContinueSelect;

        }

        private static void OnDesignerContinueSelect(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                _rubberbandSelectionStartPoint = null;

            if (_rubberbandSelectionStartPoint.HasValue)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(_itemsPanel);
                if (adornerLayer != null)
                {
                    var adorner = new SelectionAdorner(_itemsPanel, _rubberbandSelectionStartPoint, _designerPanel);
                    adornerLayer.Add(adorner);
                }
            }
            e.Handled = true;
        }

        private static ISelectionPanel _designerPanel;
        private static Canvas _itemsPanel;
        private static Point? _rubberbandSelectionStartPoint;

        private static void OnDesignerLoaded(object sender, RoutedEventArgs e)
        {
            var designer = sender as Designer;
            var itemsPresenter = designer.GetVisualChild<ItemsPresenter>();
            var itemsControl = designer.GetVisualChild<ItemsControl>();
            _itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as Canvas;
            _designerPanel = itemsControl is ISelectionPanel
                ? itemsControl as ISelectionPanel
                : throw new ArgumentException("ItemsControl must implement ISelectionPanel interface");
        }

      
        private static void OnDesignerStartSelect(object sender, MouseButtonEventArgs e)
        {
            _rubberbandSelectionStartPoint = e.GetPosition(_itemsPanel);
            _designerPanel.SelectionService.ClearSelection();
            _itemsPanel.Focus();
            e.Handled = true;
        }
    }
}
