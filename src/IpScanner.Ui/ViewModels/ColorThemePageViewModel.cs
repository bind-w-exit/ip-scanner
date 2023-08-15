using CommunityToolkit.Mvvm.ComponentModel;
using IpScanner.Ui.Extensions;
using IpScanner.Ui.Services;

namespace IpScanner.Ui.ViewModels
{
    public class ColorThemePageViewModel : ObservableObject
    {
        private string _selectedTheme;
        private readonly AppSettings _appSettings;
        private readonly IColorThemeService _colorThemeService;

        public ColorThemePageViewModel(IColorThemeService colorThemeService, ISettingsService settingsService)
        {
            _appSettings = settingsService.Settings;

            _colorThemeService = colorThemeService;
            SelectedTheme = _appSettings.ColorTheme;
        }

        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if(SetProperty(ref _selectedTheme, value))
                {
                    ChangeTheme();
                }
            }
        }

        private void ChangeTheme()
        {
            _appSettings.ColorTheme = SelectedTheme;
            _colorThemeService.SetColorTheme(SelectedTheme.ToElementTheme());
        }
    }
}
