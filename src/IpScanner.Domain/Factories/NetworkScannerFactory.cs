using System.Net;
using IpScanner.Domain.Models;
using System.Collections.Generic;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Validators;
using FluentResults;

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

        public IResult<NetworkScanner> CreateBasedOnIpRange(IpRange range)
        {
            bool validationResult = _ipRangeValidator.Validate(range);
            if (validationResult == false)
            {
                return Result.Fail<NetworkScanner>("Wrong format for IP range");
            }

            List<IPAddress> ips = range.GenerateIPAddresses();
            var scanner = new NetworkScanner(ips, _manufactorRepository, _macAddressRepository, _hostRepository);

            return Result.Ok(scanner);
        }
    }
}
