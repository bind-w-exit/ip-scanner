using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace IpScanner.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private static readonly string FileName = "devices.json";

        public async Task<StorageFile> GetDefaultFileAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);

            return file;
        }

        public async Task<StorageFile> GetFileForReadingAsync(params string[] fileTypes)
        {
            StorageFile file = await GetFileFromOpenPickerAsync(fileTypes);
            if (file == null)
            {
                throw new OperationCanceledException("Cannot open file");
            }

            return file;
        }

        public async Task<StorageFile> GetFileForWritingAsync(params string[] fileTypes)
        {
            StorageFile file = await GetFileFromSavePickerAsync(fileTypes);
            if (file == null)
            {
                throw new OperationCanceledException("Cannot save file");
            }

            return file;
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

        private async Task<StorageFile> GetFileFromSavePickerAsync(params string[] fileTypes)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            foreach (var item in fileTypes)
            {
                savePicker.FileTypeChoices.Add(item, new List<string>() { item });
            }

            savePicker.SuggestedFileName = "items";
            return await savePicker.PickSaveFileAsync();
        }
    }
}
