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
    public static class DrawAttachedProperty
    {
        public static readonly DependencyProperty DrawTargetProperty = DependencyProperty.RegisterAttached
            ("DrawTarget",
            typeof(Designer),
            typeof(DrawAttachedProperty),
            new FrameworkPropertyMetadata(OnTargetChanged));

        public static void SetDrawTarget(UIElement element, Designer value) => element.SetValue(DrawTargetProperty,
                                                                                                value);

        public static Designer GetDrawTarget(UIElement element) => (Designer)element.GetValue(DrawTargetProperty);

        private static void OnTargetChanged(DependencyObject d,
                                            DependencyPropertyChangedEventArgs e)
        {
            var designer = (e.NewValue as Designer);

            if(designer == null) return;
            if(e.OldValue != null)
            {
                designer.Loaded -= OnDesignerLoaded;
                designer.MouseLeftButtonDown -= OnDesignerStartDraw;
                designer.MouseMove -= OnDesignerContinueDraw;
            }
            designer.Loaded += OnDesignerLoaded;
            designer.MouseLeftButtonDown += OnDesignerStartDraw;
            designer.MouseMove += OnDesignerContinueDraw;
        }

        private static void OnDesignerContinueDraw(object sender,
                                                   MouseEventArgs e)
        {
            if(_drawableInstance ==
                null ||
                _selectedItemType !=
                SelectedItemType.DrawItem) return;

            if(e.LeftButton != MouseButtonState.Pressed)
                _selectionStartPoint = null;

            if(_selectionStartPoint.HasValue)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(_itemsPanel);
                if(adornerLayer != null)
                {
                    var adorner = new DrawAdorner(_itemsPanel,
                                                  _selectionStartPoint,
                                                  _designerViewModel,
                                                  _drawableInstance);
                    adornerLayer.Add(adorner);
                }
            }
            e.Handled = true;
        }

        private static IDesignerViewModel _designerViewModel;
        private static Canvas _itemsPanel;

        private static void OnDesignerLoaded(object sender, RoutedEventArgs e)
        {
            var itemsPresenter = (sender as Designer).GetVisualChild<ItemsPresenter>();
            _itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as Canvas;
            var dataContext = (sender as Designer).DataContext;
            _designerViewModel = dataContext as IDesignerViewModel ??
                throw new ArgumentException("datacontext must implement IDesignerViewModel interface");
        }

        private static IDrawableItem _drawableInstance;
        private static Point? _selectionStartPoint;
        private static SelectedItemType _selectedItemType;

        private static void OnDesignerStartDraw(object sender,
                                                MouseButtonEventArgs e)
        {
            _selectedItemType = _designerViewModel.ItemToDraw.SelectedItemType;
            if(_selectedItemType != SelectedItemType.DrawItem) return;

            _drawableInstance = Activator.CreateInstance(_designerViewModel.ItemToDraw.GetType()) as IDrawableItem;
            if(_drawableInstance == null) return;

            _selectionStartPoint = e.GetPosition(_itemsPanel);
            _drawableInstance.X1 = _selectionStartPoint.Value.X;
            _drawableInstance.Y1 = _selectionStartPoint.Value.Y;
            e.Handled = true;
        }
    }
}