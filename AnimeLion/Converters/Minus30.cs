using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AnimeLion.Converters
{
    public class Minus30 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - 30;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}