using System.Net;
using System.Linq;
using System.Collections.Generic;
using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Validators;
using System;
using IpScanner.Domain.Models;

namespace IpScanner.Domain.Factories
{
    public class IpScannerFactory : IIpScannerFactory
    {
        private readonly IMacAddressRepository _macAddressScanner;
        private readonly IManufactorRepository _manufactorReceiver;
        private readonly IValidator<IpRange> _ipRangeValidator;

        public IpScannerFactory(IMacAddressRepository macAddressScanner, 
            IManufactorRepository manufactorReceiver, IValidator<IpRange> ipRangeValidator)
        {
            _macAddressScanner = macAddressScanner;
            _manufactorReceiver = manufactorReceiver;
            _ipRangeValidator = ipRangeValidator;
        }

        public Models.NetworkScanner CreateBasedOnIpRange(IpRange range)
        {
            if (_ipRangeValidator.Validate(range) == false)
            {
                throw new IpValidationException();
            }

            var ips = GenerateIPAddresses(range.Range);
            return new Models.NetworkScanner(ips, _manufactorReceiver, _macAddressScanner);
        }

        public List<IPAddress> GenerateIPAddresses(string ipRange)
        {
            return ipRange.Split(',')
                .SelectMany(range => GenerateIPAddressesForRange(range.Trim()))
                .ToList();
        }

        private IEnumerable<IPAddress> GenerateIPAddressesForRange(string ipRange)
        {
            string[] parts = ipRange.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string networdId = string.Join('.', parts.Take(3));

            string[] lastPart = parts[3].Split('-');

            int start = int.Parse(lastPart[0]);
            int end = lastPart.Length > 1 ? int.Parse(lastPart[1]) : start;

            var ipAddressesForRange = Enumerable.Range(start, end - start + 1)
                .Select(i => IPAddress.Parse(networdId + '.' + i));

            return ipAddressesForRange;
        }
    }
}
