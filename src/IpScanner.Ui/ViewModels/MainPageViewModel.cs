using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Enums;
using IpScanner.Domain.Models;
using IpScanner.Ui.Messages;
using IpScanner.Ui.ObjectModels;
using IpScanner.Ui.Services;
using System.Threading.Tasks;
using Windows.Globalization;

namespace IpScanner.Ui.ViewModels
{
    public class MainPageViewModel : ValidationViewModel
    {
        private bool _showUnknown;
        private bool _showOnline;
        private bool _showOffline;
        private bool _showDetails;
        private readonly INavigationService _navigationService;
        private readonly ILocalizationService _localizationService;
        private readonly IMessenger _messenger;
        private readonly ItemFilter<ScannedDevice> _unknownFilter = new ItemFilter<ScannedDevice>(device => device.Status != DeviceStatus.Unknown);
        private readonly ItemFilter<ScannedDevice> _onlineFilter = new ItemFilter<ScannedDevice>(device => device.Status != DeviceStatus.Online);
        private readonly ItemFilter<ScannedDevice> _offlineFilter = new ItemFilter<ScannedDevice>(device => device.Status != DeviceStatus.Offline);

        public MainPageViewModel(INavigationService navigationService, ILocalizationService localizationService,
            IMessenger messenger)
        {
            _messenger = messenger;

            ShowUnknown = false;
            ShowOnline = true;
            ShowOffline = true;
            ShowDetails = false;

            _navigationService = navigationService;
            _localizationService = localizationService;
        }

        public bool ShowUnknown
        {
            get => _showUnknown;
            set
            {
                _messenger.Send(new FilterMessage(_unknownFilter, !value));
                SetProperty(ref _showUnknown, value);
            }
        }

        public bool ShowOnline
        {
            get => _showOnline;
            set
            {
                _messenger.Send(new FilterMessage(_onlineFilter, !value));
                SetProperty(ref _showOnline, value);
            }
        }

        public bool ShowOffline
        {
            get => _showOffline;
            set
            {
                _messenger.Send(new FilterMessage(_offlineFilter, !value));
                SetProperty(ref _showOffline, value);
            }
        }

        public bool ShowDetails
        {
            get => _showDetails;
            set
            {
                _messenger.Send(new DetailsPageVisibilityMessage(value));
                SetProperty(ref _showDetails, value);
            }
        }

        public AsyncRelayCommand<string> ChangeLanguageCommand { get => new AsyncRelayCommand<string>(ChangeLanguageAsync); }

        public RelayCommand ShowOnlineCommand { get => new RelayCommand(() => ShowOnline = !ShowOnline); }

        public RelayCommand ShowOfflineCommand { get => new RelayCommand(() => ShowOffline = !ShowOffline); }

        public RelayCommand ShowUnknownCommand { get => new RelayCommand(() => ShowUnknown = !ShowUnknown); }

        public RelayCommand ShowDetailsPageCommand { get => new RelayCommand(() => ShowDetails = !ShowDetails); }

        private async Task ChangeLanguageAsync(string language)
        {
            await _localizationService.SetLanguageAsync(new Language(language));
            _navigationService.ReloadMainPage();
        }
    }
}
