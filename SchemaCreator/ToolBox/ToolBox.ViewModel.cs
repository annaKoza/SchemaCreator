using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace SchemaCreator.UI.ViewModel
{
    public class ToolBoxViewModel : ViewModelBase
    {
        private ObservableCollection<ViewModelBase> _toolItems;
        public ObservableCollection<ViewModelBase> ToolItems
        {
            get => _toolItems;
            set { _toolItems = value; RaisePropertyChanged(nameof(ToolItems)); }
        }
        public ToolBoxViewModel()
        {
            ToolItems = new ObservableCollection<ViewModelBase>() { new RectangleViewModel(), new CircleViewModel(), new CircleViewModel(), new CircleViewModel(), new CircleViewModel(), new RectangleViewModel() };
        }
    }
}
