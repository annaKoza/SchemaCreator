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

        public IDrawableItem ItemToDraw { get; set; }

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