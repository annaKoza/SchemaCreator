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
            Messenger.Default.Register<IBaseChoosableItem>(this, HandleBaseItemSelection);
        }

        private void HandleBaseItemSelection(IBaseChoosableItem obj)
        {
            SelectItem(obj);
        }

        public void SelectItem(IBaseChoosableItem item)
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