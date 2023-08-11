using System;
using System.Net.NetworkInformation;
using System.Text;

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

        public static string ToFormattedString(this PhysicalAddress macAddress)
        {
            string macString = macAddress.ToString();
            var formattedMacString = new StringBuilder();

            for (int i = 0; i < macString.Length; i++)
            {
                formattedMacString.Append(macString[i]);

                if (i % 2 == 1 && i != macString.Length - 1)
                    formattedMacString.Append(":");
            }

            return formattedMacString.ToString();
        }

        public static byte[] ConvertToBytes(this PhysicalAddress macAddress)
        {
            byte[] macBytes = new byte[6];
            string[] macParts = macAddress.ToFormattedString().Split(':');

            for (int i = 0; i < 6; i++)
            {
                macBytes[i] = Convert.ToByte(macParts[i], 16);
            }

            return macBytes;
        }
    }
}
