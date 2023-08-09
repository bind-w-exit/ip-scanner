using System;
using System.Threading.Tasks;
using Windows.System;

namespace IpScanner.Infrastructure.Services
{
    public class UriOpenerService : IUriOpenerService
    {
        public async Task OpenUriAsync(Uri uri)
        {
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
