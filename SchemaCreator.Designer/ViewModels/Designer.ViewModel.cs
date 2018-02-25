using GalaSoft.MvvmLight;
using SchemaCreator.Designer.BaseDrawableItems;
using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SchemaCreator.Designer.UserControls
{
    public class DesignerViewModel : ViewModelBase, IDesignerViewModel
    {
        private SelectionService _selectionService;
        public SelectionService SelectionService => _selectionService ?? (_selectionService = new SelectionService());

        private ObservableCollection<BaseDesignerItemViewModel> _items;
        private IBaseChoosableItem _itemToDraw;

        public ObservableCollection<BaseDesignerItemViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }

        public IBaseChoosableItem ItemToDraw
        {
            get { return _itemToDraw; }
            set { _itemToDraw = value; }
        }

        public void AddItem(IDesignerItem item)
        {
            Items.Add(item as BaseDesignerItemViewModel);
        }

        public void RemoveItem(IDesignerItem item)
        {
        }

        public void RemoveItems(List<IDesignerItem> items)
        {
        }

        public void RemoveSelectedItems()
        {
        }

        public void RemoveAllItems()
        {
        }
        

        public void SendSelectedItemsForward()
        {
        }
        

        public void SendSelectedItemsBackward()
        {
        }

        public void BringSelectedItemsToFront()
        {
        }

        public void BringsSelectedItemsToBack()
        {
        }

        public DesignerViewModel()
        {
            Items = new
                 ObservableCollection<BaseDesignerItemViewModel>();
            ItemToDraw = new NullChoosedItemViewModel();
        }
    }
}