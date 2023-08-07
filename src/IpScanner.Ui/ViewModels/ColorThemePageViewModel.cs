using CommunityToolkit.Mvvm.ComponentModel;
using IpScanner.Ui.Services;
using Windows.UI.Xaml;

namespace IpScanner.Ui.ViewModels
{
    public class ColorThemePageViewModel : ObservableObject
    {
        private string _selectedTheme;
        private INavigationService _navigationService;

        public ColorThemePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            SelectedTheme = string.Empty;
        }

        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                SetProperty(ref _selectedTheme, value);
                ChangeTheme();
            }
        }

        private void ChangeTheme()
        {
            switch (_selectedTheme)
            {
                case "Light":
                    _navigationService.ChangeColorTheme(ElementTheme.Light);
                    break;
                case "Dark":
                    _navigationService.ChangeColorTheme(ElementTheme.Dark);
                    break;
            }

            _navigationService.ReloadMainPage();
        }
    }
}
