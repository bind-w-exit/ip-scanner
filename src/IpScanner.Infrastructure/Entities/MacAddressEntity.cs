using CsvHelper.Configuration.Attributes;

namespace IpScanner.Infrastructure.Entities
{
    internal class MacAddressEntity
    {
        [Name("Registry")]
        public string Registry { get; set; }
        [Name("Assignment")]
        public string Assignment { get; set; }
        [Name("Organization Name")]
        public string OrganizationName { get; set; }
        [Name("Organization Address")]
        public string OrganizationAddress { get; set; }
    }
}
