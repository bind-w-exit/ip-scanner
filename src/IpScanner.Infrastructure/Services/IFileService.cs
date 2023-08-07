using System.Threading.Tasks;
using Windows.Storage;

namespace IpScanner.Infrastructure.Services
{
    public interface IFileService
    {
        Task<StorageFile> GetDefaultFileAsync();
        Task<StorageFile> GetFileForReadingAsync(params string[] fileTypes);
        Task<StorageFile> GetFileForWritingAsync(params string[] fileTypes);
    }
}
