using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.UserControls;
using System.Collections.ObjectModel;

namespace SchemaCreator.Designer.Interfaces
{
    public interface IDesignerViewModel : IDrawableDesigner, ISelectionPanel
    {
        void AddItem(IDesignerItem item);

        ObservableCollection<BaseDesignerItemViewModel> Items { get; set; }
    }
}