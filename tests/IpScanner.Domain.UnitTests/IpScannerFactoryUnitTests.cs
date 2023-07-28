using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Factories;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace IpScanner.Domain.UnitTests
{
    [TestClass]
    public class IpScannerFactoryUnitTests
    {
        private readonly IMacAddressScanner _macAddressScanner;
        private readonly IManufactorReceiver _manufactorReceiver;
        private readonly IValidator<string> _ipRangeValidator;
        private readonly IpScannerFactory _sut;

        public IpScannerFactoryUnitTests()
        {
            _macAddressScanner = Substitute.For<IMacAddressScanner>();
            _manufactorReceiver = Substitute.For<IManufactorReceiver>();
            _ipRangeValidator = Substitute.For<IValidator<string>>();


            _sut = new IpScannerFactory(_macAddressScanner, _manufactorReceiver, _ipRangeValidator);
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
        public void CreateBasedOnIpRange_ShouldThrowException_WhenIpRangeIsInvalid(string ipRange)
        {
            // Arrange
            _ipRangeValidator.Validate(ipRange).Returns(false);

            // Act
            _sut.CreateBasedOnIpRange(ipRange);
        }

        [TestMethod]
        public void CreateBasedOnIpRange_ShouldReturnIpScanner_WhenIpRangeIsValid()
        {
            // Arrange
            string ipRange = "192.168.0.1-2";
            _ipRangeValidator.Validate(ipRange).Returns(true);

            // Act
            Models.IpScanner result = _sut.CreateBasedOnIpRange(ipRange);

            // Assert
            var expectedIpAddresses = new List<IPAddress> { IPAddress.Parse("192.168.0.1"), IPAddress.Parse("192.168.0.2")};
            
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ScannedIps.Count());
            CollectionAssert.AreEqual(expectedIpAddresses, result.ScannedIps.ToList());
        }

        [TestMethod]
        public void GenerateIPAddresses_ShouldReturnListOfIPAddresses_WhenValidIpRange()
        {
            // Arrange
            string ipRange = "192.168.0.1-2";

            // Act
            var result = _sut.GenerateIPAddresses(ipRange);

            // Assert
            var expected = new List<IPAddress> { IPAddress.Parse("192.168.0.1"), IPAddress.Parse("192.168.0.2") };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
