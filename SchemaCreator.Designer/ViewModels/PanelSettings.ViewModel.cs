using GalaSoft.MvvmLight;
using SchemaCreator.Designer.Interfaces;
using System.Windows;
using System.Windows.Media;

namespace SchemaCreator.Designer.ViewModels
{
    public class PanelSettingsViewModel : ViewModelBase, IDesignerPanelSettings
    {
        public PanelSettingsViewModel()
        {
            SnapItemToGrid = true;
            IsGridSnapVisible = true;
        }

        private bool _snapItemToGrid;

        public bool SnapItemToGrid
        {
            get => _snapItemToGrid;
            set
            {
                _snapItemToGrid = value;
                RaisePropertyChanged();
            }
        }

        private Point _snapGridOffset;

        public Point SnapGridOffset
        {
            get => _snapGridOffset;
            set
            {
                _snapGridOffset = value;
                RaisePropertyChanged();
            }
        }

        private bool _isGridSnapVisible;

        public bool IsGridSnapVisible
        {
            get => _isGridSnapVisible;
            set
            {
                _isGridSnapVisible = value;
                RaisePropertyChanged();
            }
        }

        private Transform _transform;

        public Transform Transform
        {
            get => _transform;
            set
            {
                _transform = value; RaisePropertyChanged();
            }
        }
    }
}