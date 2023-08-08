using CommunityToolkit.Mvvm.ComponentModel;
using IpScanner.Ui.Extensions;
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
            _colorThemeService.SetColorTheme(SelectedTheme.ToElementTheme());
        }
    }
}
