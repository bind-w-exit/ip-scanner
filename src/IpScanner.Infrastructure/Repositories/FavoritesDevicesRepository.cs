using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Mappers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.Repositories
{
    public class FavoritesDevicesRepository : IDeviceRepository
    {
        private const string FilePath = "favorites.json";

        public async Task SaveFavoritesDevicesAsync(IEnumerable<ScannedDevice> devices)
        {
            var deviceEntities = devices.Select(device => device.ToEntity());
            var serializedDevices = JsonConvert.SerializeObject(deviceEntities, Formatting.Indented);

            await File.WriteAllTextAsync(FilePath, serializedDevices);
        }

        public async Task<IEnumerable<ScannedDevice>> GetFavoritesDevicesAsync()
        {
            if (!File.Exists(FilePath)) return Enumerable.Empty<ScannedDevice>();

            var serializedDevices = await File.ReadAllTextAsync(FilePath);
            var deviceEntities = JsonConvert.DeserializeObject<IEnumerable<DeviceEntity>>(serializedDevices);

            return deviceEntities.Select(entity => entity.ToScannedDevice());
        }
    }

}
