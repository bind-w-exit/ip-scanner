using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Provider;
using Windows.Storage;
using IpScanner.Domain.Enums;

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
            var savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
            };

            savePicker.FileTypeChoices.Add("JSON", new List<string>() { ".json" });
            savePicker.SuggestedFileName = "devices";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);

                IContentCreator<ScannedDevice> contentCreator = null;
                switch (file.FileType)
                {
                    case ".json":
                        contentCreator = _contentCreatorFactory.Create(ContentFormat.Json);
                        break;
                    default:
                        throw new Exception("Unsupported file type");
                }

                string content = contentCreator.CreateContent(devices);
                await FileIO.WriteTextAsync(file, content);

                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status != FileUpdateStatus.Complete)
                {
                    throw new OperationCanceledException("Can not complete file saving");
                }
            }
        }
    }
}
