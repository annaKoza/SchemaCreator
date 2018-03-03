using SchemaCreator.Designer.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SchemaCreator.Designer.Controls
{
    public class DesignerItem : ListBoxItem
    {
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register
            ("Angle",
                typeof(double),
                typeof(DesignerItem),
                new PropertyMetadata(0.0));

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }
    
    }
}