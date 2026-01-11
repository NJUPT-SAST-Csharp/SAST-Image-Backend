using Application.CategoryServices.Queries;
using Application.Shared;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CategoryServices.Application;

internal sealed class CategoryQueryRepository(QueryDbContext context)
    : IQueryRepository<CategoriesQuery, CategoryDto[]>
{
    public Task<CategoryDto[]> GetOrDefaultAsync(
        CategoriesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return context
            .Categories.Select(c => new CategoryDto(c.Id, c.Name, c.Description))
            .ToArrayAsync(cancellationToken);
    }
}
