using System.ComponentModel;
using Domain.Entity;
using Domain.Shared.Converter;

namespace Domain.UserAggregate.UserEntity;

[OpenJsonConverter<UserId>]
[TypeConverter(typeof(OpenTypeConverter<UserId>))]
public readonly record struct UserId(long Value) : ITypedId<UserId, long>
{
    public static UserId GenerateNew() => new(Snowflake.NewId);

    public static implicit operator long(UserId id) => id.Value;

    public static implicit operator UserId(long value) => new(value);
}
