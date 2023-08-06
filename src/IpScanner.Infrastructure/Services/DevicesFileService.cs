using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Provider;
using Windows.Storage;
using IpScanner.Domain.Enums;
using Windows.Storage.Pickers;

namespace IpScanner.Infrastructure.Services
{
    public class DevicesFileService : IFileService<ScannedDevice>
    {
        private readonly IContentCreatorFactory<ScannedDevice> _contentCreatorFactory;

        public DevicesFileService(IContentCreatorFactory<ScannedDevice> contentCreatorFactory)
        {
            _contentCreatorFactory = contentCreatorFactory;
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
    }
}
