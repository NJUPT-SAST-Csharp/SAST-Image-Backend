using System.ComponentModel;
using Primitives.ValueObject;

namespace Primitives.Utilities;

public sealed class OpenType<TObject> : TypeConverter
    where TObject : IValueObject<TObject, string>
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
        if (value is string stringValue && TObject.TryCreateNew(stringValue, out var newObject))
        {
            return newObject;
        }

        return base.ConvertFrom(context, culture, value);
    }
}
