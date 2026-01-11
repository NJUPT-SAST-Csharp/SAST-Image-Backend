using Domain.Entity;
using Domain.Shared;

namespace WebAPI.Utilities;

public static class ModelBindExtensions
{
    public static TObject Bind<TObject>(this string value)
        where TObject : IValueObject<TObject, string>, IFactoryConstructor<TObject, string>
    {
        if (TObject.TryCreateNew(value, out var model) == false)
        {
            throw new DomainModelInvalidException(value?.ToString());
        }

        return model;
    }

    public static TObject Bind<TObject>(this int value)
        where TObject : IValueObject<TObject, int>, IFactoryConstructor<TObject, int>
    {
        if (TObject.TryCreateNew(value, out var model) == false)
        {
            throw new DomainModelInvalidException(value.ToString());
        }

        return model;
    }

    public static TObject Bind<TObject, TValue>(this TValue value)
        where TObject : IValueObject<TObject, TValue>, IFactoryConstructor<TObject, TValue>
    {
        if (TObject.TryCreateNew(value, out var model) == false)
        {
            throw new DomainModelInvalidException(value?.ToString());
        }

        return model;
    }
}
