using System;
using Windows.UI.Xaml.Data;

namespace IpScanner.Ui.Convertors
{
    public class StringContentToVisibility : IValueConverter
    {
        const int AcceptableCountOfChars = 35;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string content = value as string;

            if (content.Length < AcceptableCountOfChars)
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
