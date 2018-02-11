using GalaSoft.MvvmLight;

namespace SchemaCreator.Designer.UserControls
{
    public class BaseDesignerItemViewModel : ViewModelBase
    {
        public BaseDesignerItemViewModel()
        {
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChanged(nameof(IsSelected)); }
        }

        private double _angle;

        public double Angle
        {
            get { return _angle; }
            set { _angle = value; RaisePropertyChanged(nameof(Angle)); }
        }

        private double _top;
        private double _left;
        private double _width;
        private double _height;

        public double Top
        {
            get { return _top; }
            set { _top = value; RaisePropertyChanged(nameof(Top)); }
        }

        public double Left
        {
            get { return _left; }
            set { _left = value; RaisePropertyChanged(nameof(Left)); }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; RaisePropertyChanged(nameof(Width)); }
        }

        public double Height
        {
            get { return _height; }
            set { _height = value; RaisePropertyChanged(nameof(Height)); }
        }
    }
}