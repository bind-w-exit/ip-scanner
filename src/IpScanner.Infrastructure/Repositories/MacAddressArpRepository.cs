using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using IpScanner.Domain.Interfaces;

namespace IpScanner.Infrastructure.Repositories
{
    public class MacAddressArpRepository : IMacAddressRepository
    {
        public async Task<PhysicalAddress> GetMacAddressAsync(IPAddress destination, CancellationToken cancellationToken)
        {
            using (CancellationTokenSource cts = CreateLinkedCancellationTokenWithTimeout(cancellationToken, TimeSpan.FromSeconds(5)))
            {
                return await Task.Run(() => RetrieveMacAddress(destination), cts.Token);
            }
        }

        private CancellationTokenSource CreateLinkedCancellationTokenWithTimeout(CancellationToken cancellationToken, TimeSpan timeout)
        {
            CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(timeout);
            return cts;
        }

        private PhysicalAddress RetrieveMacAddress(IPAddress destination)
        {
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            int destIP = BitConverter.ToInt32(destination.GetAddressBytes(), 0);

            bool deviceFound = SendARP(destIP, 0, macAddr, ref macAddrLen) == 0;
            return deviceFound ? new PhysicalAddress(macAddr) : PhysicalAddress.None;
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

    }
}
