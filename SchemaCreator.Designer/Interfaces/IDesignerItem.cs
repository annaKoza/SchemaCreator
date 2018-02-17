namespace SchemaCreator.Designer.Interfaces
{
    public interface IDesignerItem : ISelectable
    {
        double Angle { get; set; }
        double Top { get; set; }
        double Left { get; set; }
        double Width { get; set; }
        double Height { get; set; }
    }
}