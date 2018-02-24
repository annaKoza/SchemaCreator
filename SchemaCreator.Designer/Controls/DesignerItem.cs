using SchemaCreator.Designer.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace SchemaCreator.Designer.Controls
{
    [TemplatePart(Name = "PART_DragThumb", Type = typeof(DragThumb))]
    [TemplatePart(Name = "PART_ResizeDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    public class DesignerItem : ContentControl, ISelectable
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

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(DesignerItem), new PropertyMetadata(false));
    }
}