using System.Text.Json;
using System.Text.Json.Serialization;

namespace Account.WebAPI.Configurations;

internal sealed class JsonNumberConverter : JsonConverter<long>
{
    public override long Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        long num = reader.GetInt64();
        return num;
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        if (value > 9007199254740993)
            writer.WriteStringValue(value.ToString());
        else
            writer.WriteNumberValue(value);
    }
}
