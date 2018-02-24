using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SchemaCreator.UI.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        private ObservableCollection<MenuSection> _menuItems;

        public ObservableCollection<MenuSection> MenuItems
        {
            get => _menuItems;
            set => _menuItems = value;
        }

        public MenuViewModel(ObservableCollection<MenuSection> menuItems)
        {
            MenuItems = menuItems;
        }

        public void DisableItems(string itemName)
        {
            var menuItem = GetMenuSectionsOfGivenMenuText(itemName, MenuItems);
            menuItem?.ForEach(x => x.IsEnabled = false);
        }

        public void EnableItems(string itemName)
        {
            var menuItem = GetMenuSectionsOfGivenMenuText(itemName, MenuItems);
            menuItem?.ForEach(x => x.IsEnabled = true);
        }

        public List<MenuSection> GetMenuSectionsOfGivenMenuText(string menuText, ObservableCollection<MenuSection> menuItems)
        {
            List<MenuSection> foundItems = new List<MenuSection>();
            foreach (var item in menuItems)
            {
                GetMenuItems(menuText, item, foundItems);
            }
            return foundItems;
        }

        private static void GetMenuItems(string textToFind, MenuSection item, List<MenuSection> items)
        {
            if (textToFind.Equals(item.MenuText)) items.Add(item);
            if (item.SubMenu?.Count == 0) return;
            foreach (var subitem in item.SubMenu)
            {
                GetMenuItems(textToFind, subitem, items);
            }
        }
    }
}