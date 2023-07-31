using IpScanner.Domain.Models;
using System;

namespace IpScanner.Domain.Args
{
    public class ScannedDeviceEventArgs : EventArgs
    {
        public ScannedDeviceEventArgs(ScannedDevice device)
        {
            ScannedDevice = device;
        }

        public ScannedDevice ScannedDevice { get; }
    }
}
