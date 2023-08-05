using IpScanner.Domain.Enums;

namespace IpScanner.Domain.Interfaces
{
    public interface IContentCreatorFactory<T>
    {
        IContentCreator<T> Create(ContentFormat format);
    }
}
