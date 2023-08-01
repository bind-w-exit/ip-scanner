using System;
using System.Net;
using System.Threading.Tasks;
using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Interfaces;

namespace IpScanner.Infrastructure
{
    public class DnsHostRepository : IHostRepository
    {
        public async Task<IPHostEntry> GetHostAsync(IPAddress address)
        {
			try
			{
                IPHostEntry hostEntry = await Dns.GetHostEntryAsync(address);
                return hostEntry;
            }
			catch (Exception e)
			{
				throw new HostNotFoundException(address, e.Message, e);
			}
        }
    }
}
