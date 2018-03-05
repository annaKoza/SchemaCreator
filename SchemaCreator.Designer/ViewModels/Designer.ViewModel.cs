using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SchemaCreator.Designer.BaseDrawableItems;
using SchemaCreator.Designer.Helpers;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.Services;
using SchemaCreator.Designer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SchemaCreator.Designer.UserControls
{
    public class DesignerViewModel : ViewModelBase, IDesignerViewModel
    {
        private ICommand _bringToBack;
        private ICommand _bringToFront;

        private ObservableCollection<BaseDesignerItemViewModel> _items;
        private IBaseChooseAbleItem _itemToDraw;
        private ICommand _removeAll;
        private ICommand _removeItems;
        private ObservableCollection<BaseDesignerItemViewModel> _selectedItem;
        private SelectionService _selectionService;
        private ICommand _sendBackward;
        private ICommand _sendForward;

        public DesignerViewModel()
        {
            PanelSettings = new PanelSettingsViewModel();
            Items = new
                 ObservableCollection<BaseDesignerItemViewModel>();
            ItemToDraw = new NullChoosedItemViewModel();
        }

        public void AddItem(IDesignerItem item) => Items.Add(item as BaseDesignerItemViewModel);

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

        public void RemoveAllItems()
        {
            Items.Clear();
            SelectionService.ClearSelection();
        }

        public void RemoveSelectedItems()
        {
            Items.RemoveAll(x => x.IsSelected);
            SelectionService.ClearSelection();
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
                    foreach(BaseDesignerItemViewModel elm in Items.Where(item => item.ZIndex ==
                        newIndex))
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

        public ObservableCollection<BaseDesignerItemViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }

        public IBaseChooseAbleItem ItemToDraw
        {
            get => _itemToDraw;
            set => _itemToDraw = value;
        }

        public IDesignerPanelSettings PanelSettings
        {
            get;
        }

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

        public ObservableCollection<BaseDesignerItemViewModel> SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public SelectionService SelectionService => _selectionService ??
            (_selectionService =
                new SelectionService());

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
    }
}