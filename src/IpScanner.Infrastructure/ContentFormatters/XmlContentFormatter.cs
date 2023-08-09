using System.IO;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using FluentResults;

namespace IpScanner.Infrastructure.ContentFormatters
{
    public class XmlContentFormatter<T> : IContentFormatter<T>
    {
        public IResult<T> FormatContent(string content)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(content))
                {
                    return Result.Ok((T)serializer.Deserialize(reader));
                }
            }
            catch (InvalidOperationException e)
            {
                return Result.Fail<T>(new Error("The content is not in the correct format.", new Error(e.Message)));
            }
        }

        public IResult<IEnumerable<T>> FormatContentAsCollection(string content)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using (StringReader reader = new StringReader(content))
                {
                    return Result.Ok((List<T>)serializer.Deserialize(reader));
                }
            }
            catch (InvalidOperationException e)
            {
                return Result.Fail<IEnumerable<T>>(new Error("The content is not in the correct format.", new Error(e.Message)));
            }
        }
    }
}
