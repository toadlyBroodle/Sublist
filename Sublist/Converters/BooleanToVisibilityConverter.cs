using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Sublist.Converters
{
    public class BooleanToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isVisible = (bool) value;
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visibility = (Visibility) value;
            return visibility == Visibility.Visible;
        }
    }
}
