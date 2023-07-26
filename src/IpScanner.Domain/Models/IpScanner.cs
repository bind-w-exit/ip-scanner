using IpScanner.Domain.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


namespace IpScanner.Domain.Models
{
    public class IpScanner
    {
        private readonly int _startHostId;
        private readonly int _endHostId;
        private readonly ILazyResultProvider _resultProvider;

        public IpScanner(int startHost, int endHost, ILazyResultProvider progressProvider)
        {
            _startHostId = startHost;
            _endHostId = endHost;

            _resultProvider = progressProvider;
        }

        public static IpScanner Create(IPAddress start, IPAddress end, ILazyResultProvider progressProvider)
        {
            var startHostId = int.Parse(start.ToString().Split('.').Last());
            var endHostId = int.Parse(end.ToString().Split('.').Last());

            return new IpScanner(startHostId, endHostId, progressProvider);
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            await Task.Delay(1);

            using (var webClient = new WebClient())
            {
                for (int i = _startHostId; i <= _endHostId; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    var destination = IPAddress.Parse($"192.168.0.{i}");
                    IPAddress source = null;

                    byte[] macAddr = new byte[6];
                    uint macAddrLen = (uint)macAddr.Length;

                    if (SendARP(BitConverter.ToInt32(destination.GetAddressBytes(), 0),
                        source == null ? 0 : BitConverter.ToInt32(source.GetAddressBytes(), 0),
                        macAddr, ref macAddrLen) != 0)
                    {
                        var scannedDevice = new ScannedDevice("Offline", destination.ToString(),
                            destination, string.Empty, string.Empty, string.Empty);

                        _resultProvider.Report(scannedDevice, i);
                    }
                    else
                    {
                        var address = new PhysicalAddress(macAddr);

                        var mac = address.ToString();

                        string oui = mac.Substring(0, 6);

                        string url = $"https://api.macvendors.com/{oui}";
                        string manufacturer = string.Empty;

                        try
                        {
                            manufacturer = webClient.DownloadString(url);
                        }
                        catch (WebException)
                        {
                            manufacturer = string.Empty;
                        }

                        var scannedDevice = new ScannedDevice("Online", destination.ToString(),
                            destination, manufacturer, mac, string.Empty);

                        _resultProvider.Report(scannedDevice, i);
                    }
                }
            }
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);
    }
}
