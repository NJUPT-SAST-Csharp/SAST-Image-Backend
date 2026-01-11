using System.Buffers.Binary;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using Domain.Entity;
using Domain.Shared.Converter;

namespace Domain.UserAggregate.UserEntity;

[OpenJsonConverter<RefreshToken, string>]
public readonly record struct RefreshToken
    : IValueObject<RefreshToken, string>,
        IFactoryConstructor<RefreshToken, string>
{
    const int ByteLength = 32;

    public string Value { get; }

    internal UserId Id
    {
        get
        {
            Span<byte> buffer = stackalloc byte[ByteLength];
            Base64Url.TryDecodeFromChars(Value, buffer, out _);

            long id = BinaryPrimitives.ReadInt64LittleEndian(buffer[0..8]);

            return new(id);
        }
    }

    internal bool IsExpired
    {
        get
        {
            Span<byte> buffer = stackalloc byte[ByteLength];

            Base64Url.TryDecodeFromChars(Value, buffer, out _);

            long timeB = BinaryPrimitives.ReadInt64LittleEndian(buffer[8..16]);
            return DateTime.FromBinary(timeB) < DateTime.UtcNow;
        }
    }

    internal RefreshToken(string value)
    {
        Value = value;
    }

    public static bool TryCreateNew(string input, [NotNullWhen(true)] out RefreshToken newObject)
    {
        Span<byte> buffer = stackalloc byte[ByteLength];

        if (Base64Url.IsValid(input) == false)
        {
            newObject = default;
            return false;
        }

        newObject = new(input);
        return true;
    }

    public override string ToString() => Value;
}
