using System;
using Windows.UI.Xaml.Data;

namespace IpScanner.Ui.Convertors
{
    public class BoolToVisibilityInvertConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool visibility = (bool)value;
            return visibility ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
