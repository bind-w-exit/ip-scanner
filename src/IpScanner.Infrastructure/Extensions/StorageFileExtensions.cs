using System;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.Extensions
{
    public static class StorageFileExtensions
    {
        public static async Task<string> ReadTextAsync(this Windows.Storage.StorageFile file)
        {
            return await Windows.Storage.FileIO.ReadTextAsync(file);
        }
    }
}
