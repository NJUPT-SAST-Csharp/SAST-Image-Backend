using System.Diagnostics.CodeAnalysis;

namespace Domain.Entity;

public interface IFactoryConstructor<TObject>
{
    public static abstract bool TryCreateNew([NotNullWhen(true)] out TObject? entity);
}

public interface IFactoryConstructor<TObject, TInput>
{
    public static abstract bool TryCreateNew(
        TInput input,
        [NotNullWhen(true)] out TObject? newObject
    );
}
