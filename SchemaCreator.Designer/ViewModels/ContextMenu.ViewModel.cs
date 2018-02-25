using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SchemaCreator.Designer.ViewModels
{
    public class ContextMenuViewModel : ViewModelBase
    {
        private ICommand _action;
        private ObservableCollection<ContextMenuViewModel> _contextMenu;
        private string _iconPath;
        private string _name;

        public ICommand Action
        {
            get => _action;
            set
            {
                _action = value;
                RaisePropertyChanged(nameof(Action));
            }
        }

        public virtual ObservableCollection<ContextMenuViewModel> ContextMenu
        {
            get => _contextMenu;
            set
            {
                _contextMenu = value;
                RaisePropertyChanged(nameof(ContextMenu));
            }
        }

        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                RaisePropertyChanged(nameof(IconPath));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }
    }
}