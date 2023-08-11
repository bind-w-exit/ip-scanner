using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Storage.Streams;
using IpScanner.Infrastructure.Extensions;

namespace IpScanner.Infrastructure.Services
{
    public class WakeOnLanService : IWakeOnLanService, IDisposable
    {
        private readonly DatagramSocket _datagramSocker;

        public WakeOnLanService()
        {
            _datagramSocker = new DatagramSocket();
        }

        public async Task SendPacketAsync(PhysicalAddress macAddress, IPAddress ipAddress, int port = 9)
        {
            byte[] macBytes = macAddress.ConvertToBytes();
            byte[] magicPacket = CreateMagicPacket(macBytes);

            IOutputStream stream = await _datagramSocker.GetOutputStreamAsync(new HostName(ipAddress.ToString()), port.ToString());
            using (var writer = new DataWriter(stream))
            {
                writer.WriteBytes(magicPacket);
                await writer.StoreAsync();
            }
        }

        public void Dispose()
        {
            _datagramSocker.Dispose();
        }

        /// <summary>
        /// Create a magic packet fom the NIC (Network Interface Card) to wake up or shut down the device.
        /// </summary>
        /// <param name="macBytes">Mac Address in byte format</param>
        /// <returns></returns>
        private byte[] CreateMagicPacket(byte[] macBytes)
        {
            byte[] magicPacket = new byte[102];
            for (int i = 0; i < 6; i++)
            {
                magicPacket[i] = 0xFF;
            }

            for (int i = 1; i <= 16; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    magicPacket[i * 6 + j] = macBytes[j];
                }
            }

            return magicPacket;
        }
    }
}
