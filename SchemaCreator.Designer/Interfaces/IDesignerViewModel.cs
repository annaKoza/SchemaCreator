using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.UserControls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SchemaCreator.Designer.Interfaces
{
    public interface IDesignerViewModel : IDrawablePanel, ISelectionPanel
    {
        IDesignerPanelSettings PanelSettings
        {
            get;
        }

        ICommand BringToFront
        {
            get;
        }

        ICommand SendForward
        {
            get;
        }

        ICommand BringToBack
        {
            get;
        }

        ICommand SendBackward
        {
            get;
        }

        ICommand RemoveItems
        {
            get;
        }

        ICommand RemoveAll
        {
            get;
        }

        void BringSelectedItemsToFront();

        void SendSelectedItemsForward();

        void BringsSelectedItemsToBack();

        void SendSelectedItemsBackward();

        void AddItem(IDesignerItem item);

        void RemoveSelectedItems();

        void RemoveAllItems();

        ObservableCollection<BaseDesignerItemViewModel> Items
        {
            get; set;
        }
    }
}