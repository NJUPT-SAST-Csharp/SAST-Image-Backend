using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoxResult.Extensions
{
    internal sealed class JsonLongConverter : JsonConverter<long>
    {
        public override long Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            return reader.GetInt64();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            if (value > 9007199254740993)
                writer.WriteStringValue(value.ToString());
            else
                writer.WriteNumberValue(value);
        }
    }
}
