using System;
using System.Net;

namespace IpScanner.Domain.Exceptions
{
    public class HostNotFoundException : Exception
    {
        public HostNotFoundException(IPAddress destination) : base()
        {
            Destination = destination;
        }

        public HostNotFoundException(IPAddress destination, string message) : base(message)
        { 
            Destination = destination;
        }

        public HostNotFoundException(IPAddress destination, string message, Exception innerException) : base(message, innerException)
        { 
            Destination = destination;
        }

        public IPAddress Destination { get; }
    }
}
