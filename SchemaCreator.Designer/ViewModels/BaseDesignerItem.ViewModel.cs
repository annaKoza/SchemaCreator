using GalaSoft.MvvmLight;
using SchemaCreator.Designer.Interfaces;

namespace SchemaCreator.Designer.UserControls
{
    public class BaseDesignerItemViewModel : ViewModelBase
    {
        private IDesignerViewModel _parentViewModel;
        public IDesignerViewModel ParentViewModel
        {
            get { return _parentViewModel; }
            set { _parentViewModel = value; }
        }

        private double _top;

        public double Top
        {
            get { return _top; }
            set { _top = value; RaisePropertyChanged(nameof(Top)); }
        }
        private double _left;
        private double _width;
        private double _height;

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