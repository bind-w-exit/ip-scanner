using IpScanner.Domain.Enums;
using System;

namespace IpScanner.Infrastructure.Extensions
{
    public static class ContentFormatExtensions
    {
        public static ContentFormat GetContentFormatFromString(this string fileExtension)
        {
            switch (fileExtension)
            {
                case ".json":
                    return ContentFormat.Json;
                case ".xml":
                    return ContentFormat.Xml;
                case ".csv":
                    return ContentFormat.Csv;
                case ".html":
                    return ContentFormat.Html;
                default:
                    throw new Exception("Unsupported file type");
            }
        }
    }
}
