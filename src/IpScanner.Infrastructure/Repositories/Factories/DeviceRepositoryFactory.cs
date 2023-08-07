using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.ContentFormatters.Factories;
using IpScanner.Infrastructure.Entities;
using Windows.Storage;

namespace IpScanner.Infrastructure.Repositories.Factories
{
    public class DeviceRepositoryFactory : IDeviceRepositoryFactory 
    {
        private readonly IContentCreatorFactory<ScannedDevice> _contentCreatorFactory;
        private readonly IContentFormatterFactory<DeviceEntity> _contentFormatterFactory;

        public DeviceRepositoryFactory(IContentCreatorFactory<ScannedDevice> contentCreatorFactory,
            IContentFormatterFactory<DeviceEntity> contentFormatterFactory)
        {
            _contentCreatorFactory = contentCreatorFactory;
            _contentFormatterFactory = contentFormatterFactory;
        }
        public IDeviceRepository CreateWithFile(StorageFile file)
        {
            return new DeviceRepository(file, _contentCreatorFactory, _contentFormatterFactory);
        }
    }
}
