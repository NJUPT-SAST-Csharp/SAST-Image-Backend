using Domain.CategoryAggregate.CategoryEntity;

namespace Domain.CategoryAggregate.Services;

public interface ICategoryNameUniquenessChecker
{
    public Task CheckAsync(CategoryName name, CancellationToken cancellationToken = default);
}
