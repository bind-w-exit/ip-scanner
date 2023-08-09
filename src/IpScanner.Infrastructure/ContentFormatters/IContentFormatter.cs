using FluentResults;
using System.Collections.Generic;

namespace IpScanner.Infrastructure.ContentFormatters
{
    public interface IContentFormatter<T>
    {
        IResult<T> FormatContent(string content);
        IResult<IEnumerable<T>> FormatContentAsCollection(string content);
    }
}
