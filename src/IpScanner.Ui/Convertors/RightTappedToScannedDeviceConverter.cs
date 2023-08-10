using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using IpScanner.Domain.Models;

namespace IpScanner.Ui.Convertors
{
    public class RightTappedToScannedDeviceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = value as RightTappedRoutedEventArgs;
            var originalSource = (FrameworkElement)args?.OriginalSource;
            return (FindParent<DataGridRow>(originalSource)).DataContext as ScannedDevice;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }
    }
}
