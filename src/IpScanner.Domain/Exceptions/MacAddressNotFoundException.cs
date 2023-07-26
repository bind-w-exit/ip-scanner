using System;
using System.Net;

namespace IpScanner.Domain.Exceptions
{
    public class MacAddressNotFoundException : Exception
    {
        public MacAddressNotFoundException(IPAddress ipAddress) : base()
        {
            ScannedIpAddress = ipAddress;
        }

        public MacAddressNotFoundException(IPAddress ipAddress, string message) : base(message)
        {
            ScannedIpAddress = ipAddress;
        }

        public MacAddressNotFoundException(IPAddress ipAddress, string message, Exception innerException) : base(message, innerException)
        {
            ScannedIpAddress = ipAddress;
        }

        public IPAddress ScannedIpAddress { get; private set; }
    }
}
