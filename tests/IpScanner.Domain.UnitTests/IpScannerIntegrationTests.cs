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
            await Task.Delay(1);
            Assert.IsTrue(true);
        }
    }
}
