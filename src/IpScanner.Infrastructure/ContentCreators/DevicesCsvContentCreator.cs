using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpScanner.Infrastructure.ContentCreators
{
    public class DevicesCsvContentCreator : IContentCreator<ScannedDevice>
    {
        public string CreateContent(IEnumerable<ScannedDevice> items)
        {
            List<DeviceEntity> entities = items.Select(x => x.ToEntity()).ToList();

            StringBuilder csvBuilder = new StringBuilder();

            csvBuilder.AppendLine("Status,Name,Ip,Manufacturer,MacAddress,Comments,Favorite");
            foreach (var entity in entities)
            {
                csvBuilder.AppendLine(ToCsvRow(entity));
            }

            return csvBuilder.ToString();
        }

        private string ToCsvRow(DeviceEntity entity)
        {
            return $"{entity.Status},{EscapeCsvValue(entity.Name)},{EscapeCsvValue(entity.Ip)},{EscapeCsvValue(entity.Manufacturer)},{EscapeCsvValue(entity.MacAddress)},{EscapeCsvValue(entity.Comments)},{entity.Favorite}";
        }

        private string EscapeCsvValue(string value)
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
    }
}
