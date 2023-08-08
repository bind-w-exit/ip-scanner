using System;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.Services
{
    public interface IBrowserService
    {
        Task OpenBrowserAsync(Uri uri);
    }
}
