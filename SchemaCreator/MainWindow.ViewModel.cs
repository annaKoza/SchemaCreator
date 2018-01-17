using SchemaCreator.UI.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SchemaCreator.UI.ViewModel
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private BackgroundViewModel _backgroundViewModel;
        public BackgroundViewModel BackgroundViewModel
        {
            get { return _backgroundViewModel; }
            set { _backgroundViewModel = value; }
        }

        private MenuViewModel _menuViewModel;
        public MenuViewModel MenuViewModel
        {
            get { return _menuViewModel; }
            set { _menuViewModel = value; }
        }

        public MainWindowViewModel()
        {
            MenuViewModel = new MenuViewModel(Menu);
            BackgroundViewModel = new BackgroundViewModel();
        }

        private ObservableCollection<MenuSection> _menu;
        public ObservableCollection<MenuSection> Menu => _menu ?? (_menu = GetContextMenu());

        public ObservableCollection<MenuSection> GetContextMenu()
        {
            var editSubmenu = new ObservableCollection<MenuSection>() {
                new MenuSection() { MenuText = "one"},
                new MenuSeparator(),
                new MenuSection() { MenuText = "other"} };
            var fileSublemu = new ObservableCollection<MenuSection>() {
                new MenuSection() { MenuText = "Add Background image", Command = OpenImage},
                new MenuSection() { MenuText = "Remove Background image", Command = RemoveImage, IsEnabled=false} };
            var menu = new ObservableCollection<MenuSection>() {
                new MenuSection() { MenuText = "File", SubMenu = fileSublemu },
                new MenuSection() { MenuText = "Edit", SubMenu = editSubmenu } };
            return menu;
        }

        ICommand _openImage;
        ICommand OpenImage => _openImage ?? (_openImage = new RelayCommand(
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
                BackgroundViewModel.ImagePath = dialog.FilePath;

                MenuViewModel.EnableItems("Remove Background image");
            }));

        ICommand _removeImage;
        ICommand RemoveImage => _removeImage ?? (_removeImage = new RelayCommand(
            (obj) =>
            {
                BackgroundViewModel.ImagePath = string.Empty;
                
                MenuViewModel.DisableItems("Remove Background image");
            }));
    }
}
