using System;
using Windows.UI.Xaml.Data;

namespace IpScanner.Ui.Convertors
{
    public class BoolToColumnSpanConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool span = (bool)value;

            return span ? 1 : 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
