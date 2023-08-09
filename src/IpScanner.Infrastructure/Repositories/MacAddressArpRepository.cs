using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Interfaces;

namespace IpScanner.Infrastructure.Repositories
{
    public class MacAddressArpRepository : IMacAddressRepository
    {
        public async Task<PhysicalAddress> GetMacAddressAsync(IPAddress destination)
        {
            return await Task.Run(() =>
            {
                byte[] macAddr = new byte[6];
                uint macAddrLen = (uint)macAddr.Length;

                bool deviceFound = SendARP(BitConverter.ToInt32(destination.GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) == 0;
                return deviceFound ? new PhysicalAddress(macAddr) : PhysicalAddress.None;
            });
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);
    }
}
