using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SchemaCreator.Designer.Controls;
using SchemaCreator.Designer.Interfaces;
using System.Windows;
using System.Windows.Input;

namespace SchemaCreator.Designer.UserControls
{
    public class BaseDesignerItemViewModel : ViewModelBase, IDesignerItem
    {
        public BaseDesignerItemViewModel()
        {
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; RaisePropertyChanged(nameof(IsSelected)); }
        }

        private double _minWidth;

        public double MinWidth
        {
            get { return _minWidth; }
            set { _minWidth = value; RaisePropertyChanged(nameof(MinWidth)); }
        }

        private double _minHeight;

        public double MinHeight
        {
            get { return _minHeight; }
            set { _minHeight = value; RaisePropertyChanged(nameof(MinHeight)); }
        }

        private double _angle;

        public double Angle
        {
            get => _angle;
            set { _angle = value; RaisePropertyChanged(nameof(Angle)); }
        }

        private double _top;
        private double _left;
        private double _width;
        private double _height;

        private Point _transformOrigin;

        public Point TransformOrigin
        {
            get { return _transformOrigin; }
            set { _transformOrigin = value; RaisePropertyChanged(nameof(TransformOrigin)); }
        }

        private int _zIndex;
        public int ZIndex
        {
            get { return _zIndex; }
            set { _zIndex = value; RaisePropertyChanged(nameof(ZIndex)); }
        }

        public double Top
        {
            get => _top;
            set { _top = value; RaisePropertyChanged(nameof(Top)); }
        }

        public double Left
        {
            get => _left;
            set { _left = value; RaisePropertyChanged(nameof(Left)); }
        }

        public double Width
        {
            get => _width;
            set { _width = value; RaisePropertyChanged(nameof(Width)); }
        }

        public double Height
        {
            get => _height;
            set { _height = value; RaisePropertyChanged(nameof(Height)); }
        }

        private ICommand _selectCommand;

        public ICommand SelectCommand
        {
            get
            {
                return _selectCommand ??
                    (
                    _selectCommand = new RelayCommand<object>
                        (
                            (designer) =>
                            {
                                SelectItem(designer as DesignerViewModel);
                            }
                        )
                    );
            }
        }

        private void SelectItem(DesignerViewModel designer)
        {
            if (designer is ISelectionPanel)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                {
                    if ((Keyboard.Modifiers & (ModifierKeys.Shift)) != ModifierKeys.None)
                    {
                        if (!IsSelected)
                        {
                            designer.SelectionService.AddToSelection(this);
                        }
                        else
                        {
                            designer.SelectionService.RemoveFromSelection(this);
                        }
                    }

                    if ((Keyboard.Modifiers & (ModifierKeys.Control)) != ModifierKeys.None)
                    {
                        if (!IsSelected)
                        {
                            designer.SelectionService.AddToSelection(this);
                        }
                        else
                        {
                            designer.SelectionService.RemoveFromSelection(this);
                        }
                    }
                }
                else if (!IsSelected)
                {
                    designer.SelectionService.SelectItem(this);
                }
            }
        }
    }
}