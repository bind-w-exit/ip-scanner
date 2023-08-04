using IpScanner.Domain.Enums;
using System.Net;
using System.Net.NetworkInformation;

namespace IpScanner.Domain.Models
{
    public class ScannedDevice
    {
        private bool _favorite;

        public ScannedDevice(IPAddress ipAddress)
        {
            Status = DeviceStatus.Unknown;
            Name = string.Empty;
            Ip = ipAddress;
            Manufacturer = string.Empty;
            MacAddress = PhysicalAddress.None;
            Comments = string.Empty;
            _favorite = false;
        }

        public ScannedDevice(DeviceStatus status, string name, IPAddress ip, 
            string manufactor, PhysicalAddress macAddress, string comments)
        {
            Status = status;
            Name = name;
            Ip = ip;
            Manufacturer = manufactor;
            MacAddress = macAddress;
            Comments = comments;
            _favorite = false;
        }

        public ScannedDevice(DeviceStatus status, string name, IPAddress ip,
            string manufactor, PhysicalAddress macAddress, string comments, bool favorite)
        {
            Status = status;
            Name = name;
            Ip = ip;
            Manufacturer = manufactor;
            MacAddress = macAddress;
            Comments = comments;
            _favorite = favorite;
        }

        public DeviceStatus Status { get; private set; }
        public string Name { get; private set; }
        public IPAddress Ip { get; private set; }
        public string Manufacturer { get; private set; }
        public PhysicalAddress MacAddress { get; private set; }
        public string Comments { get; private set; }
        public bool Favorite { get => _favorite; }

        public bool MarkAsFavorite()
        {
            _favorite = true;
            return _favorite;
        }

        public bool UnmarkAsFavorite()
        {
            _favorite = false;
            return _favorite;
        }
    }
}
