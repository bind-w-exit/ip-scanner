using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Models;
using IpScanner.Ui.Messages;
using System.Net;

namespace IpScanner.Ui.ViewModels
{
    public class DetailsPageViewModel : ObservableObject
    {
        private ScannedDevice _device;

        public DetailsPageViewModel(IMessenger messenger)
        {
            _device = new ScannedDevice(IPAddress.Any);

            RegisterMessages(messenger);
        }

        public ScannedDevice Device
        {
            get => _device;
            set => SetProperty(ref _device, value);
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<DeviceSelectedMessage>(this, OnDeviceChanged);
        }

        private void OnDeviceChanged(object sender, DeviceSelectedMessage message)
        {
            Device = message.Device;
        }
    }
}
