using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace IpScanner.Domain.Models
{
    public class IpRange
    {
        public IpRange(string range)
        {
            Range = range;
        }

        public string Range { get; }

        public List<IPAddress> GenerateIPAddresses(IpRange ipRange)
        {
            return ipRange.Range.Split(',')
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
