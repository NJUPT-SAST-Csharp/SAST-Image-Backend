using System.ComponentModel;
using Domain.Entity;

namespace Domain.Shared.Converter;

public sealed class OpenTypeConverter<T> : TypeConverter
    where T : ITypedId<T, long>, new()
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override object? ConvertFrom(
        ITypeDescriptorContext? context,
        System.Globalization.CultureInfo? culture,
        object value
    )
    {
        if (value is string stringValue && long.TryParse(stringValue, out long idValue))
        {
            T id = new() { Value = idValue };
            return id;
        }

        return base.ConvertFrom(context, culture, value);
    }
}
