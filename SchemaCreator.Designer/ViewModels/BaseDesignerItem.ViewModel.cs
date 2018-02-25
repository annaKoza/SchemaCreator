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
        protected BaseDesignerItemViewModel() => ContextMenu =
            new ObservableCollection<ContextMenuViewModel>();

        private ObservableCollection<ContextMenuViewModel> _contextMenu;

        public ObservableCollection<ContextMenuViewModel> ContextMenu
        {
            get => _contextMenu;
            protected set
            {
                _contextMenu = value;
                RaisePropertyChanged(nameof(ContextMenu));
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value; RaisePropertyChanged(nameof(IsSelected));
            }
        }

        private double _minWidth;

        public double MinWidth
        {
            get => _minWidth;
            set
            {
                _minWidth = value; RaisePropertyChanged(nameof(MinWidth));
            }
        }

        private double _minHeight;

        public double MinHeight
        {
            get => _minHeight;
            set
            {
                _minHeight = value; RaisePropertyChanged(nameof(MinHeight));
            }
        }

        private double _angle;

        public double Angle
        {
            get => _angle;
            set
            {
                _angle = value; RaisePropertyChanged(nameof(Angle));
            }
        }

        private double _top;
        private double _left;
        private double _width;
        private double _height;

        private Point _transformOrigin;

        public Point TransformOrigin
        {
            get => _transformOrigin;
            set
            {
                _transformOrigin = value; RaisePropertyChanged(nameof(TransformOrigin));
            }
        }

        private int _zIndex;

        public int ZIndex
        {
            get => _zIndex;
            set
            {
                _zIndex = value; RaisePropertyChanged(nameof(ZIndex));
            }
        }

        public double Top
        {
            get => _top;
            set
            {
                _top = value; RaisePropertyChanged(nameof(Top));
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

        public double Width
        {
            get => _width;
            set
            {
                _width = value; RaisePropertyChanged(nameof(Width));
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

        private ICommand _selectCommand;
        private IDesignerViewModel _parent;

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

        public IDesignerViewModel Parent
        {
            get => _parent;
            set
            {
                _parent = value;
                if(value != null)
                    AddContextMenu(value);
                RaisePropertyChanged(nameof(Parent));
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

        private void SelectItem()
        {
            if((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) !=
                ModifierKeys.None)
            {
                if((Keyboard.Modifiers & (ModifierKeys.Shift)) !=
                    ModifierKeys.None)
                {
                    if(!IsSelected)
                    {
                        Parent.SelectionService.AddToSelection(this);
                    } else
                    {
                        Parent.SelectionService.RemoveFromSelection(this);
                    }
                }

                if((Keyboard.Modifiers & (ModifierKeys.Control)) !=
                    ModifierKeys.None)
                {
                    if(!IsSelected)
                    {
                        Parent.SelectionService.AddToSelection(this);
                    } else
                    {
                        Parent.SelectionService.RemoveFromSelection(this);
                    }
                }
            } else if(!IsSelected)
            {
                Parent.SelectionService.SelectItem(this);
            }
        }
    }
}