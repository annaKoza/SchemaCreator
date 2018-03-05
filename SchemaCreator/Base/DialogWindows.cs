using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace SchemaCreator.UI.Base
{
    public abstract class CommandDialogBox : DialogBox
    {
        public override ICommand Show
        {
            get
            {
                if(show == null) show = new RelayCommand(
                    o =>
                    {
                        ExecuteCommand(CommandBefore, CommandParameter);
                        execute(o);
                        ExecuteCommand(CommandAfter, CommandParameter);
                    });
                return show;
            }
        }

        public static DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter",
                                                                                                typeof(object),
                                                                                                typeof(CommandDialogBox));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        protected static void ExecuteCommand(ICommand command,
                                             object commandParameter)
        {
            if(command != null)
                if(command.CanExecute(commandParameter))
                    command.Execute(commandParameter);
        }

        public static DependencyProperty CommandBeforeProperty = DependencyProperty.Register("CommandBefore",
                                                                                             typeof(ICommand),
                                                                                             typeof(CommandDialogBox));

        public ICommand CommandBefore
        {
            get => (ICommand)GetValue(CommandBeforeProperty);
            set => SetValue(CommandBeforeProperty, value);
        }

        public static DependencyProperty CommandAfterProperty = DependencyProperty.Register("CommandAfter",
                                                                                            typeof(ICommand),
                                                                                            typeof(CommandDialogBox));

        public ICommand CommandAfter
        {
            get => (ICommand)GetValue(CommandAfterProperty);
            set => SetValue(CommandAfterProperty, value);
        }
    }

    public abstract class DialogBox : FrameworkElement, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string nazwaWłasności)
        {
            if(PropertyChanged != null)
                PropertyChanged(this,
                                new PropertyChangedEventArgs(nazwaWłasności));
        }

        #endregion INotifyPropertyChanged

        protected Action<object> execute = null;

        public string Caption
        {
            get; set;
        }// = null;

        protected ICommand show;

        public virtual ICommand Show
        {
            get
            {
                if(show == null) show = new RelayCommand(execute);
                return show;
            }
        }
    }

    public class SimpleMessageDialogBox : DialogBox
    {
        public SimpleMessageDialogBox() => execute =
                o =>
                {
                    MessageBox.Show((string)o, Caption);
                };
    }

    public class NotificationDialogBox : CommandDialogBox
    {
        public NotificationDialogBox() => execute =
                o =>
                {
                    MessageBox.Show((string)o,
                                    Caption,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                };
    }

    public class MessageDialogBox : CommandDialogBox
    {
        public MessageBoxResult? LastResult
        {
            get; protected set;
        }

        public MessageBoxButton Buttons
        {
            get; set;
        }// = MessageBoxButton.OK;

        public MessageBoxImage Icon
        {
            get; set;
        }// = MessageBoxImage.None;

        public bool IsLastResultYes
        {
            get
            {
                if(!LastResult.HasValue) return false;
                return LastResult.Value == MessageBoxResult.Yes;
            }
        }

        public bool IsLastResultNo
        {
            get
            {
                if(!LastResult.HasValue) return false;
                return LastResult.Value == MessageBoxResult.No;
            }
        }

        public bool IsLastResultCancel
        {
            get
            {
                if(!LastResult.HasValue) return false;
                return LastResult.Value == MessageBoxResult.Cancel;
            }
        }

        public bool IsLastResultOK
        {
            get
            {
                if(!LastResult.HasValue) return false;
                return LastResult.Value == MessageBoxResult.OK;
            }
        }

        public MessageDialogBox()
        {
            Buttons = MessageBoxButton.OK;
            Icon = MessageBoxImage.None;

            execute = o =>
            {
                LastResult = MessageBox.Show((string)o, Caption, Buttons, Icon);
                OnPropertyChanged(nameof(LastResult));
                switch(LastResult)
                {
                    case MessageBoxResult.Yes:
                        OnPropertyChanged(nameof(IsLastResultYes));
                        ExecuteCommand(CommandYes, CommandParameter);
                        break;

                    case MessageBoxResult.No:
                        OnPropertyChanged(nameof(IsLastResultNo));
                        ExecuteCommand(CommandNo, CommandParameter);
                        break;

                    case MessageBoxResult.Cancel:
                        OnPropertyChanged(nameof(IsLastResultCancel));
                        ExecuteCommand(CommandCancel, CommandParameter);
                        break;

                    case MessageBoxResult.OK:
                        OnPropertyChanged(nameof(IsLastResultOK));
                        ExecuteCommand(CommandOK, CommandParameter);
                        break;
                }
            };
        }

        public static DependencyProperty CommandYesProperty = DependencyProperty.Register("CommandYes",
                                                                                          typeof(ICommand),
                                                                                          typeof(MessageDialogBox));
        public static DependencyProperty CommandNoProperty = DependencyProperty.Register("CommandNo",
                                                                                         typeof(ICommand),
                                                                                         typeof(MessageDialogBox));
        public static DependencyProperty CommandCancelProperty = DependencyProperty.Register("CommandCancel",
                                                                                             typeof(ICommand),
                                                                                             typeof(MessageDialogBox));
        public static DependencyProperty CommandOKProperty = DependencyProperty.Register("CommandOK",
                                                                                         typeof(ICommand),
                                                                                         typeof(MessageDialogBox));

        public ICommand CommandYes
        {
            get => (ICommand)GetValue(CommandYesProperty);
            set => SetValue(CommandYesProperty, value);
        }

        public ICommand CommandNo
        {
            get => (ICommand)GetValue(CommandNoProperty);
            set => SetValue(CommandNoProperty, value);
        }

        public ICommand CommandCancel
        {
            get => (ICommand)GetValue(CommandCancelProperty);
            set => SetValue(CommandCancelProperty, value);
        }

        public ICommand CommandOK
        {
            get => (ICommand)GetValue(CommandOKProperty);
            set => SetValue(CommandOKProperty, value);
        }
    }

    public class ConditionalMessageDialogBox : MessageDialogBox
    {
        public static DependencyProperty IsDialogBypassedProperty = DependencyProperty.Register("IsDialogBypassed",
                                                                                                typeof(bool),
                                                                                                typeof(ConditionalMessageDialogBox));

        public bool IsDialogBypassed
        {
            get => (bool)GetValue(IsDialogBypassedProperty);
            set => SetValue(IsDialogBypassedProperty, value);
        }

        public MessageBoxResult DialogBypassButton
        {
            get; set;
        }// = MessageBoxResult.None;

        public ConditionalMessageDialogBox()
        {
            DialogBypassButton = MessageBoxResult.None;

            execute = o =>
            {
                MessageBoxResult result;
                if(!IsDialogBypassed)
                {
                    LastResult = MessageBox.Show((string)o,
                                                 Caption,
                                                 Buttons,
                                                 Icon);
                    OnPropertyChanged(nameof(LastResult));
                    result = LastResult.Value;
                } else
                {
                    result = DialogBypassButton;
                }
                switch(result)
                {
                    case MessageBoxResult.Yes:
                        if(!IsDialogBypassed) OnPropertyChanged(nameof(IsLastResultYes));
                        ExecuteCommand(CommandYes, CommandParameter);
                        break;

                    case MessageBoxResult.No:
                        if(!IsDialogBypassed) OnPropertyChanged(nameof(IsLastResultNo));
                        ExecuteCommand(CommandNo, CommandParameter);
                        break;

                    case MessageBoxResult.Cancel:
                        if(!IsDialogBypassed) OnPropertyChanged(nameof(IsLastResultCancel));
                        ExecuteCommand(CommandCancel, CommandParameter);
                        break;

                    case MessageBoxResult.OK:
                        if(!IsDialogBypassed) OnPropertyChanged(nameof(IsLastResultOK));
                        ExecuteCommand(CommandOK, CommandParameter);
                        break;
                }
            };
        }
    }

    public abstract class FileDialogBox : CommandDialogBox
    {
        public bool? FileDialogResult
        {
            get; protected set;
        }

        public string FilePath
        {
            get; set;
        }

        public string Filter
        {
            get; set;
        }

        public int FilterIndex
        {
            get; set;
        }

        public string DefaultExt
        {
            get; set;
        }

        protected FileDialog fileDialog = null;

        protected FileDialogBox() => execute =
                o =>
                {
                    fileDialog.Title = Caption;
                    fileDialog.Filter = Filter;
                    fileDialog.FilterIndex = FilterIndex;
                    fileDialog.DefaultExt = DefaultExt;
                    string filePath = string.Empty;
                    if(FilePath != null) filePath = FilePath; else FilePath = string.Empty;
                    if(o != null) filePath = (string)o;
                    if(!string.IsNullOrWhiteSpace(filePath))
                    {
                        fileDialog.InitialDirectory = Path.GetDirectoryName(filePath);
                        fileDialog.FileName = Path.GetFileName(filePath);
                    }
                    FileDialogResult = fileDialog.ShowDialog();
                    OnPropertyChanged(nameof(FileDialogResult));
                    if(FileDialogResult.HasValue && FileDialogResult.Value)
                    {
                        FilePath = fileDialog.FileName;
                        OnPropertyChanged(nameof(FilePath));
                        ExecuteCommand(CommandFileOk, FilePath);
                    };
                };

        public static DependencyProperty CommandFileOkProperty = DependencyProperty.Register("CommandFileOk",
                                                                                             typeof(ICommand),
                                                                                             typeof(FileDialogBox));

        public ICommand CommandFileOk
        {
            get => (ICommand)GetValue(CommandFileOkProperty);
            set => SetValue(CommandFileOkProperty, value);
        }
    }

    public class OpenFileDialogBox : FileDialogBox
    {
        public OpenFileDialogBox() => fileDialog =
            new OpenFileDialog();
    }

    public class SaveFileDialogBox : FileDialogBox
    {
        public SaveFileDialogBox() => fileDialog =
            new SaveFileDialog();
    }

    public class FileSavedNotificationDialogBox : CommandDialogBox
    {
        public FileSavedNotificationDialogBox() => execute =
                o =>
                {
                    MessageBox.Show("File " + (string)o + "Has been saved",
                                    Caption,
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                };
    }

    [ContentProperty(nameof(WindowContent))]
    public class CustomContentDialogBox : CommandDialogBox
    {
        private bool? LastResult;

        public double WindowWidth
        {
            get; set;
        }// = 640;

        public double WindowHeight
        {
            get; set;
        }// = 480;

        public object WindowContent
        {
            get; set;
        }// = null;

        private static Window window = null;

        public CustomContentDialogBox()
        {
            WindowWidth = 640;
            WindowHeight = 480;

            execute =
                o =>
                {
                    if(window == null)
                    {
                        window = new Window();
                        window.Width = WindowWidth;
                        window.Height = WindowHeight;
                        if(Caption != null) window.Title = Caption;
                        window.Content = WindowContent;
                        LastResult = window.ShowDialog();
                        OnPropertyChanged(nameof(LastResult));
                        window = null;
                        switch(LastResult)
                        {
                            case true:
                                ExecuteCommand(CommandTrue, CommandParameter);
                                break;

                            case false:
                                ExecuteCommand(CommandFalse, CommandParameter);
                                break;

                            case null:
                                ExecuteCommand(CommandNull, CommandParameter);
                                break;
                        }
                    }
                };
        }

        public static bool? GetCustomContentDialogResult(DependencyObject d) => (bool?)d.GetValue(DialogResultProperty);

        public static void SetCustomContentDialogResult(DependencyObject d,
                                                        bool? value) => d.SetValue(DialogResultProperty,
                                                                                   value);

        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(CustomContentDialogBox),
                new PropertyMetadata(null, DialogResultChanged));

        private static void DialogResultChanged(DependencyObject d,
                                                DependencyPropertyChangedEventArgs e)
        {
            var dialogResult = (bool?)e.NewValue;
            if(d is Button)
            {
                var button = d as Button;
                button.Click += (object sender, RoutedEventArgs _e) =>
                {
                    window.DialogResult = dialogResult;
                };
            }
        }

        public static DependencyProperty CommandTrueProperty = DependencyProperty.Register("CommandTrue",
                                                                                           typeof(ICommand),
                                                                                           typeof(CustomContentDialogBox));
        public static DependencyProperty CommandFalseProperty = DependencyProperty.Register("CommandFalse",
                                                                                            typeof(ICommand),
                                                                                            typeof(CustomContentDialogBox));
        public static DependencyProperty CommandNullProperty = DependencyProperty.Register("CommandNull",
                                                                                           typeof(ICommand),
                                                                                           typeof(CustomContentDialogBox));

        public ICommand CommandTrue
        {
            get => (ICommand)GetValue(CommandTrueProperty);
            set => SetValue(CommandTrueProperty, value);
        }

        public ICommand CommandFalse
        {
            get => (ICommand)GetValue(CommandFalseProperty);
            set => SetValue(CommandFalseProperty, value);
        }

        public ICommand CommandNull
        {
            get => (ICommand)GetValue(CommandNullProperty);
            set => SetValue(CommandNullProperty, value);
        }
    }
}