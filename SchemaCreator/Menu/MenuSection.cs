using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        private string _menutext;

        public string MenuText
        {
            get => _menutext;
            set => _menutext = value;
        }

        private string _iconPath;

        public string IconPath
        {
            get => _iconPath;
            set => _iconPath = value;
        }

        private ICommand _command;

        public ICommand Command
        {
            get => _command;
            set => _command = value;
        }

        private ObservableCollection<MenuSection> _subMenu;

        public ObservableCollection<MenuSection> SubMenu
        {
            get => _subMenu;
            set => _subMenu = value;
        }
    }
}