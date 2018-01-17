using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaCreator.UI.ViewModel
{
    class BackgroundViewModel : BaseViewModel
    {
        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; OnPropertyChanged(nameof(ImagePath)); }
        }
        private int _opacity;
        public int Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }
        public BackgroundViewModel()
        {
            ImagePath = string.Empty;
            Opacity = 1;
        }
    }
}
