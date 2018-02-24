using SchemaCreator.Designer.UserControls;

namespace SchemaCreator.UI.ViewModel
{
    public class RectangleViewModel : BaseDesignerItemViewModel
    {
        private string _imageSource;

        public string ImageSource
        {
            get => _imageSource;
            set { _imageSource = value; RaisePropertyChanged(nameof(ImageSource)); }
        }

        public RectangleViewModel()
        {
            ImageSource = @"pack://application:,,,/SchemaCreator;component/Resources/Images/settings.png";
        }
    }
}