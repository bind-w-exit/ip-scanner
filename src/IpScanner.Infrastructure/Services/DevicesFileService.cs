using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Provider;
using Windows.Storage;
using IpScanner.Domain.Enums;
using Windows.Storage.Pickers;
using System.Linq;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Mappers;
using IpScanner.Infrastructure.ContentFormatters.Factories;
using IpScanner.Infrastructure.ContentFormatters;
using IpScanner.Infrastructure.Extensions;

namespace IpScanner.Infrastructure.Services
{
    public class DevicesFileService : IFileService<ScannedDevice>
    {
        private readonly IContentCreatorFactory<ScannedDevice> _contentCreatorFactory;
        private readonly IContentFormatterFactory<DeviceEntity> _contentFormatterFactory;

        public DevicesFileService(IContentCreatorFactory<ScannedDevice> contentCreatorFactory,
            IContentFormatterFactory<DeviceEntity> contentFormatterFactory)
        {
            _contentCreatorFactory = contentCreatorFactory;
            _contentFormatterFactory = contentFormatterFactory;
        }

        public async Task<string> GetStringAsync()
        {
            StorageFile file = await GetFileFromOpenPickerAsync(".txt");
            if (file == null)
            {
                return string.Empty;
            }

            return await ReadContentFromFileAsync(file);
        }

        public async Task<IEnumerable<ScannedDevice>> GetItemsAsync()
        {
            StorageFile file = await GetFileFromOpenPickerAsync(".xml", ".json");
            if (file == null)
            {
                return Enumerable.Empty<ScannedDevice>();
            }

            string content = await ReadContentFromFileAsync(file);
            IContentFormatter<DeviceEntity> formatter = CreateContentFormatter(file.FileType);

            return DeserializeContent(content, formatter);
        }

        public async Task SaveItemsAsync(IEnumerable<ScannedDevice> devices)
        {
            StorageFile file = await GetFileFromPickerAsync();
            if (file == null) return;

            CachedFileManager.DeferUpdates(file);

            string content = GenerateFileContent(devices, file.FileType);
            await WriteContentToFileAsync(file, content);
        }

        private async Task<StorageFile> GetFileFromPickerAsync()
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            savePicker.FileTypeChoices.Add("JSON", new List<string>() { ".json" });
            savePicker.FileTypeChoices.Add("XML", new List<string>() { ".xml" });
            savePicker.FileTypeChoices.Add("CSV", new List<string>() { ".csv" });
            savePicker.FileTypeChoices.Add("HTML", new List<string>() { ".html" });

            savePicker.SuggestedFileName = "devices";

            return await savePicker.PickSaveFileAsync();
        }

        private string GenerateFileContent(IEnumerable<ScannedDevice> devices, string fileType)
        {
            ContentFormat format = fileType.GetContentFormatFromString();
            IContentCreator<ScannedDevice> contentCreator = _contentCreatorFactory.Create(format);

            return contentCreator.CreateContent(devices);
        }

        private async Task WriteContentToFileAsync(StorageFile file, string content)
        {
            CachedFileManager.DeferUpdates(file);
            await FileIO.WriteTextAsync(file, content);

            FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            if (status != FileUpdateStatus.Complete)
            {
                throw new OperationCanceledException("Cannot complete file saving");
            }
        }

        private async Task<StorageFile> GetFileFromOpenPickerAsync(params string[] fileTypes)
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            foreach (var item in fileTypes)
            {
                openPicker.FileTypeFilter.Add(item);
            }

            return await openPicker.PickSingleFileAsync();
        }

        private async Task<string> ReadContentFromFileAsync(StorageFile file)
        {
            return await FileIO.ReadTextAsync(file);
        }

        private IContentFormatter<DeviceEntity> CreateContentFormatter(string fileType)
        {
            ContentFormat format = fileType.GetContentFormatFromString();
            IContentFormatter<DeviceEntity> formatter = _contentFormatterFactory.Create(format);

            return formatter;
        }

        private IEnumerable<ScannedDevice> DeserializeContent(string content, IContentFormatter<DeviceEntity> contentFormatter)
        {
            IEnumerable<DeviceEntity> scannedDevices = contentFormatter.FormatContentAsCollection(content);
            return scannedDevices.Select(x => x.ToDomain());
        }
    }
}
