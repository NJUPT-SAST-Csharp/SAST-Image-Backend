using Domain.CategoryAggregate.CategoryEntity;

namespace Domain.CategoryAggregate;

internal interface ICategoryRepository
{
    public Task AddAsync(Category category, CancellationToken cancellationToken);

    public Task<Category> GetAsync(CategoryId id, CancellationToken cancellationToken);
}
