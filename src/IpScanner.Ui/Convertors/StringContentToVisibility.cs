using System;
using Windows.UI.Xaml.Data;

namespace IpScanner.Ui.Convertors
{
    public class StringContentToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string content = value as string;

            if (string.IsNullOrWhiteSpace(content))
            {
                return Windows.UI.Xaml.Visibility.Visible;
            }

            return Windows.UI.Xaml.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
