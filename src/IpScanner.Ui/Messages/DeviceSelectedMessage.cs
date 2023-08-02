using IpScanner.Domain.Models;

namespace IpScanner.Ui.Messages
{
    public class DeviceSelectedMessage
    {
        public DeviceSelectedMessage(ScannedDevice selectedDevice)
        {
            Device = selectedDevice;
        }

        public ScannedDevice Device { get; }
    }
}
