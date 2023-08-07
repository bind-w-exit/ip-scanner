using IpScanner.Domain.Models;
using System.Collections.Generic;

namespace IpScanner.Ui.Messages
{
    public class DevicesLoadedMessage
    {
        public DevicesLoadedMessage(IEnumerable<ScannedDevice> devices)
        {
            Devices = devices;
        }

        public IEnumerable<ScannedDevice> Devices { get; }
    }
}
