using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SchemaCreator.Designer.Converters
{
    public class DoubleToPointConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture) => throw new NotImplementedException();

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            var val = (double)value;
            return new Point(val, val);
        }
    }
}