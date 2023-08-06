using System.Text.Json;

namespace IpScanner.Infrastructure.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T item, bool writeIndented = false)
        {
            return JsonSerializer.Serialize(item, new JsonSerializerOptions
            {
                WriteIndented = writeIndented
            });
        }

        public static T FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
