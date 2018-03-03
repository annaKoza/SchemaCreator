using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.UserControls;

namespace SchemaCreator.UI.ViewModel
{
    public class DesignerPanelViewModel : ViewModelBase
    {
        public DesignerPanelViewModel()
        {
            DesignerViewModel = new DesignerViewModel();
            Messenger.Default.Register<IBaseChooseAbleItem>(this, HandleBaseItemSelection);
        }

        private void HandleBaseItemSelection(IBaseChooseAbleItem obj)
        {
            SelectItem(obj);
        }

        private void SelectItem(IBaseChooseAbleItem item)
        {
            DesignerViewModel.ItemToDraw = item;
        }

        private IDesignerViewModel _designerViewModel;

        public IDesignerViewModel DesignerViewModel
        {
            get => _designerViewModel;
            set { _designerViewModel = value; RaisePropertyChanged(nameof(DesignerViewModel)); }
        }
    }
}