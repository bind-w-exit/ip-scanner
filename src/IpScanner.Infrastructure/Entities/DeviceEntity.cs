using IpScanner.Domain.Enums;
using System.Net.NetworkInformation;
using System.Net;

namespace IpScanner.Infrastructure.Entities
{
    public class DeviceEntity
    {
        public DeviceStatus Status { get; set; }
        public string Name { get; set; }
        public IPAddress Ip { get; set; }
        public string Manufacturer { get; set; }
        public PhysicalAddress MacAddress { get; set; }
        public string Comments { get; set; }
    }
}
