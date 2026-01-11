using Domain.CategoryAggregate;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.Shared;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CategoryServices.Domain;

internal sealed class CategoryDomainRepository(DomainDbContext context) : ICategoryRepository
{
    public async Task AddAsync(Category entity, CancellationToken cancellationToken = default)
    {
        await context.Categories.AddAsync(entity, cancellationToken);
    }

    public async Task<Category> GetAsync(
        CategoryId id,
        CancellationToken cancellationToken = default
    )
    {
        var category =
            await context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException();

        return category;
    }
}
