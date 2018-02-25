using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.UserControls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SchemaCreator.Designer.Interfaces
{
    public interface IDesignerViewModel : IDrawablePanel, ISelectionPanel
    {
        void BringSelectedItemsToFront();
        void SendSelectedItemsForward();
        void BringsSelectedItemsToBack();
        void SendSelectedItemsBackward();
        void AddItem(IDesignerItem item);
        void RemoveItem(IDesignerItem item);
        void RemoveItems(List<IDesignerItem> items);
        void RemoveSelectedItems();
        void RemoveAllItems();
        ObservableCollection<BaseDesignerItemViewModel> Items { get; set; }
    }
}