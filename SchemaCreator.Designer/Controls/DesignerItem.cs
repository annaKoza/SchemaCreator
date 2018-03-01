using SchemaCreator.Designer.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SchemaCreator.Designer.Controls
{
    public class DesignerItem : ListBoxItem
    { 
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