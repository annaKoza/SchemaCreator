namespace SchemaCreator.Designer.Interfaces
{
    public interface IDrawableDesigner
    {
        IDrawableItem DefaultItemToDraw { get; set; }
        IDrawableItem ItemToDraw { get; set; }
    }
}