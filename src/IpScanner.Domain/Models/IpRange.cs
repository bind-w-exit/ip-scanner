namespace IpScanner.Domain.Models
{
    public class IpRange
    {
        public IpRange(string range)
        {
            Range = range;
        }

        public string Range { get; }
    }
}
