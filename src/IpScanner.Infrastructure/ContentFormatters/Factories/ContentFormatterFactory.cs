using IpScanner.Domain.Enums;
using System;

namespace IpScanner.Infrastructure.ContentFormatters.Factories
{
    public class ContentFormatterFactory<T> : IContentFormatterFactory<T>
    {
        public IContentFormatter<T> Create(ContentFormat format)
        {
            switch(format)
            {
                case ContentFormat.Json:
                    return new JsonContentFormatter<T>();
                case ContentFormat.Xml:
                    return new XmlContentFormatter<T>();
                default:
                    throw new InvalidOperationException("Invalid content format");
            }
        }
    }
}
