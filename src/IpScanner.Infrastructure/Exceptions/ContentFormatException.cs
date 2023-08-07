using System;

namespace IpScanner.Infrastructure.Exceptions
{
    public class ContentFormatException : Exception
    {
        public ContentFormatException() : base()
        { }

        public ContentFormatException(string message) : base(message)
        { }

        public ContentFormatException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
