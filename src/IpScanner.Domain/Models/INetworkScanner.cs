using IpScanner.Domain.Args;
using IpScanner.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace IpScanner.Domain.Models
{
    public interface INetworkScanner
    {

        event EventHandler ScanningFinished;

        event EventHandler<ScannedDeviceEventArgs> DeviceScanned;

        void Pause();

        void Resume();

        Task StartAsync(IEnumerable<IPAddress> addresses, CancellationToken cancellationToken);
    }
}
