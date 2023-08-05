using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Extensions;
using IpScanner.Infrastructure.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace IpScanner.Infrastructure.ContentCreators
{
    public class DevicesJsonContentCreator : IContentCreator<ScannedDevice>
    {
        public string CreateContent(IEnumerable<ScannedDevice> items)
        {
            if(items == null)
            {
                throw new System.ArgumentNullException(nameof(items));
            }

            List<DeviceEntity> entities = items.Select(x => x.ToEntity()).ToList();
            return entities.ToJson();
        }
    }
}
