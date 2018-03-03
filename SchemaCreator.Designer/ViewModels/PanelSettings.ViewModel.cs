using GalaSoft.MvvmLight;
using SchemaCreator.Designer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemaCreator.Designer.ViewModels
{
    public class PanelSettingsViewModel : ViewModelBase, IDesignerPanelSettings
    {
        public PanelSettingsViewModel()
        {
            SnapItemToGrid = true;
            IsGridSnapVisible = false;
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
    }
}
