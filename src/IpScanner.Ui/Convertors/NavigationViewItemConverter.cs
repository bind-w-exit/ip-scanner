using Microsoft.UI.Xaml.Controls;
using System;
using Windows.UI.Xaml.Data;

namespace IpScanner.Ui.Convertors
{
    public class NavigationViewItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is NavigationViewItemInvokedEventArgs args && args.InvokedItemContainer is NavigationViewItem item)
            {
                return item.Tag?.ToString();
            }

            throw new ArgumentException("Invalid argument type", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
