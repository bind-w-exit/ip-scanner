using IpScanner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace IpScanner.Domain.Models
{
    public class ScannedDevice : IEquatable<ScannedDevice>
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

        public bool Equals(ScannedDevice other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Status == other.Status &&
                   Name == other.Name &&
                   Equals(Ip, other.Ip) &&
                   Manufacturer == other.Manufacturer &&
                   Equals(MacAddress, other.MacAddress) &&
                   Comments == other.Comments &&
                   Favorite == other.Favorite;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ScannedDevice);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Status.GetHashCode();
                hash = hash * 23 + (Name?.GetHashCode() ?? 0);
                hash = hash * 23 + (Ip?.GetHashCode() ?? 0);
                hash = hash * 23 + (Manufacturer?.GetHashCode() ?? 0);
                hash = hash * 23 + (MacAddress?.GetHashCode() ?? 0);
                hash = hash * 23 + (Comments?.GetHashCode() ?? 0);
                hash = hash * 23 + Favorite.GetHashCode();
                return hash;
            }
        }

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

        public override string ToString()
        {
            return $"{Status}: {Name}; {Ip}; {Manufacturer}; {MacAddress}; {Comments}";
        }
    }
}
