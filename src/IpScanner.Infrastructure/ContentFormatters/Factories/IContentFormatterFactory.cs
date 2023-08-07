using IpScanner.Domain.Enums;

namespace IpScanner.Infrastructure.ContentFormatters.Factories
{
    public interface IContentFormatterFactory<T>
    {
        IContentFormatter<T> Create(ContentFormat format);
    }
}
