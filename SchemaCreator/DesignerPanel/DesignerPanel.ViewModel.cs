using GalaSoft.MvvmLight;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.UserControls;

namespace SchemaCreator.UI.ViewModel
{
    public class DesignerPanelViewModel : ViewModelBase
    {
        public DesignerPanelViewModel()
        {
            DesignerViewModel = new DesignerViewModel();
            // TEST:
            DesignerViewModel.ItemToDraw = new LineViewModel();
        }

        private IDesignerViewModel _designerViewModel;
        public IDesignerViewModel DesignerViewModel
        {
            get { return _designerViewModel; }
            set { _designerViewModel = value; RaisePropertyChanged(nameof(DesignerViewModel)); }
        }
    }
}
