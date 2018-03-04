using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.UserControls;
using System.Windows;
using System.Windows.Media;

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

        Point designerOffset;
        public Point DesignerOffset
        {
            get
            {
                return designerOffset;
            }
            set
            {
                designerOffset = value;
                DesignerViewModel.PanelSettings.SnapGridOffset = value;
            }
        }

        private Transform _transform;
        public Transform Transform
        {
            get => _transform;
            set
            {
                _transform = value;
                DesignerViewModel.PanelSettings.Transform = value;
            }
        }


        private IDesignerViewModel _designerViewModel;

        public IDesignerViewModel DesignerViewModel
        {
            get => _designerViewModel;
            set { _designerViewModel = value; RaisePropertyChanged(nameof(DesignerViewModel)); }
        }
    }
}