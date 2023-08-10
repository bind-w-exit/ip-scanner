using IpScanner.Infrastructure.Settings;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.Services
{
    public interface IFtpService
    {
        Task<bool> ConnectAsync(FtpConfiguration configuration);
    }
}
