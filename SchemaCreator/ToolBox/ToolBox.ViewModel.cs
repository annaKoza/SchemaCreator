using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using SchemaCreator.Designer.DrawingPart;
using SchemaCreator.Designer.UserControls;

namespace SchemaCreator.UI.ViewModel
{
    public class ToolBoxViewModel : ViewModelBase
    {
        private DrawingItemViewModel _selectedDrawingItem;
        public DrawingItemViewModel SelectedDrawingItem
        {
            get => _selectedDrawingItem;
            set { _selectedDrawingItem = value; RaisePropertyChanged((nameof(SelectedDrawingItem)));}
        }

        public ObservableCollection<DrawingItemViewModel> DrawableItems
        { 
            get => _drawdableItems;
            set
            {
                _drawdableItems = value;
                RaisePropertyChanged(nameof(DrawableItems));
            }
        }

        private ObservableCollection<BaseDesignerItemViewModel> _toolItems;
        private ObservableCollection<DrawingItemViewModel> _drawdableItems;

        public ObservableCollection<BaseDesignerItemViewModel> ToolItems
        {
            get => _toolItems;
            set { _toolItems = value; RaisePropertyChanged(nameof(ToolItems)); }
        }
        public ToolBoxViewModel()
        {
            DrawableItems = new ObservableCollection<DrawingItemViewModel>(){ new LineViewModel()};
            ToolItems = new ObservableCollection<BaseDesignerItemViewModel>() { new RectangleViewModel(), new CircleViewModel(), new CircleViewModel(), new CircleViewModel(), new CircleViewModel(), new RectangleViewModel() };
        }
    }
}
