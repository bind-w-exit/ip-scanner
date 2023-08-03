using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Entities;

namespace IpScanner.Infrastructure.Mappers
{
    internal static class DeviceMapper
    {
        public static DeviceEntity ToEntity(this ScannedDevice domain)
        {
            return new DeviceEntity
            {
                Status = domain.Status,
                Name = domain.Name,
                Ip = domain.Ip,
                Manufacturer = domain.Manufacturer,
                MacAddress = domain.MacAddress,
                Comments = domain.Comments
            };
        }

        public static ScannedDevice ToScannedDevice(this DeviceEntity entity)
        {
            return new ScannedDevice(entity.Status, entity.Name, entity.Ip,
                entity.Manufacturer, entity.MacAddress, entity.Comments);
        }
    }
}
