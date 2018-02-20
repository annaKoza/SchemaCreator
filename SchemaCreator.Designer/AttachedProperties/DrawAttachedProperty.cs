using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Interfaces;

namespace SchemaCreator.Designer.AttachedProperties
{
    public static class DrawAttachedProperty
    {
        public static readonly DependencyProperty DrawTargetProperty = DependencyProperty.RegisterAttached
            ("DrawTarget", 
            typeof(Designer), 
            typeof(DrawAttachedProperty), 
            new FrameworkPropertyMetadata(OnTargetChanged));

        public static void SetDrawTarget(UIElement element, Designer value)
        {
            element.SetValue(DrawTargetProperty, value);
        }
        public static Designer GetDrawTarget(UIElement element)
        {
            return (Designer)element.GetValue(DrawTargetProperty);
        }

        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var designer = (e.NewValue as Designer);

            if (designer == null) return;
            if (e.OldValue != null)
            {
                designer.Loaded -= OnDesignerLoaded;
                designer.MouseLeftButtonDown -= OnDesignerStartDraw;
                designer.MouseLeftButtonUp -= OnDesignerStopDraw;
            }
            designer.Loaded += OnDesignerLoaded;
            designer.MouseLeftButtonDown += OnDesignerStartDraw;
            designer.MouseLeftButtonUp += OnDesignerStopDraw;
        }

        private static IDesignerViewModel _designerViewModel;
        private static Canvas _itemsPanel;
        private static void OnDesignerLoaded(object sender, RoutedEventArgs e)
        {
            var itemsPresenter = (sender as Designer).GetVisualChild<ItemsPresenter>();
            _itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 0) as Canvas;

            var dataContext = (sender as Designer).DataContext;
            _designerViewModel = dataContext as IDesignerViewModel ?? throw new ArgumentException("datacontext must implement IDesignerViewModel interface");
        }

        private static void OnDesignerStopDraw(object sender, MouseButtonEventArgs e)
        {
            if(_drawableInstance == null || _designerViewModel.ItemToDraw == null) return;
            
            var position = e.GetPosition(_itemsPanel);
            _drawableInstance.Y2 = position.Y;
            _drawableInstance.X2 = position.X;
            
            _drawableInstance.Left = Math.Min(_drawableInstance.X1, _drawableInstance.X2);
            _drawableInstance.Top = Math.Min(_drawableInstance.Y1, _drawableInstance.Y2);

            var width = Math.Abs(_drawableInstance.X1 - _drawableInstance.X2);
            var height = Math.Abs(_drawableInstance.Y1 - _drawableInstance.Y2);

            _drawableInstance.Width = width;
            _drawableInstance.Height = height;

            _designerViewModel.AddItem(_drawableInstance);
            _designerViewModel.ItemToDraw = null;
            e.Handled = true;
        }

        private static IDrawableItem _drawableInstance;
        private static void OnDesignerStartDraw(object sender, MouseButtonEventArgs e)
        {
            if (_designerViewModel.ItemToDraw == null) return;
            _drawableInstance = Activator.CreateInstance(_designerViewModel.ItemToDraw.GetType()) as IDrawableItem;
            if(_drawableInstance == null) return;
            
            var position = e.GetPosition(_itemsPanel);
            _drawableInstance.X1 = position.X;
            _drawableInstance.Y1 = position.Y;
            e.Handled = false;
        }
        
    }
}
