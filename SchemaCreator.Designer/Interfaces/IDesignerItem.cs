using System.Windows;

namespace SchemaCreator.Designer.Interfaces
{
    public interface IDesignerItem : ISelectable
    {
        IDesignerViewModel Parent
        {
            get; set;
        }

        int ZIndex
        {
            get; set;
        }

        double MinWidth
        {
            get; set;
        }

        double MinHeight
        {
            get; set;
        }

        Point TransformOrigin
        {
            get; set;
        }

        double Angle
        {
            get; set;
        }

        double Top
        {
            get; set;
        }

        double Left
        {
            get; set;
        }

        double Width
        {
            get; set;
        }

        double Height
        {
            get; set;
        }
    }
}