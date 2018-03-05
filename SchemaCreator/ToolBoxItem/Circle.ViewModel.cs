using SchemaCreator.Designer.UserControls;

namespace SchemaCreator.UI.ViewModel
{
    public class CircleViewModel : BaseDesignerItemViewModel
    {
        private string _imageSource;

        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value; RaisePropertyChanged(nameof(ImageSource));
            }
        }

        public CircleViewModel() => ImageSource =
            @"pack://application:,,,/SchemaCreator;component/Resources/Images/help.png";
    }
}