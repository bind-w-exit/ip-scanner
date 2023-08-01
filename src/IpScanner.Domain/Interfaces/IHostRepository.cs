using System.Net;
using System.Threading.Tasks;

namespace IpScanner.Domain.Interfaces
{
    public interface IHostRepository
    {
        Task<IPHostEntry> GetHostAsync(IPAddress address);
    }
}
