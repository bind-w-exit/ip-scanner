using System;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.Services
{
    public interface IUriOpenerService
    {
        Task OpenUriAsync(Uri uri);
    }
}
