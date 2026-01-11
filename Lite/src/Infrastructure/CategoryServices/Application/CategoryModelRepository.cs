using Application.CategoryServices;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.Shared;
using Infrastructure.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CategoryServices.Application;

internal sealed class CategoryModelRepository(QueryDbContext context) : ICategoryModelRepository
{
    public async Task AddAsync(CategoryModel entity, CancellationToken cancellationToken = default)
    {
        await context.Categories.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(CategoryId id, CancellationToken cancellationToken = default)
    {
        var category = await GetOrDefaultAsync(id, cancellationToken);
        if (category is not null)
            context.Categories.Remove(category);
    }

    public async Task<CategoryModel> GetAsync(
        CategoryId id,
        CancellationToken cancellationToken = default
    )
    {
        return await GetOrDefaultAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException();
    }

    private Task<CategoryModel?> GetOrDefaultAsync(
        CategoryId id,
        CancellationToken cancellationToken = default
    )
    {
        return context.Categories.FirstOrDefaultAsync(
            category => category.Id == id.Value,
            cancellationToken
        );
    }
}
