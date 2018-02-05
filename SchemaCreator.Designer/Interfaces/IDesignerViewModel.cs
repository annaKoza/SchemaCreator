using SchemaCreator.Designer.Services;
using SchemaCreator.Designer.UserControls;
using System.Collections.ObjectModel;

namespace SchemaCreator.Designer.Interfaces
{
    public interface IDesignerViewModel
    { 
        void AddItem(BaseDesignerItemViewModel item);
        ObservableCollection<BaseDesignerItemViewModel> Items { get; set; }
    }

}