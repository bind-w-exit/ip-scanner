using System;
using System.Net;
using System.Threading.Tasks;
using IpScanner.Domain.Interfaces;

namespace IpScanner.Infrastructure.Repositories
{
    public class HostDnsRepository : IHostRepository
    {
        public async Task<IPHostEntry> GetHostAsync(IPAddress address)
        {
			try
			{
                IPHostEntry hostEntry = await Dns.GetHostEntryAsync(address);
                return hostEntry;
            }
			catch (Exception)
			{
                return new IPHostEntry() { HostName = address.ToString() };
			}
        }
    }
}
