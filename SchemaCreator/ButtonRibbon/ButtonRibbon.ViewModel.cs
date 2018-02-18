using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaCreator.UI.ViewModel
{
    public class ButtonRibbonViewModel : ViewModelBase
    {
        private ViewModelBase _viewModelForButtons;
        public ViewModelBase ViewModelForButtons
        {
            get => _viewModelForButtons;
            set
            {
                _viewModelForButtons = value;
                RaisePropertyChanged(nameof(ViewModelForButtons));
            }
        }
        public ButtonRibbonViewModel(ViewModelBase viewModelForButtons)
        {
            ViewModelForButtons = viewModelForButtons;
        }

    }
}
    