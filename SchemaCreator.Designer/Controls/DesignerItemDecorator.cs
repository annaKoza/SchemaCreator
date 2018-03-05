using SchemaCreator.Designer.Adorners;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SchemaCreator.Designer.Controls
{
    public class DesignerItemDecorator : Control
    {
        public static readonly DependencyProperty ShowDecoratorProperty =
            DependencyProperty.Register("ShowDecorator",
                                        typeof(bool),
                                        typeof(DesignerItemDecorator),
            new FrameworkPropertyMetadata(false,
                                          new PropertyChangedCallback(ShowDecoratorProperty_Changed)));

        private Adorner adorner;

        public DesignerItemDecorator() => Unloaded +=
            new RoutedEventHandler(DesignerItemDecorator_Unloaded);

        private void DesignerItemDecorator_Unloaded(object sender,
                                                    RoutedEventArgs e)
        {
            if(adorner == null) return;

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
            adornerLayer?.Remove(adorner);
            adorner = null;
        }

        private void HideAdorner()
        {
            if(adorner != null)
            {
                adorner.Visibility = Visibility.Hidden;
            }
        }

        private void ShowAdorner()
        {
            if(adorner == null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);

                if(adornerLayer == null) return;

                var designerItem = DataContext as ContentControl;
                adorner = new ResizeRotateAdorner(designerItem);
                adornerLayer.Add(adorner);

                adorner.Visibility = ShowDecorator
                    ? Visibility.Visible
                    : Visibility.Hidden;
            } else
            {
                adorner.Visibility = Visibility.Visible;
            }
        }

        private static void ShowDecoratorProperty_Changed(DependencyObject d,
                                                          DependencyPropertyChangedEventArgs e)
        {
            var decorator = (DesignerItemDecorator)d;
            var showDecorator = (bool)e.NewValue;

            if(showDecorator)
            {
                decorator.ShowAdorner();
                return;
            }
            decorator.HideAdorner();
        }

        public bool ShowDecorator
        {
            get => (bool)GetValue(ShowDecoratorProperty);
            set => SetValue(ShowDecoratorProperty, value);
        }
    }
}