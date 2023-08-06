using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Mappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace IpScanner.Infrastructure.ContentCreators
{
    public class DevicesXmlContentCreator : IContentCreator<ScannedDevice>
    {
        public string CreateContent(IEnumerable<ScannedDevice> items)
        {
            List<DeviceEntity> entities = items.Select(x => x.ToEntity()).ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<DeviceEntity>));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, entities);
                return writer.ToString();
            }
        }
    }
}
