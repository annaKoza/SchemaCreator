using SchemaCreator.Designer.Adorners;
using SchemaCreator.Designer.Common;
using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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
            if (_selectedItemType != SelectedItemType.SelectItem) return;

            if (e.LeftButton != MouseButtonState.Pressed)
                _rubberbandSelectionStartPoint = null;

            if (_rubberbandSelectionStartPoint.HasValue)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(_itemsPanel);
                if (adornerLayer != null)
                {
                    var adorner = new SelectionAdorner(_itemsPanel, _rubberbandSelectionStartPoint, _designerVewModel, _designerVewModel.ItemToDraw as ISelectionItem);
                    adornerLayer.Add(adorner);
                }
            }
            e.Handled = true;
        }

        private static Canvas _itemsPanel;
        private static Point? _rubberbandSelectionStartPoint;
        private static IDesignerViewModel _designerVewModel;

        private static void OnDesignerLoaded(object sender, RoutedEventArgs e)
        {
            var designer = sender as Designer;
            _designerVewModel = designer.DataContext is IDesignerViewModel
                ? designer.DataContext as IDesignerViewModel
                : throw new ArgumentException("Designers datacontext must be IDesignerViewModel");
            var itemsPresenter = designer.GetVisualChild<ItemsPresenter>();
            var itemsControl = designer.GetVisualChild<ItemsControl>();
            _itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as Canvas;
        }

        private static SelectedItemType _selectedItemType;

        private static void OnDesignerStartSelect(object sender, MouseButtonEventArgs e)
        {
            _selectedItemType = _designerVewModel.ItemToDraw.SelectedItemType;
            if (_selectedItemType != SelectedItemType.SelectItem) return;

            _rubberbandSelectionStartPoint = e.GetPosition(_itemsPanel);
            _designerVewModel.SelectionService.ClearSelection();
            _itemsPanel.Focus();
            e.Handled = true;
        }
    }
}