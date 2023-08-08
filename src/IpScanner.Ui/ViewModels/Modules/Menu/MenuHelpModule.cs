using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Infrastructure.Services;
using IpScanner.Ui.Services;
using System;
using System.Threading.Tasks;

namespace IpScanner.Ui.ViewModels.Modules.Menu
{
    public class MenuHelpModule : ObservableObject
    {
        private readonly IBrowserService _browserService;
        private readonly IDialogService _dialogService;

        public MenuHelpModule(IBrowserService browserService, IDialogService dialogService)
        {
            _browserService = browserService;
            _dialogService = dialogService;
        }

        public AsyncRelayCommand OpenContentsCommand => new AsyncRelayCommand(OpenContentsAsync);

        public AsyncRelayCommand OpenBugReportCommand => new AsyncRelayCommand(OpenBugReportAsync);

        public AsyncRelayCommand OpenRequestFeatureCommand => new AsyncRelayCommand(OpenRequestFeatureAsync);

        public AsyncRelayCommand OpenCommunityCommand => new AsyncRelayCommand(OpenCommunityAsync);

        public AsyncRelayCommand OpenAboutCommand => new AsyncRelayCommand(OpenAboutAsync);

        public async Task OpenContentsAsync()
        {
            await _browserService.OpenBrowserAsync(new Uri("http://www.advanced-ip-scanner.com/link.php?lng=en&ver=2-5-4594-1&beta=n&page=help"));
        }

        public async Task OpenBugReportAsync()
        {
            await _browserService.OpenBrowserAsync(new Uri("https://www.advanced-ip-scanner.com/support/?category=bug&lng=en&ver=2-5-4594-1&beta=n"));
        }

        public async Task OpenRequestFeatureAsync()
        {
            await _browserService.OpenBrowserAsync(new Uri("http://www.advanced-ip-scanner.com/link.php?lng=en&ver=2-5-4594-1&beta=n&page=feature"));
        }

        public async Task OpenCommunityAsync()
        {
            await _browserService.OpenBrowserAsync(new Uri("http://www.advanced-ip-scanner.com/link.php?lng=en&ver=2-5-4594-1&beta=n&page=community"));
        }

        public async Task OpenAboutAsync()
        {
            await _dialogService.ShowMessageAsync("About", "Advanced IP Scanner 2.5.4594.1");
        }
    }
}
