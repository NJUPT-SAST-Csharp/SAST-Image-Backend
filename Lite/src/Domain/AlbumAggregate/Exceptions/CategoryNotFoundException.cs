using System.Diagnostics.CodeAnalysis;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.Extensions;

namespace Domain.AlbumAggregate.Exceptions;

public sealed class CategoryNotFoundException(CategoryId category) : DomainException
{
    public CategoryId Category { get; } = category;

    [DoesNotReturn]
    public static void Throw(CategoryId category) => throw new CategoryNotFoundException(category);
}
