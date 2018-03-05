using GalaSoft.MvvmLight.Command;
using SchemaCreator.Designer.UserControls;
using System;
using System.Linq;
using System.Windows.Controls;

namespace SchemaCreator.Designer.Converters
{
    class SelectionChangedEventArgsToItemsViewModel : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var args = (SelectionChangedEventArgs)value;
            var param = parameter as Designer;
            var datacontext = param.DataContext as DesignerViewModel;
            return null;
        }
    }
}
