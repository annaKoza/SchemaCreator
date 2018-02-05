using GalaSoft.MvvmLight;
using SchemaCreator.Designer.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaCreator.UI.ViewModel
{
    public  class CircleViewModel : BaseDesignerItemViewModel
    {
        private string _imageSource;
        public string ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; RaisePropertyChanged(nameof(ImageSource)); }
        }
        public CircleViewModel()
        {
            ImageSource = @"pack://application:,,,/SchemaCreator;component/Resources/Images/Setting.png";
        }
    }
}
