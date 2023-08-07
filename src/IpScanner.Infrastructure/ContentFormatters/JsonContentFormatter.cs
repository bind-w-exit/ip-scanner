using IpScanner.Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Text.Json;

namespace IpScanner.Infrastructure.ContentFormatters
{
    public class JsonContentFormatter<T> : IContentFormatter<T>
    {
        public T FormatContent(string content)
        {
			try
			{
                return JsonSerializer.Deserialize<T>(content);
            }
			catch (JsonException e)
			{
                throw new ContentFormatException("The content is not in the correct format.", e);
			}
        }

        public IEnumerable<T> FormatContentAsCollection(string content)
        {
            try
            {
                return JsonSerializer.Deserialize<IEnumerable<T>>(content);
            }
            catch (JsonException e)
            {
                throw new ContentFormatException("The content is not in the correct format.", e);
            }
        }
    }
}
