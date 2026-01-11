using Domain.CategoryAggregate.CategoryEntity;

namespace Domain.CategoryAggregate.Exceptions;

public sealed class CategoryNameDuplicateException(CategoryName name) : Exception
{
    public CategoryName Name { get; } = name;
}
