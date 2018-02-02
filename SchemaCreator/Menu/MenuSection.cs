using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SchemaCreator.UI.ViewModel
{
    public class MenuSection : ViewModelBase
    {
        public MenuSection()
        {
            SubMenu = new ObservableCollection<MenuSection>();
            IsEnabled = true;
        }
 
        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }
        private string _menutext;
        public string MenuText
        {
            get { return _menutext; }
            set { _menutext = value; }
        }

        private string _iconPath;
        public string IconPath
        {
            get { return _iconPath; }
            set { _iconPath = value; }
        }

        private ICommand _command;
        public ICommand Command
        {
            get { return _command; }
            set { _command = value; }
        }

        private ObservableCollection<MenuSection> _subMenu;
        public ObservableCollection<MenuSection> SubMenu
        {
            get { return _subMenu; }
            set { _subMenu = value; }
        }

    }
}
