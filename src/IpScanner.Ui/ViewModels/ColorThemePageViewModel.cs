using CommunityToolkit.Mvvm.ComponentModel;
using IpScanner.Ui.Services;
using Windows.UI.Xaml;

namespace IpScanner.Ui.ViewModels
{
    public class ColorThemePageViewModel : ObservableObject
    {
        private string _selectedTheme;
        private readonly IColorThemeService _colorThemeService;

        public ColorThemePageViewModel(IColorThemeService colorThemeService)
        {
            _colorThemeService = colorThemeService;
            SelectedTheme = _colorThemeService.GetColorTheme().ToString();
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
                    _colorThemeService.SetColorTheme(ElementTheme.Light);
                    break;
                case "Dark":
                    _colorThemeService.SetColorTheme(ElementTheme.Dark);
                    break;
                case "Default":
                    _colorThemeService.SetColorTheme(ElementTheme.Default);
                    break;
            }
        }
    }
}
