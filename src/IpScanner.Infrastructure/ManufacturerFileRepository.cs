using CsvHelper;
using IpScanner.Domain.Interfaces;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure
{
    public class ManufacturerFileRepository : IManufactorReceiver
    {
        private readonly string _path = "Assets/oui.csv";
        private readonly Dictionary<string, string> _assignmentsAndManufacturers;

        public ManufacturerFileRepository()
        {
            using(var reader = new StreamReader(_path))
            {
                using(var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    _assignmentsAndManufacturers = csv.GetRecords<MacAddressEntity>()
                       .GroupBy(x => x.Assignment)
                       .Select(grp => grp.First())
                       .ToDictionary(mac => mac.Assignment, manufacturer => manufacturer.OrganizationName);
                }
            }
        }

        public async Task<string> GetManufacturerOrEmptyStringAsync(PhysicalAddress macAddress)
        {
            return await Task.Run(() => GetManufacturerOrEmptyString(macAddress));
        }

        private string GetManufacturerOrEmptyString(PhysicalAddress macAddress)
        {
            string assignment = macAddress.GetAssignment();
            if (_assignmentsAndManufacturers.TryGetValue(assignment, out string manufacturer))
            {
                return manufacturer;
            }

            return string.Empty;
        }
    }
}
