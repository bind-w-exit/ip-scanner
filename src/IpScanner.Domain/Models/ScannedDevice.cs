using IpScanner.Domain.Enums;
using System.Net;
using System.Net.NetworkInformation;

namespace IpScanner.Domain.Models
{
    public class ScannedDevice
    {
        public ScannedDevice(IPAddress ipAddress)
        {
            Status = DeviceStatus.Unknown;
            Name = ipAddress.ToString();
            Ip = ipAddress;
            Manufactor = string.Empty;
            MacAddress = PhysicalAddress.None;
            Comments = string.Empty;
        }

        public ScannedDevice(DeviceStatus status, string name, IPAddress ip, 
            string manufactor, PhysicalAddress macAddress, string comments)
        {
            Status = status;
            Name = name;
            Ip = ip;
            Manufactor = manufactor;
            MacAddress = macAddress;
            Comments = comments;
        }

        public DeviceStatus Status { get; private set; }
        public string Name { get; private set; }
        public IPAddress Ip { get; private set; }
        public string Manufactor { get; private set; }
        public PhysicalAddress MacAddress { get; private set; }
        public string Comments { get; private set; }

        public override string ToString()
        {
            return $"{Status} {Name} {Ip} {Manufactor} {MacAddress} {Comments}";
        }
    }
}
