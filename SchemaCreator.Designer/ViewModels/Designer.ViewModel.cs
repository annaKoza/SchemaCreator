using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SchemaCreator.Designer.BaseDrawableItems;
using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SchemaCreator.Designer.UserControls
{
    public class DesignerViewModel : ViewModelBase, IDesignerViewModel
    {
        private SelectionService _selectionService;

        public SelectionService SelectionService => _selectionService ??
            (_selectionService =
                new SelectionService());

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
            get => _itemToDraw;
            set => _itemToDraw = value;
        }

        private ICommand _bringToFront;
        private ICommand _sendForward;
        private ICommand _bringToBack;
        private ICommand _sendBackward;
        private ICommand _removeItems;
        private ICommand _removeAll;

        public ICommand BringToFront => _bringToFront ??
                   (
                   _bringToFront =
                new RelayCommand
                       (
                           () =>
                           {
                               BringSelectedItemsToFront();
                           }
                       )
                   );

        public ICommand SendForward => _sendForward ??
                  (
                  _sendForward =
                new RelayCommand
                      (
                          () =>
                          {
                              SendSelectedItemsForward();
                          }
                      )
                  );

        public ICommand BringToBack => _bringToBack ??
                 (
                 _bringToBack =
                new RelayCommand
                     (
                         () =>
                         {
                             BringsSelectedItemsToBack();
                         }
                     )
                 );

        public ICommand SendBackward => _sendBackward ??
               (
               _sendBackward =
                new RelayCommand
                   (
                       () =>
                       {
                           SendSelectedItemsBackward();
                       }
                   )
               );

        public ICommand RemoveItems => _removeItems ??
               (
               _removeItems =
                new RelayCommand
                   (
                       () =>
                       {
                           RemoveSelectedItems();
                       }
                   )
               );

        public ICommand RemoveAll => _removeAll ??
             (
             _removeAll =
                new RelayCommand
                 (
                     () =>
                     {
                         RemoveAllItems();
                     }
                 )
             );

        public void AddItem(IDesignerItem item) => Items.Add(item as BaseDesignerItemViewModel);

        public void RemoveSelectedItems()
        {
            Items.RemoveAll(x => x.IsSelected);
            SelectionService.ClearSelection();
        }

        public void RemoveAllItems()
        {
            Items.Clear();
            SelectionService.ClearSelection();
        }

        public void SendSelectedItemsForward()
        {
            List<BaseDesignerItemViewModel> ordered = (from item in SelectionService.SelectedItems
                                                       orderby (item as BaseDesignerItemViewModel).ZIndex descending
                                                       select item as BaseDesignerItemViewModel).ToList();

            int count = Items.Count;

            for(int i = 0; i < ordered.Count; i++)
            {
                int currentIndex = ordered[i].ZIndex;
                int newIndex = Math.Min(count - 1 - i, currentIndex + 1);
                if(currentIndex != newIndex)
                {
                    ordered[i].ZIndex = newIndex;
                    foreach(BaseDesignerItemViewModel elm in Items.Where(item => item.ZIndex
                        == newIndex))
                    {
                        if(elm != ordered[i])
                        {
                            elm.ZIndex = currentIndex;
                            break;
                        }
                    }
                }
            }
        }

        public void SendSelectedItemsBackward()
        {
            List<BaseDesignerItemViewModel> ordered = (from item in SelectionService.SelectedItems
                                                       orderby (item as BaseDesignerItemViewModel).ZIndex ascending
                                                       select item as BaseDesignerItemViewModel).ToList();

            int count = Items.Count;

            for(int i = 0; i < ordered.Count; i++)
            {
                int currentIndex = ordered[i].ZIndex;
                int newIndex = Math.Max(i, currentIndex - 1);
                if(currentIndex != newIndex)
                {
                    ordered[i].ZIndex = newIndex;
                    var it = Items.Where(item => item.ZIndex == newIndex);

                    foreach(var elm in it)
                    {
                        if(elm != ordered[i])
                        {
                            elm.ZIndex = currentIndex;
                            break;
                        }
                    }
                }
            }
        }

        public void BringSelectedItemsToFront()
        {
            List<BaseDesignerItemViewModel> selectionSorted = (from item in SelectionService.SelectedItems
                                                               orderby (item as BaseDesignerItemViewModel).ZIndex ascending
                                                               select item as BaseDesignerItemViewModel).ToList();

            List<BaseDesignerItemViewModel> childrenSorted = (from BaseDesignerItemViewModel item in Items
                                                              orderby item.ZIndex ascending
                                                              select item).ToList();

            int i = 0;
            int j = 0;

            foreach(var item in childrenSorted)
            {
                if(selectionSorted.Contains(item))
                {
                    int idx = item.ZIndex;
                    item.ZIndex = childrenSorted.Count -
                        selectionSorted.Count +
                        j++;
                } else
                {
                    item.ZIndex = i++;
                }
            }
        }

        public void BringsSelectedItemsToBack()
        {
            List<BaseDesignerItemViewModel> selectionSorted = (from item in SelectionService.SelectedItems
                                                               orderby (item as BaseDesignerItemViewModel).ZIndex ascending
                                                               select item as BaseDesignerItemViewModel).ToList();

            List<BaseDesignerItemViewModel> childrenSorted = (from BaseDesignerItemViewModel item in Items
                                                              orderby item.ZIndex ascending
                                                              select item).ToList();
            int i = 0;
            int j = 0;
            foreach(var item in childrenSorted)
            {
                if(selectionSorted.Contains(item))
                {
                    int idx = item.ZIndex;
                    item.ZIndex = j++;
                } else
                {
                    item.ZIndex = selectionSorted.Count + i++;
                }
            }
        }

        public DesignerViewModel()
        {
            Items = new
                 ObservableCollection<BaseDesignerItemViewModel>();
            ItemToDraw = new NullChoosedItemViewModel();
        }
    }
}