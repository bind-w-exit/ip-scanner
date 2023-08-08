using System;
using System.Threading.Tasks;
using Windows.System;

namespace IpScanner.Infrastructure.Services
{
    public class BrowserService : IBrowserService
    {
        public async Task OpenBrowserAsync(Uri uri)
        {
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
