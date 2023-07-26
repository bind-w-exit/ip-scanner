using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;
using IpScanner.Domain.UnitTests.Mocks;
using System.Net;

namespace IpScanner.Domain.UnitTests
{
    [TestClass]
    public class IpScannerIntegrationTests
    {
        private CancellationTokenSource cts;

        [TestInitialize]
        public void TestInitialize()
        {
            cts = new CancellationTokenSource();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            cts.Dispose();
        }

        [TestMethod]
        public async Task Start_ShouldScanNetwork()
        {
            // Arrange
            var lazyResultProvider = new LazyResultProviderMock();

            var from = IPAddress.Parse("192.168.0.104");
            var to = IPAddress.Parse($"192.168.0.106");

            var ipScanner = Models.IpScanner.Create(from, to, lazyResultProvider);

            // Act
            await ipScanner.StartAsync(cts.Token);

            // Assert
            int notExpectedScannedDevicesCount = 0;
            Assert.AreNotSame(notExpectedScannedDevicesCount, lazyResultProvider.ScannedDevices.Count);
        }
    }
}
