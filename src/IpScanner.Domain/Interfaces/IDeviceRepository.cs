using IpScanner.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IpScanner.Domain.Interfaces
{
    public interface IDeviceRepository
    {
        Task SaveFavoritesDevicesAsync(IEnumerable<ScannedDevice> devices);
        Task<IEnumerable<ScannedDevice>> GetFavoritesDevicesAsync();
    }
}
