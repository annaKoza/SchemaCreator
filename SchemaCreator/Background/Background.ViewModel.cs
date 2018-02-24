using GalaSoft.MvvmLight;

namespace SchemaCreator.UI.ViewModel
{
    public class BackgroundViewModel : ViewModelBase
    {
        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                RaisePropertyChanged(nameof(ImagePath));
            }
        }

        private int _opacity;

        public int Opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                RaisePropertyChanged(nameof(ImagePath));
            }
        }

        public BackgroundViewModel()
        {
            ImagePath = string.Empty;
            Opacity = 1;
        }
    }
}