using SchemaCreator.Designer.Services;

namespace SchemaCreator.Designer.Controls
{
    public interface ISelectionPanel
    {
        SelectionService SelectionService { get; }
    }
}