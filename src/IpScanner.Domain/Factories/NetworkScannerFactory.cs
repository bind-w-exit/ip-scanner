using System.Net;
using IpScanner.Domain.Models;
using System.Collections.Generic;
using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Validators;

namespace IpScanner.Domain.Factories
{
    public class NetworkScannerFactory : INetworkScannerFactory
    {
        private readonly IHostRepository _hostRepository;
        private readonly IMacAddressRepository _macAddressRepository;
        private readonly IManufactorRepository _manufactorRepository;
        private readonly IValidator<IpRange> _ipRangeValidator;

        public NetworkScannerFactory(IMacAddressRepository macAddressScanner, IManufactorRepository manufactorReceiver,
            IHostRepository hostRepository, IValidator<IpRange> ipRangeValidator)
        {
            _hostRepository = hostRepository;
            _macAddressRepository = macAddressScanner;
            _manufactorRepository = manufactorReceiver;
            _ipRangeValidator = ipRangeValidator;
        }

        public NetworkScanner CreateBasedOnIpRange(IpRange range)
        {
            if (_ipRangeValidator.Validate(range) == false)
            {
                throw new IpValidationException();
            }

            List<IPAddress> ips = range.GenerateIPAddresses();
            return new NetworkScanner(ips, _manufactorRepository, _macAddressRepository, _hostRepository);
        }
    }
}
