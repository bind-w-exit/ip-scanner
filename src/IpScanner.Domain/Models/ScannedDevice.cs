using System.Net;

namespace IpScanner.Domain.Models
{
    public class ScannedDevice
    {
        public ScannedDevice(string status, string name, IPAddress ip, string manufactor, string macAddress, string comments)
        {
            Status = status;
            Name = name;
            Ip = ip;
            Manufactor = manufactor;
            MacAddress = macAddress;
            Comments = comments;
        }

        public string Status { get; private set; }
        public string Name { get; private set; }
        public IPAddress Ip { get; private set; }
        public string Manufactor { get; private set; }
        public string MacAddress { get; private set; }
        public string Comments { get; private set; }

        public override string ToString()
        {
            return $"{Status} {Name} {Ip} {Manufactor} {MacAddress} {Comments}";
        }
    }
}
