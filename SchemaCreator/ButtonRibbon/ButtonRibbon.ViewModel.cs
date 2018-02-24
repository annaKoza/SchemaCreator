using GalaSoft.MvvmLight;

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