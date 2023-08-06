using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpScanner.Infrastructure.ContentCreators
{
    public class DevicesHtmlContentCreator : IContentCreator<ScannedDevice>
    {
        public string CreateContent(IEnumerable<ScannedDevice> items)
        {
            List<DeviceEntity> entities = items.Select(x => x.ToEntity()).ToList();
            return ConstructHtmlDocument(ConstructTable(entities));
        }

        private string ConstructHtmlDocument(string tableContent)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <title>Devices</title>
    <!-- Add Bootstrap CSS -->
    <link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css'>
</head>
<body>
<div class='container mt-5'> <!-- Bootstrap container for centering and margin -->
{tableContent}
</div>
</body>
</html>";
        }

        private string ConstructTable(List<DeviceEntity> entities)
        {
            StringBuilder tableBuilder = new StringBuilder();

            // Add Bootstrap table classes
            tableBuilder.AppendLine("<table class='table table-hover table-striped'>");
            tableBuilder.AppendLine(ConstructTableHeaders());
            entities.ForEach(entity => tableBuilder.AppendLine(ToHtmlRow(entity)));
            tableBuilder.AppendLine("</table>");

            return tableBuilder.ToString();
        }

        private string ConstructTableHeaders()
        {
            return @"
<tr>
    <th>Status</th>
    <th>Name</th>
    <th>Ip</th>
    <th>Manufacturer</th>
    <th>MacAddress</th>
    <th>Comments</th>
    <th>Favorite</th>
</tr>";
        }

        private string ToHtmlRow(DeviceEntity entity)
        {
            return $@"
<tr>
    <td>{EscapeHtmlValue(entity.Status.ToString())}</td>
    <td>{EscapeHtmlValue(entity.Name)}</td>
    <td>{EscapeHtmlValue(entity.Ip)}</td>
    <td>{EscapeHtmlValue(entity.Manufacturer)}</td>
    <td>{EscapeHtmlValue(entity.MacAddress)}</td>
    <td>{EscapeHtmlValue(entity.Comments)}</td>
    <td>{entity.Favorite}</td>
</tr>";
        }

        private string EscapeHtmlValue(string value)
        {
            return System.Net.WebUtility.HtmlEncode(value);
        }
    }
}
