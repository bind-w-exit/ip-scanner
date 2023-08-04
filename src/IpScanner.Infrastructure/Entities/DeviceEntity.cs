using IpScanner.Domain.Enums;
using System.Net.NetworkInformation;
using System.Net;

namespace IpScanner.Infrastructure.Entities
{
    public class DeviceEntity
    {
        public DeviceStatus Status { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Manufacturer { get; set; }
        public string MacAddress { get; set; }
        public string Comments { get; set; }
    }
}
