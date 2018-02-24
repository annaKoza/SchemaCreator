using GalaSoft.MvvmLight;
using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.Services;
using System.Collections.ObjectModel;

namespace SchemaCreator.Designer.UserControls
{
    public class DesignerViewModel : ViewModelBase, IDesignerViewModel, ISelectionPanel
    {
        private SelectionService _selectionService;
        public SelectionService SelectionService => _selectionService ?? (_selectionService = new SelectionService());

        private ObservableCollection<BaseDesignerItemViewModel> _items;

        public ObservableCollection<BaseDesignerItemViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }

        public IBaseChoosableItem ItemToDraw { get; set; }

        public void AddItem(IDesignerItem item)
        {
            Items.Add(item as BaseDesignerItemViewModel);
        }

        public DesignerViewModel()
        {
            Items = new
                 ObservableCollection<BaseDesignerItemViewModel>();
        }
    }
}