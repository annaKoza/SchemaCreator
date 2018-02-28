using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SchemaCreator.Designer.Interfaces;
using SchemaCreator.Designer.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SchemaCreator.Designer.UserControls
{
    public abstract class BaseDesignerItemViewModel : ViewModelBase, IDesignerItem
    {
        private double _angle;

        private ObservableCollection<ContextMenuViewModel> _contextMenu;
        private double _height;

        private bool _isSelected;
        private double _left;

        private double _minHeight;

        private double _minWidth;
        private IDesignerViewModel _parent;

        private ICommand _selectCommand;

        private double _top;

        private Point _transformOrigin;
        private double _width;

        private int _zIndex;

        protected BaseDesignerItemViewModel() => ContextMenu =
            new ObservableCollection<ContextMenuViewModel>();

        private void SelectItem()
        {
            if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) !=
                ModifierKeys.None)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift)) !=
                    ModifierKeys.None)
                {
                    if (!IsSelected)
                    {
                        Parent.SelectionService.AddToSelection(this);
                    }
                    else
                    {
                        Parent.SelectionService.RemoveFromSelection(this);
                    }
                }

                if ((Keyboard.Modifiers & (ModifierKeys.Control)) !=
                    ModifierKeys.None)
                {
                    if (!IsSelected)
                    {
                        Parent.SelectionService.AddToSelection(this);
                    }
                    else
                    {
                        Parent.SelectionService.RemoveFromSelection(this);
                    }
                }
            }
            else if (!IsSelected)
            {
                Parent.SelectionService.SelectItem(this);
            }
        }

        public virtual void AddContextMenu(IDesignerViewModel parent) => ContextMenu =
            new ObservableCollection<ContextMenuViewModel>()
            {
                new ContextMenuViewModel()
                {
                        Name = "Delete",
                        Action =
                                parent.RemoveItems
                },
                new ContextMenuViewModel()
                {
                    Name = "Order",
                    ContextMenu = new ObservableCollection<ContextMenuViewModel>()
                    {
                         new ContextMenuViewModel()  { Name="Bring to Front", Action=Parent.BringToFront},
                         new ContextMenuViewModel()  { Name="Send Forward", Action=Parent.SendForward},
                         new ContextMenuViewModel()  { Name="Bring to Back", Action=Parent.BringToBack},
                         new ContextMenuViewModel()  { Name="Send Backward", Action=Parent.SendBackward},
                    }
                }
            };

        public double Angle
        {
            get => _angle;
            set
            {
                _angle = value; RaisePropertyChanged(nameof(Angle));
            }
        }

        public ObservableCollection<ContextMenuViewModel> ContextMenu
        {
            get => _contextMenu;
            protected set
            {
                _contextMenu = value;
                RaisePropertyChanged(nameof(ContextMenu));
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                _height = value; RaisePropertyChanged(nameof(Height));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value; RaisePropertyChanged(nameof(IsSelected));
            }
        }

        public double Left
        {
            get => _left;
            set
            {
                _left = value; RaisePropertyChanged(nameof(Left));
            }
        }

        public double MinHeight
        {
            get => _minHeight;
            set
            {
                _minHeight = value; RaisePropertyChanged(nameof(MinHeight));
            }
        }

        public double MinWidth
        {
            get => _minWidth;
            set
            {
                _minWidth = value; RaisePropertyChanged(nameof(MinWidth));
            }
        }

        public IDesignerViewModel Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                if (value != null)
                    AddContextMenu(value);
                RaisePropertyChanged(nameof(Parent));
            }
        }

        public ICommand SelectCommand => _selectCommand ??
                    (
                    _selectCommand =
                new RelayCommand
                        (
                            () =>
                            {
                                SelectItem();
                            }
                        )
                    );

        public double Top
        {
            get => _top;
            set
            {
                _top = value; RaisePropertyChanged(nameof(Top));
            }
        }

        public Point TransformOrigin
        {
            get => _transformOrigin;
            set
            {
                _transformOrigin = value; RaisePropertyChanged(nameof(TransformOrigin));
            }
        }

        public double Width
        {
            get => _width;
            set
            {
                _width = value; RaisePropertyChanged(nameof(Width));
            }
        }

        public int ZIndex
        {
            get => _zIndex;
            set
            {
                _zIndex = value; RaisePropertyChanged(nameof(ZIndex));
            }
        }
    }
}