using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Ui.Pages;
using IpScanner.Ui.Services;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.ViewModels
{
    public class OptionsDialogViewModel : ObservableObject
    {
        private readonly Frame _frame;
        private readonly INavigationService _navigationService;

        public OptionsDialogViewModel(Frame frame, INavigationService navigationService)
        {
            _frame = frame;
            _navigationService = navigationService;
        }

        public RelayCommand NavigateToColorThemePageCommand => new RelayCommand(NavigateToColorThemePage);

        private void NavigateToColorThemePage()
        {
            _navigationService.NavigateToPage(_frame, typeof(ColorThemePage));
        }
    }
}
