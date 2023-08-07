using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Entities;
using System.Net;
using System.Net.NetworkInformation;

namespace IpScanner.Infrastructure.Mappers
{
    public static class DeviceMapper
    {
        public static DeviceEntity ToEntity(this ScannedDevice domain)
        {
            return new DeviceEntity
            {
                Status = domain.Status,
                Name = domain.Name,
                Ip = domain.Ip.ToString(),
                Manufacturer = domain.Manufacturer,
                MacAddress = domain.MacAddress.ToString(),
                Comments = domain.Comments,
                Favorite = domain.Favorite
            };
        }

        public static ScannedDevice ToDomain(this DeviceEntity entity)
        {
            PhysicalAddress macAddress = ParseMacAddress(entity.MacAddress);

            return new ScannedDevice(entity.Status, entity.Name, IPAddress.Parse(entity.Ip),
                entity.Manufacturer, macAddress, entity.Comments, entity.Favorite);
        }

        private static PhysicalAddress ParseMacAddress(string macAddress)
        {
            return PhysicalAddress.None.ToString() == macAddress
                ? PhysicalAddress.None
                : PhysicalAddress.Parse(macAddress);
        }
    }
}
