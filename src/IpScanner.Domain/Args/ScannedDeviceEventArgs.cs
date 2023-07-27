using IpScanner.Domain.Models;
using System;

namespace IpScanner.Domain.Args
{
    public class ScannedDeviceEventArgs : EventArgs
    {
        public ScannedDeviceEventArgs(ScannedDevice device, int currentProgress)
        {
            ScannedDevice = device;
            CurrentProgress = currentProgress;
        }

        public ScannedDevice ScannedDevice { get; }
        public int CurrentProgress { get; }
    }
}
