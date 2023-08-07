using IpScanner.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace IpScanner.Infrastructure.Repositories
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<ScannedDevice>> GetDevicesAsync();
        Task SaveDevicesAsync(IEnumerable<ScannedDevice> devices);
        Task AddDeviceAsync(ScannedDevice device);
        Task RemoveDeviceAsync(ScannedDevice device);
        Task UpdateDeviceAsync(ScannedDevice device);
    }
}
