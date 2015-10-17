using System;
using Windows.UI.Xaml.Data;

namespace Sublist.Converters
{
    public class BooleanToNullableBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool?) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var boolean = value as bool?;
            return boolean ?? false;
        }
    }
}