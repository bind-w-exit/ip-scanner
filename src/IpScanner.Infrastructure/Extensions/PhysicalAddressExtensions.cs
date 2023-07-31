using System;
using System.Net.NetworkInformation;

namespace IpScanner.Infrastructure.Extensions
{
    public static class PhysicalAddressExtensions
    {
        public static Uri GetUrlToFindManufacturer(this PhysicalAddress macAddress)
        {
            string oui = macAddress.GetAssignment();
            return new Uri($"https://api.macvendors.com/{oui}");
        }

        public static string GetAssignment(this PhysicalAddress macAddress)
        {
            string oui = macAddress.ToString().Substring(0, 6);
            return oui;
        }
    }
}
