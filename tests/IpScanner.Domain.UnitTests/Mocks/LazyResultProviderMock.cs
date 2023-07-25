using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using System.Collections.Generic;

namespace IpScanner.Domain.UnitTests.Mocks
{
    public class LazyResultProviderMock : ILazyResultProvider
    {
        public LazyResultProviderMock()
        {
            ScannedDevices = new List<ScannedDevice>();
        }

        public ICollection<ScannedDevice> ScannedDevices { get; private set; }
        public int Progress { get; private set; }

        public void Report(ScannedDevice device, int progress)
        {
            ScannedDevices.Add(device);
            Progress = progress;
        }
    }
}
