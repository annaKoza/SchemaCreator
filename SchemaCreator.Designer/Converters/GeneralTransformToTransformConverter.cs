using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SchemaCreator.Designer.Converters
{
    public class GeneralTransformToTransformConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            var val = value as GeneralTransform;
            if(val != null)
                return (val as Transform).Inverse; else return value;
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture) => throw new NotImplementedException();
    }
}