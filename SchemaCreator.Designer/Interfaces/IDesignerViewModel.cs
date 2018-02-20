using SchemaCreator.Designer.Services;
using SchemaCreator.Designer.UserControls;
using System.Collections.ObjectModel;

namespace SchemaCreator.Designer.Interfaces
{
    public interface IDesignerViewModel : IDrawableDesigner
    { 
        void AddItem(IDesignerItem item);
        ObservableCollection<BaseDesignerItemViewModel> Items { get; set; }
    }
}