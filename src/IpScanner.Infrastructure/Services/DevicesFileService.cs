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
using System.Text.Json;

namespace IpScanner.Infrastructure.Services
{
    public class DevicesFileService : IFileService<ScannedDevice>
    {
        private readonly IContentCreatorFactory<ScannedDevice> _contentCreatorFactory;

        public DevicesFileService(IContentCreatorFactory<ScannedDevice> contentCreatorFactory)
        {
            _contentCreatorFactory = contentCreatorFactory;
        }

        public async Task<IEnumerable<ScannedDevice>> GetItemsAsync()
        {
            StorageFile file = await GetFileFromOpenPickerAsync();
            if (file == null) return Enumerable.Empty<ScannedDevice>();

            string content = await ReadContentFromFileAsync(file);
            return DeserializeContent(content);
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
            IContentCreator<ScannedDevice> contentCreator = null;

            switch (fileType)
            {
                case ".json":
                    contentCreator = _contentCreatorFactory.Create(ContentFormat.Json);
                    break;
                case ".xml":
                    contentCreator = _contentCreatorFactory.Create(ContentFormat.Xml);
                    break;
                case ".csv":
                    contentCreator = _contentCreatorFactory.Create(ContentFormat.Csv);
                    break;
                case ".html":
                    contentCreator = _contentCreatorFactory.Create(ContentFormat.Html);
                    break;
                default:
                    throw new Exception("Unsupported file type");
            }

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

        private async Task<StorageFile> GetFileFromOpenPickerAsync()
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            openPicker.FileTypeFilter.Add(".json");

            return await openPicker.PickSingleFileAsync();
        }

        private async Task<string> ReadContentFromFileAsync(StorageFile file)
        {
            return await FileIO.ReadTextAsync(file);
        }

        private IEnumerable<ScannedDevice> DeserializeContent(string content)
        {
            var scannedDevices = JsonSerializer.Deserialize<IEnumerable<DeviceEntity>>(content);
            return scannedDevices.Select(x => x.ToDomain());
        }
    }
}
