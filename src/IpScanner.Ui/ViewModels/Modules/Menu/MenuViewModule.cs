using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Enums;
using IpScanner.Domain.Models;
using IpScanner.Ui.Messages;
using IpScanner.Ui.ObjectModels;
using IpScanner.Ui.Services;

namespace IpScanner.Ui.ViewModels.Modules.Menu
{
    public class MenuViewModule : ObservableObject
    {
        private bool _showUnknown;
        private bool _showOnline;
        private bool _showOffline;
        private bool _showDetails;
        private bool _showMiscellaneous;
        private bool _showActions;
        private readonly IMessenger _messenger;
        private readonly AppSettings _appSettings;
        private readonly ItemFilter<ScannedDevice> _unknownFilter = new ItemFilter<ScannedDevice>(device => device.Status != DeviceStatus.Unknown);
        private readonly ItemFilter<ScannedDevice> _onlineFilter = new ItemFilter<ScannedDevice>(device => device.Status != DeviceStatus.Online);
        private readonly ItemFilter<ScannedDevice> _offlineFilter = new ItemFilter<ScannedDevice>(device => device.Status != DeviceStatus.Offline);

        public MenuViewModule(IMessenger messenger, ISettingsService settingsService)
        {
            _messenger = messenger;
            _appSettings = settingsService.Settings;

            ShowUnknown = _appSettings.ShowUnknown;
            ShowOnline = _appSettings.ShowOnline;
            ShowOffline = _appSettings.ShowOffline;
            ShowDetails = false;
            ShowMiscellaneous = true;
            ShowActions = true;
        }

        public bool ShowUnknown
        {
            get => _showUnknown;
            set
            {
                _messenger.Send(new FilterMessage(_unknownFilter, !value));
                if(SetProperty(ref _showUnknown, value))
                {
                    _appSettings.ShowUnknown = value;
                }
            }
        }

        public bool ShowOnline
        {
            get => _showOnline;
            set
            {
                _messenger.Send(new FilterMessage(_onlineFilter, !value));
                if (SetProperty(ref _showOnline, value))
                {
                    _appSettings.ShowOnline = value;
                }
            }
        }

        public bool ShowOffline
        {
            get => _showOffline;
            set
            {
                _messenger.Send(new FilterMessage(_offlineFilter, !value));
                if(SetProperty(ref _showOffline, value))
                {
                    _appSettings.ShowOffline = value;
                }
            }
        }

        public bool ShowDetails
        {
            get => _showDetails;
            set
            {
                if (SetProperty(ref _showDetails, value))
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
                if (SetProperty(ref _showMiscellaneous, value))
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
                if (SetProperty(ref _showActions, value))
                {
                    _messenger.Send(new ActionsBarVisibilityMessage(value));
                }
            }
        }

        public RelayCommand ShowOnlineCommand { get => new RelayCommand(() => ShowOnline = !ShowOnline); }

        public RelayCommand ShowOfflineCommand { get => new RelayCommand(() => ShowOffline = !ShowOffline); }

        public RelayCommand ShowUnknownCommand { get => new RelayCommand(() => ShowUnknown = !ShowUnknown); }

        public RelayCommand ShowDetailsPageCommand { get => new RelayCommand(() => ShowDetails = !ShowDetails); }

        public RelayCommand ShowMiscellaneousCommand { get => new RelayCommand(() => ShowMiscellaneous = !ShowMiscellaneous); }

        public RelayCommand ShowActionsCommand { get => new RelayCommand(() => ShowActions = !ShowActions); }
    }
}
