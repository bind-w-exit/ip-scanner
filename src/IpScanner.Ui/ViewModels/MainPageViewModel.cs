using CommunityToolkit.Mvvm.ComponentModel;
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
    public class MainPageViewModel : ObservableObject
    {
        private bool _showUnknown;
        private bool _showOnline;
        private bool _showOffline;
        private bool _showDetails;
        private bool _showMiscellaneous;
        private bool _showActions;
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
            ShowMiscellaneous = true;
            ShowActions = true;

            _navigationService = navigationService;
            _localizationService = localizationService;
        }

        public bool ShowUnknown
        {
            get => _showUnknown;
            set
            {
                if(SetProperty(ref _showUnknown, value))
                {
                    _messenger.Send(new FilterMessage(_unknownFilter, !value));
                }
            }
        }

        public bool ShowOnline
        {
            get => _showOnline;
            set
            {
                if(SetProperty(ref _showOnline, value))
                {
                    _messenger.Send(new FilterMessage(_onlineFilter, !value));
                }
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
                if(SetProperty(ref _showDetails, value))
                {
                    _messenger.Send(new DetailsPageVisibilityMessage(value));
                }
            }
        }

        public bool ShowMiscellaneous
        {
            get => _showMiscellaneous;
            set
            {
                if(SetProperty(ref _showMiscellaneous, value))
                {
                    _messenger.Send(new MiscellaneousBarVisibilityMessage(value));
                }
            }
        }

        public bool ShowActions
        {
            get => _showActions;
            set
            {
                if(SetProperty(ref _showActions, value))
                {
                    _messenger.Send(new ActionsBarVisibilityMessage(value));
                }
            }
        }

        public AsyncRelayCommand<string> ChangeLanguageCommand { get => new AsyncRelayCommand<string>(ChangeLanguageAsync); }

        public RelayCommand ShowOnlineCommand { get => new RelayCommand(() => ShowOnline = !ShowOnline); }

        public RelayCommand ShowOfflineCommand { get => new RelayCommand(() => ShowOffline = !ShowOffline); }

        public RelayCommand ShowUnknownCommand { get => new RelayCommand(() => ShowUnknown = !ShowUnknown); }

        public RelayCommand ShowDetailsPageCommand { get => new RelayCommand(() => ShowDetails = !ShowDetails); }

        public RelayCommand ShowMiscellaneousCommand { get => new RelayCommand(() => ShowMiscellaneous = !ShowMiscellaneous); }

        public RelayCommand ShowActionsCommand { get => new RelayCommand(() => ShowActions = !ShowActions); }

        private async Task ChangeLanguageAsync(string language)
        {
            await _localizationService.SetLanguageAsync(new Language(language));
            _navigationService.ReloadMainPage();
        }
    }
}
