using IpScanner.Domain.Enums;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.ContentFormatters;
using IpScanner.Infrastructure.ContentFormatters.Factories;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Extensions;
using IpScanner.Infrastructure.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace IpScanner.Infrastructure.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly StorageFile _file;
        private readonly IContentCreatorFactory<ScannedDevice> _contentCreatorFactory;
        private readonly IContentFormatterFactory<DeviceEntity> _contentFormatterFactory;

        public DeviceRepository(StorageFile file, IContentCreatorFactory<ScannedDevice> contentCreatorFactory,
            IContentFormatterFactory<DeviceEntity> contentFormatterFactory)
        {
            _file = file;

            _contentCreatorFactory = contentCreatorFactory;
            _contentFormatterFactory = contentFormatterFactory;
        }

        public async Task<IEnumerable<ScannedDevice>> GetDevicesAsync()
        {
            string content = await FileIO.ReadTextAsync(_file);
            if(string.IsNullOrEmpty(content))
            {
                return Enumerable.Empty<ScannedDevice>();
            }

            IContentFormatter<DeviceEntity> formatter = CreateContentFormatter(_file.FileType);
            return DeserializeContent(content, formatter);
        }

        public async Task SaveDevicesAsync(IEnumerable<ScannedDevice> devices)
        {
            string content = GenerateFileContent(devices, _file.FileType);
            await FileIO.WriteTextAsync(_file, content);
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

        public async Task UpdateDeviceAsync(ScannedDevice device)
        {
            List<ScannedDevice> currentDevices = (await GetDevicesAsync()).ToList();

            ScannedDevice destination = currentDevices.FirstOrDefault(d => d.Ip.Equals(device.Ip));
            if (destination == null)
            {
                throw new ArgumentException("Device not found");
            }

            currentDevices.Remove(destination);
            currentDevices.Add(device);

            await SaveDevicesAsync(currentDevices);
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

        private string GenerateFileContent(IEnumerable<ScannedDevice> devices, string fileType)
        {
            ContentFormat format = fileType.GetContentFormatFromString();
            IContentCreator<ScannedDevice> contentCreator = _contentCreatorFactory.Create(format);

            return contentCreator.CreateContent(devices);
        }
    }
}
