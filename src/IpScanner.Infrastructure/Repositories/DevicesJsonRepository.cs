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
        private static readonly string FileName = "devices.json";

        public async Task SaveDevicesAsync(IEnumerable<ScannedDevice> devices)
        {
            var deviceEntities = devices.Select(device => device.ToEntity()).ToList();
            string jsonString = JsonSerializer.Serialize(deviceEntities);

            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            var file = await localFolder.CreateFileAsync(FileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            await Windows.Storage.FileIO.WriteTextAsync(file, jsonString);
        }

        public async Task<IEnumerable<ScannedDevice>> GetDevicesAsync()
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

        public async Task AddDeviceAsync(ScannedDevice device)
        {
            List<ScannedDevice> currentDevices = (await GetDevicesAsync()).ToList();
            currentDevices.Add(device);

            await SaveDevicesAsync(currentDevices);
        }

        public async Task RemoveDeviceAsync(ScannedDevice device)
        {
            List<ScannedDevice> currentDevices = (await GetDevicesAsync()).ToList();

            ScannedDevice deviceToRemove = currentDevices.FirstOrDefault(d => d.Ip.Equals(device.Ip));
            if (deviceToRemove == null)
            {
                throw new ArgumentException("Device not found");
            }

            currentDevices.Remove(deviceToRemove);
            await SaveDevicesAsync(currentDevices);
        }
    }
}
