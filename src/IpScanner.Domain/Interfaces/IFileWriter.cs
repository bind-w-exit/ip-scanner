using System.Threading.Tasks;

namespace IpScanner.Domain.Interfaces
{
    public interface IFileWriter
    {
        Task WriteAsync(string path, string content);
    }
}
