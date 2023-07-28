using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace IpScanner.Domain.Validators
{
    public class IpRangeValidator : IValidator<string>
    {
        public bool Validate(string ipRange)
        {
            string pattern = @"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.(\d{1,3}-\d{1,3}|\d{1,3})(, \d{1,3}\.\d{1,3}\.\d{1,3}\.(\d{1,3}-\d{1,3}|\d{1,3}))*)$";

            return Regex.IsMatch(ipRange, pattern) && ipRange.Split(',')
                    .Select(ip => ip.Trim().Split('.'))
                    .All(parts => parts.All(ValidateIPPart));
        }

        private bool ValidateIPPart(string part)
        {
            return part.Split('-').All(p => int.TryParse(p, out int number) && number >= 0 && number <= 255);
        }
    }
}
