using System.IO;
using System.Xml.Serialization;
using System;
using IpScanner.Infrastructure.Exceptions;
using System.Collections.Generic;

namespace IpScanner.Infrastructure.ContentFormatters
{
    public class XmlContentFormatter<T> : IContentFormatter<T>
    {
        public T FormatContent(string content)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(content))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (InvalidOperationException e)
            {
                throw new ContentFormatException("The content is not in the correct format.", e);
            }
        }

        public IEnumerable<T> FormatContentAsCollection(string content)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using (StringReader reader = new StringReader(content))
                {
                    return (List<T>)serializer.Deserialize(reader);
                }
            }
            catch (InvalidOperationException e)
            {
                throw new ContentFormatException("The content is not in the correct format.", e);
            }
        }
    }
}
