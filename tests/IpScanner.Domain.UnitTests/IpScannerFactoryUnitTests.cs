using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Factories;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Domain.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace IpScanner.Domain.UnitTests
{
    [TestClass]
    public class IpScannerFactoryUnitTests
    {
        private readonly NetworkScannerFactory _sut;
        private readonly IHostRepository _hostRepository = Substitute.For<IHostRepository>();
        private readonly IMacAddressRepository _macAddressRepository = Substitute.For<IMacAddressRepository>();
        private readonly IManufactorRepository _manufactorRepository = Substitute.For<IManufactorRepository>();
        private readonly IValidator<IpRange> _ipRangeValidator = Substitute.For<IValidator<IpRange>>();

        public IpScannerFactoryUnitTests()
        {
            _sut = new NetworkScannerFactory(_macAddressRepository, _manufactorRepository, _hostRepository, _ipRangeValidator);
        }

        [TestMethod]
        [DataRow("Invalid input")]
        [DataRow("184.jf1.131.d")]
        [DataRow("192.168.0.256-400")]
        [DataRow("192.168.0.155-155, 192.168.0.355")]
        [DataRow("192.168.0.")]
        [DataRow("300.168.0.105")]
        [DataRow("192.168.0.1-")]
        [DataRow("192.168.0.155 192.168.0.201")]
        [ExpectedException(typeof(IpValidationException))]
        public void CreateBasedOnIpRange_ShouldThrowException_WhenIpRangeIsInvalid(string range)
        {
            // Arrange
            var ipRange = new IpRange(range);
            _ipRangeValidator.Validate(ipRange).Returns(false);

            // Act
            _sut.CreateBasedOnIpRange(ipRange);
        }
    }
}
