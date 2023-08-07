using IpScanner.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.Services
{
    public interface IFileService<T>
    {
        Task<IEnumerable<T>> GetItemsAsync();
        Task SaveItemsAsync(IEnumerable<T> devices);
    }
}
