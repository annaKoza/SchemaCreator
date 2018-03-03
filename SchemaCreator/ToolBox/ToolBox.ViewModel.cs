using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using SchemaCreator.Designer.BaseDrawableItems;
using SchemaCreator.Designer.Interfaces;
using System.Collections.ObjectModel;

namespace SchemaCreator.UI.ViewModel
{
    public class ToolBoxViewModel : ViewModelBase
    {
        private IBaseChooseAbleItem _selectedDrawingItem;

        public IBaseChooseAbleItem SelectedDrawingItem
        {
            get => _selectedDrawingItem;
            set
            {
                _selectedDrawingItem = value;
                Messenger.Default.Send<IBaseChooseAbleItem>(value);
                RaisePropertyChanged(nameof(SelectedDrawingItem));
            }
        }

        public ObservableCollection<IBaseChooseAbleItem> DrawableItems
        {
            get => _drawdableItems;
            set
            {
                _drawdableItems = value;
                RaisePropertyChanged(nameof(DrawableItems));
            }
        }

        private ObservableCollection<IDesignerItem> _toolItems;
        private ObservableCollection<IBaseChooseAbleItem> _drawdableItems;

        public ObservableCollection<IDesignerItem> ToolItems
        {
            get => _toolItems;
            set { _toolItems = value; RaisePropertyChanged(nameof(ToolItems)); }
        }

        public ToolBoxViewModel()
        {
            DrawableItems = new ObservableCollection<IBaseChooseAbleItem>() { new SelectionViewModel(), new LineViewModel(), new PolyLineViewModel() };
            ToolItems = new ObservableCollection<IDesignerItem>() { new RectangleViewModel(), new CircleViewModel(), new CircleViewModel(), new CircleViewModel(), new CircleViewModel(), new RectangleViewModel() };
        }
    }
}