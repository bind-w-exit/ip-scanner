using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Repositories;
using IpScanner.Infrastructure.Repositories.Factories;
using IpScanner.Infrastructure.Services;
using IpScanner.Ui.Messages;
using IpScanner.Ui.Printing;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace IpScanner.Ui.ViewModels
{
    public class DetailsPageViewModel : ObservableObject
    {
        private ScannedDevice _device;
        private readonly IPrintService<ScannedDevice> _printService;
        private readonly IFileService _fileService;
        private readonly IDeviceRepositoryFactory _deviceRepositoryFactory;

        public DetailsPageViewModel(IMessenger messenger, IPrintService<ScannedDevice> printService, 
            IFileService fileService, IDeviceRepositoryFactory deviceRepositoryFactory)
        {
            _device = new ScannedDevice(IPAddress.Any);
            _printService = printService;
            _fileService = fileService;
            _deviceRepositoryFactory = deviceRepositoryFactory;

            RegisterMessages(messenger);
        }

        public ScannedDevice Device
        {
            get => _device;
            set => SetProperty(ref _device, value);
        }

        public ICommand ShowPrintPreviewCommand => new AsyncRelayCommand(ShowPrintPreviewAsync);

        public ICommand SaveDeviceCommand => new AsyncRelayCommand(SaveDeviceAsync);

        private async Task ShowPrintPreviewAsync()
        {
            await _printService.ShowPrintUIAsync(new List<ScannedDevice> { _device });
        }

        private async Task SaveDeviceAsync()
        {
            StorageFile file = await _fileService.GetFileForWritingAsync(".json", ".xml", ".csv", ".html");
            if (file == null || _device == null)
            {
                return;
            }

            IDeviceRepository repository = _deviceRepositoryFactory.CreateWithFile(file);
            await repository.SaveDevicesAsync(new List<ScannedDevice> { _device });
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
