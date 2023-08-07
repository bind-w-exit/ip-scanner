using System;
using IpScanner.Domain.Enums;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.ContentCreators;

namespace IpScanner.Infrastructure.Factories
{
    public class DeviceContentCreatorFactory : IContentCreatorFactory<ScannedDevice>
    {
        private readonly DevicesJsonContentCreator _jsonContentCreator;
        private readonly DevicesXmlContentCreator _xmlContentCreator;
        private readonly DevicesCsvContentCreator _csvContentCreator;
        private readonly DevicesHtmlContentCreator _htmlContentCreator;

        public DeviceContentCreatorFactory(DevicesJsonContentCreator jsonContentCreator, DevicesXmlContentCreator xmlContentCreator, 
            DevicesCsvContentCreator csvContentCreator, DevicesHtmlContentCreator htmlContentCreator)
        {
            _jsonContentCreator = jsonContentCreator;
            _xmlContentCreator = xmlContentCreator;
            _csvContentCreator = csvContentCreator;
            _htmlContentCreator = htmlContentCreator;
        }

        public IContentCreator<ScannedDevice> Create(ContentFormat format)
        {
            switch (format)
            {
                case ContentFormat.Json:
                    return _jsonContentCreator;
                case ContentFormat.Xml:
                    return _xmlContentCreator;
                case ContentFormat.Csv:
                    return _csvContentCreator;
                case ContentFormat.Html:
                    return _htmlContentCreator;
                default:
                    throw new NotImplementedException($"Content creator for format {format} is not implemented.");
            }
        }
    }
}
