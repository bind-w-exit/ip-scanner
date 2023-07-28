using System;

namespace IpScanner.Domain.Exceptions
{
    public class IpValidationException : Exception
    {
        public IpValidationException() : base("Wrong format for IP range")
        { }

        public IpValidationException(string message) : base(message)
        { }

        public IpValidationException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
