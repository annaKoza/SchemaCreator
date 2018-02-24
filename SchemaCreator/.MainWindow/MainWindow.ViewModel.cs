using GalaSoft.MvvmLight;
using SchemaCreator.UI.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SchemaCreator.UI.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DesignerPanelViewModel _designerPanelViewModel;

        public DesignerPanelViewModel DesignerPanelViewModel
        {
            get => _designerPanelViewModel;
            set
            {
                _designerPanelViewModel = value;
                RaisePropertyChanged(nameof(DesignerPanelViewModel));
            }
        }

        private BackgroundViewModel _backgroundViewModel;

        public BackgroundViewModel BackgroundViewModel
        {
            get => _backgroundViewModel;
            set
            {
                _backgroundViewModel = value;
                RaisePropertyChanged(nameof(BackgroundViewModel));
            }
        }

        private ToolBoxViewModel _toolBoxViewModel;

        public ToolBoxViewModel ToolBoxViewModel
        {
            get => _toolBoxViewModel;
            set
            {
                _toolBoxViewModel = value;
                RaisePropertyChanged(nameof(BackgroundViewModel));
            }
        }

        private MenuViewModel _menuViewModel;

        public MenuViewModel MenuViewModel
        {
            get => _menuViewModel;
            set
            {
                _menuViewModel = value;
                RaisePropertyChanged(nameof(BackgroundViewModel));
            }
        }

        private ButtonRibbonViewModel _buttonRibbonViewModel;

        public ButtonRibbonViewModel ButtonRibbonViewModel
        {
            get => _buttonRibbonViewModel;
            set
            {
                _buttonRibbonViewModel = value;
                RaisePropertyChanged(nameof(ButtonRibbonViewModel));
            }
        }

        public MainWindowViewModel()
        {
            MenuViewModel = new MenuViewModel(Menu);
            ToolBoxViewModel = new ToolBoxViewModel();
            DesignerPanelViewModel = new DesignerPanelViewModel();
        }

        private ObservableCollection<MenuSection> _menu;
        public ObservableCollection<MenuSection> Menu => _menu ?? (_menu = GetContextMenu());

        public ObservableCollection<MenuSection> GetContextMenu()
        {
            var editSubmenu = new ObservableCollection<MenuSection>() {
                new MenuSection() { MenuText = "one"},
                new MenuSeparator(),
                new MenuSection() { MenuText = "other"} };
            var backgroundSubmenu = new ObservableCollection<MenuSection>() {
                new MenuSection() { MenuText = "Add Background image", Command = OpenImage},
                new MenuSection() { MenuText = "Remove Background image", Command = RemoveImage, IsEnabled=false} };
            var menu = new ObservableCollection<MenuSection>() {
                new MenuSection() { MenuText = "File" },
                new MenuSection() { MenuText = "Edit", SubMenu = editSubmenu },
                new MenuSection() { MenuText = "Background", SubMenu = backgroundSubmenu },};
            return menu;
        }

        private ICommand _openImage;

        private ICommand OpenImage => _openImage ?? (_openImage = new RelayCommand(
            (obj) =>
            {
                OpenFileDialogBox dialog = new OpenFileDialogBox()
                {
                    Caption = "Open Background File",
                    DefaultExt = "jpg",
                    Filter = "JPEG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp",
                    FilterIndex = 1
                };
                dialog.Show.Execute(null);

                if (string.IsNullOrEmpty(dialog.FilePath)) return;
                BackgroundViewModel = new BackgroundViewModel() { ImagePath = dialog.FilePath };
                ButtonRibbonViewModel = new ButtonRibbonViewModel(BackgroundViewModel);
                MenuViewModel.EnableItems("Remove Background image");
            }));

        private ICommand _removeImage;

        private ICommand RemoveImage => _removeImage ?? (_removeImage = new RelayCommand(
            (obj) =>
            {
                BackgroundViewModel = null;
                ButtonRibbonViewModel = null;
                MenuViewModel.DisableItems("Remove Background image");
            }));
    }
}