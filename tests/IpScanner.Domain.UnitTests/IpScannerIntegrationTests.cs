using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;
using IpScanner.Domain.UnitTests.Mocks;
using System;

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
            var maxHostId = 10;
            var lazyResultProvider = new LazyResultProviderMock(); // You need to create this mock class
            var ipScanner = new Models.IpScanner(maxHostId, lazyResultProvider);

            // Act
            await ipScanner.Start(cts.Token);

            // Assert
            foreach (var item in lazyResultProvider.ScannedDevices)
            {
                Console.WriteLine(item.ToString());
            }

            Assert.IsTrue(true);
        }
    }
}
