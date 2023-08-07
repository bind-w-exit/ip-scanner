using System.Collections.Generic;

namespace IpScanner.Infrastructure.ContentFormatters
{
    public interface IContentFormatter<T>
    {
        T FormatContent(string content);
        IEnumerable<T> FormatContentAsCollection(string content);
    }
}
