using System;
using IpScanner.Domain.Enums;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.ContentCreators;

namespace IpScanner.Infrastructure.Factories
{
    public class DeviceContentCreatorFactory : IContentCreatorFactory<ScannedDevice>
    {
        public IContentCreator<ScannedDevice> Create(ContentFormat format)
        {
            switch (format)
            {
                case ContentFormat.Json:
                    return new DevicesJsonContentCreator();
                default:
                    throw new NotImplementedException($"Content creator for format {format} is not implemented.");
            }
        }
    }
}
