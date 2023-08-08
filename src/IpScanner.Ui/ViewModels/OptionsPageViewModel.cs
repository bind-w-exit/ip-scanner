using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Ui.Pages;
using IpScanner.Ui.Services;
using System;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.ViewModels
{
    public class OptionsPageViewModel : ObservableObject
    {
        private readonly Frame _frame;
        private readonly INavigationService _navigationService;

        public OptionsPageViewModel(Frame frame, INavigationService navigationService)
        {
            _frame = frame;
            _navigationService = navigationService;

            _navigationService.NavigateToPage(_frame, typeof(ColorThemePage));
        }

        public RelayCommand<NavigationViewItemInvokedEventArgs> NavigateCommand { get => new RelayCommand<NavigationViewItemInvokedEventArgs>(Navigate); }

        private void Navigate(NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem item)
            {
                string tag = item.Tag as string;
                switch (tag)
                {
                    case "ColorTheme":
                        _navigationService.NavigateToPage(_frame, typeof(ColorThemePage));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
