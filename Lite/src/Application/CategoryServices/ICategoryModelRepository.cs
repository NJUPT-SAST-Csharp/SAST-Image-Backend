using Domain.CategoryAggregate.CategoryEntity;

namespace Application.CategoryServices;

public interface ICategoryModelRepository
{
    public Task<CategoryModel> GetAsync(CategoryId id, CancellationToken cancellationToken);

    public Task AddAsync(CategoryModel category, CancellationToken cancellationToken);
}
