using Windows.Storage;

namespace IpScanner.Infrastructure.Repositories.Factories
{
    public interface IDeviceRepositoryFactory
    {
        IDeviceRepository CreateWithFile(StorageFile file);
    }
}
