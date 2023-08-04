using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Mappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.Repositories
{
    public class DevicesJsonRepository : IDeviceRepository
    {
        private static readonly string FileName = "favorites.json";

        public async Task SaveFavoritesDevicesAsync(IEnumerable<ScannedDevice> devices)
        {
            try
            {
                var deviceEntities = devices.Select(device => device.ToEntity()).ToList();
                string jsonString = JsonSerializer.Serialize(deviceEntities);

                var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

                var file = await localFolder.CreateFileAsync(FileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);

                await Windows.Storage.FileIO.WriteTextAsync(file, jsonString);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ScannedDevice>> GetFavoritesDevicesAsync()
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                var file = await localFolder.GetFileAsync(FileName);

                string jsonString = await Windows.Storage.FileIO.ReadTextAsync(file);

                var deviceEntities = JsonSerializer.Deserialize<List<DeviceEntity>>(jsonString);

                var scannedDevices = deviceEntities.Select(entity => entity.ToDomain());

                return scannedDevices;
            }
            catch (FileNotFoundException)
            {
                return Enumerable.Empty<ScannedDevice>();
            }
        }
    }

}
