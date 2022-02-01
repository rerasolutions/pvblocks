using System.IO;
using System.Text;
using System.Text.Json;

namespace ReRa.PVBlocks.Blazor.Client.Helpers
{
    public static class JsonUtil
    {
        public static string ToJsonString(this JsonDocument doc)
        {
            using var stream = new MemoryStream();
            Utf8JsonWriter writer = new(stream, new JsonWriterOptions { Indented = true });
            doc.WriteTo(writer);
            writer.Flush();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public static JsonDocument ToJsonDocument(this JsonElement prop)
            => JsonDocument.Parse(JsonSerializer.Serialize(new
            {
                Value = prop
            }));
        
        public static JsonElement ToJsonElement(object obj)
            => ToJsonDocument(new
            {
                val = obj
            }).RootElement.GetProperty("val");
        
        public static JsonDocument EmptyDocument()
            => JsonDocument.Parse("{}");

        public static JsonDocument ToJsonDocument(object? obj)
            => obj == null ? EmptyDocument() : JsonDocument.Parse(JsonSerializer.Serialize(obj));

        public static T? ToObject<T>(this JsonDocument doc)
            => JsonSerializer.Deserialize<T>(doc.ToJsonString());
    }
}