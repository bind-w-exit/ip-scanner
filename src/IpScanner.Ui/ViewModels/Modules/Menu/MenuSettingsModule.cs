using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Ui.Pages;
using IpScanner.Ui.Services;
using System.Threading.Tasks;
using Windows.Globalization;

namespace IpScanner.Ui.ViewModels.Modules.Menu
{
    public class MenuSettingsModule : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalizationService _localizationService;
        private readonly IModalsService _modalsService;

        public MenuSettingsModule(INavigationService navigationService, ILocalizationService localizationService,
                       IModalsService modalsService)
        {
            _navigationService = navigationService;
            _localizationService = localizationService;
            _modalsService = modalsService;
        }

        public AsyncRelayCommand<string> ChangeLanguageCommand { get => new AsyncRelayCommand<string>(ChangeLanguageAsync); }

        public AsyncRelayCommand ShowOptionsDialogCommand => new AsyncRelayCommand(ShowOptionsDialog);

        private async Task ChangeLanguageAsync(string language)
        {
            await _localizationService.SetLanguageAsync(new Language(language));
            _navigationService.ReloadMainPage();
        }

        private async Task ShowOptionsDialog()
        {
            await _modalsService.ShowPageAsync(typeof(OptionsPage));
        }
    }
}
