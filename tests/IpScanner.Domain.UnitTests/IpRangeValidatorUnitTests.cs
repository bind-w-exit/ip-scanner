using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IpScanner.Domain.UnitTests
{
    [TestClass]
    public class IpRangeValidatorUnitTests
    {
        [TestMethod]
        [DataRow("192.168.0.1-255")]
        [DataRow("192.168.0.1-155, 192.168.0.201")]
        [DataRow("192.168.0.105")]
        public void ValidateIPRange_ShouldReturnTrue_WhenValidIpRange(string ipRange)
        {
            // Arrange
            var validator = new Validators.IpRangeValidator();
            
            // Act
            bool result = validator.Validate(ipRange);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow("192.168.0.256-400")] // Invalid because 256 and 400 are out of range (0-255)
        [DataRow("192.168.0.155-155, 192.168.0.355")] // Invalid because 355 is out of range
        [DataRow("192.168.0.")] // Invalid because it doesn't specify a host ID
        [DataRow("300.168.0.105")] // Invalid because 300 is out of range for an IP octet
        [DataRow("192.168.0.1-")] // Invalid because it doesn't specify the end of the range
        [DataRow("192.168.0.155 192.168.0.201")] // Invalid because it's missing a comma between the IP addresses
        public void ValidateIPRange_ShouldReturnFalse_WhenInvalidIpRange(string ipRange)
        {
            // Arrange
            var validator = new Validators.IpRangeValidator();

            // Act
            bool result = validator.Validate(ipRange);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
