﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Ui.Messages;
using System.Threading.Tasks;
using System;
using Windows.Storage;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Exceptions;
using IpScanner.Infrastructure.Repositories;
using System.Collections.Generic;
using IpScanner.Infrastructure.Services;
using IpScanner.Ui.Services;
using IpScanner.Infrastructure.Repositories.Factories;
using IpScanner.Infrastructure.Extensions;

namespace IpScanner.Ui.ViewModels.Modules.Menu
{
    public class MenuFileModule : ObservableObject
    {
        private readonly IMessenger _messenger;
        private readonly IFileService _fileService;
        private readonly IApplicationService _applicationService;
        private readonly ILocalizationService _localizationService;
        private readonly IDialogService _dialogService;
        private readonly ScanningModule _scanningModule;
        private readonly IDeviceRepositoryFactory _deviceRepositoryFactory;

        public MenuFileModule(IMessenger messanger, IFileService fileService, IApplicationService applicationService, 
            ILocalizationService localizationService, IDialogService dialogService, IDeviceRepositoryFactory deviceRepositoryFactory, 
            ScanningModule scanningModule)
        {
            _messenger = messanger;
            _fileService = fileService;
            _applicationService = applicationService;
            _localizationService = localizationService;
            _dialogService = dialogService;
            _deviceRepositoryFactory = deviceRepositoryFactory;
            _scanningModule = scanningModule;
        }

        public AsyncRelayCommand ScanFromFileCommand { get => new AsyncRelayCommand(ScanFromFileAsync); }

        public AsyncRelayCommand SaveDevicesCommand { get => new AsyncRelayCommand(SaveDevicesAsync); }

        public AsyncRelayCommand LoadFavoritesCommand { get => new AsyncRelayCommand(LoadDevicesAsync); }

        public RelayCommand ExitCommand { get => new RelayCommand(ExitFromApplication); }

        public RelayCommand PrintPreviewCommand { get => new RelayCommand(ShowPrintPreview); }

        private async Task ScanFromFileAsync()
        {
            try
            {
                StorageFile file = await _fileService.GetFileForReadingAsync(".txt");

                string content = await file.ReadTextAsync();
                _messenger.Send(new ScanFromFileMessage(content));
            }
            catch (OperationCanceledException)
            { }
        }

        private async Task SaveDevicesAsync()
        {
            List<ScannedDevice> devices = _scanningModule.Devices;

            try
            {
                StorageFile file = await _fileService.GetFileForWritingAsync(".xml", ".json", ".csv", ".html");
                IDeviceRepository deviceRepository = _deviceRepositoryFactory.CreateWithFile(file);
                await deviceRepository.SaveDevicesAsync(devices);
            }
            catch (OperationCanceledException)
            { }
        }

        private async Task LoadDevicesAsync()
        {
            try
            {
                StorageFile file = await _fileService.GetFileForReadingAsync(".xml", ".json");
                _messenger.Send(new DevicesLoadedMessage(file));
            }
            catch (OperationCanceledException)
            { }
            catch (ContentFormatException)
            {
                string message = _localizationService.GetLocalizedString("WrongFile");
                await _dialogService.ShowMessageAsync("Error", message);
            }
        }

        private void ShowPrintPreview()
        {
            _messenger.Send(new PrintPreviewMessage());
        }

        private void ExitFromApplication()
        {
            _applicationService.Exit();
        }
    }
}