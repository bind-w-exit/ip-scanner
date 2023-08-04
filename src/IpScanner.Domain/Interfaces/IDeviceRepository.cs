using IpScanner.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IpScanner.Domain.Interfaces
{
    public interface IDeviceRepository
    {
        Task SaveDevicesAsync(IEnumerable<ScannedDevice> devices);
        Task AddDeviceAsync(ScannedDevice device);
        Task<IEnumerable<ScannedDevice>> GetDevicesAsync();
        Task RemoveDeviceAsync(ScannedDevice device);
    }
}
