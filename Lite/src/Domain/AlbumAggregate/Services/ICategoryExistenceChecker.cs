using Domain.CategoryAggregate.CategoryEntity;

namespace Domain.AlbumAggregate.Services;

public interface ICategoryExistenceChecker
{
    public Task CheckAsync(CategoryId category, CancellationToken cancellationToken = default);
}
