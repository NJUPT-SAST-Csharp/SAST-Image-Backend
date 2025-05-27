using System.Text.Json;
using System.Text.Json.Serialization;
using Primitives.Entity;
using Primitives.Exceptions;

namespace Primitives.ValueObject;

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
public sealed class OpenJsonConverterAttribute<TObject, TValue>()
    : JsonConverterAttribute(typeof(OpenJsonConverter<TObject, TValue>))
    where TObject : IValueObject<TObject, TValue> { }

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
public sealed class OpenJsonConverterAttribute<TId>()
    : JsonConverterAttribute(typeof(OpenJsonConverter<TId>))
    where TId : ITypedId<TId>, new() { }

file sealed class OpenJsonConverter<TObject, TValue> : JsonConverter<TObject>
    where TObject : IValueObject<TObject, TValue>
{
    public override TObject? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var value = JsonSerializer.Deserialize<TValue>(ref reader, options);

        if (TObject.TryCreateNew(value!, out var newObject) == false)
            ValueObjectInvalidException.Throw(newObject);

        return newObject;
    }

    public override void Write(Utf8JsonWriter writer, TObject value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Value, options);
    }

    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(TObject);
}

file sealed class OpenJsonConverter<TId> : JsonConverter<TId>
    where TId : ITypedId<TId>, new()
{
    public override TId? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        long value = JsonSerializer.Deserialize<long>(ref reader, options);
        TId id = new() { Value = value };
        return id;
    }

    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Value, options);
    }

    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(TId);
}
