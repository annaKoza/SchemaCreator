using GalaSoft.MvvmLight;
using SchemaCreator.Designer.Interfaces;
using System.Collections.ObjectModel;

namespace SchemaCreator.Designer.UserControls
{
    public class DesignerViewModel : ViewModelBase, IDesignerViewModel
    {
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
        
        public void AddItem(BaseDesignerItemViewModel item)
        {
            Items.Add(item);
        }
        public DesignerViewModel()
        {
            Items = new
                 ObservableCollection<BaseDesignerItemViewModel>();
        }
    }
}
