using FluentResults;
using System.Collections.Generic;
using System.Text.Json;

namespace IpScanner.Infrastructure.ContentFormatters
{
    public class JsonContentFormatter<T> : IContentFormatter<T>
    {
        IResult<T> IContentFormatter<T>.FormatContent(string content)
        {
            try
            {
                return Result.Ok(JsonSerializer.Deserialize<T>(content));
            }
            catch (JsonException e)
            {
                return Result.Fail<T>(new Error("The content is not in the correct format.", new Error(e.Message)));
            }
        }

        IResult<IEnumerable<T>> IContentFormatter<T>.FormatContentAsCollection(string content)
        {
            try
            {
                return Result.Ok(JsonSerializer.Deserialize<IEnumerable<T>>(content));
            }
            catch (JsonException e)
            {
                return Result.Fail<IEnumerable<T>>(new Error("The content is not in the correct format.", new Error(e.Message)));
            }
        }
    }
}
