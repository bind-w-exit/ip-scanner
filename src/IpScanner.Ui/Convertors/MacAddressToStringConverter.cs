using IpScanner.Infrastructure.Extensions;
using System;
using System.Net.NetworkInformation;
using Windows.UI.Xaml.Data;

namespace IpScanner.Ui.Convertors
{
    public class MacAddressToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var macAddress = value as PhysicalAddress;
            if (macAddress == null)
            {
                return string.Empty;
            }

            return macAddress.ToFormattedString();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
