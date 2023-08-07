using System.Collections.Generic;

namespace IpScanner.Domain.Interfaces
{
    public interface IContentCreator<T>
    {
        string CreateContent(IEnumerable<T> items);
    }
}
