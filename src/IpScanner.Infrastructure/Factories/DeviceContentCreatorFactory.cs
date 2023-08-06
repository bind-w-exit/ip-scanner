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

        public DeviceContentCreatorFactory(DevicesJsonContentCreator jsonContentCreator, DevicesXmlContentCreator xmlContentCreator)
        {
            _jsonContentCreator = jsonContentCreator;
            _xmlContentCreator = xmlContentCreator;
        }

        public IContentCreator<ScannedDevice> Create(ContentFormat format)
        {
            switch (format)
            {
                case ContentFormat.Json:
                    return _jsonContentCreator;
                case ContentFormat.Xml:
                    return _xmlContentCreator;
                default:
                    throw new NotImplementedException($"Content creator for format {format} is not implemented.");
            }
        }
    }
}
