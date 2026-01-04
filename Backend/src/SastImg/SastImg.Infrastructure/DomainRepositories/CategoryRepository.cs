using Primitives.Exceptions;
using SastImg.Domain.Categories;
using SastImg.Domain.CategoryEntity;
using SastImg.Infrastructure.Persistence;

namespace SastImg.Infrastructure.DomainRepositories;

internal sealed class CategoryRepository(SastImgDbContext context) : ICategoryRepository
{
    private readonly SastImgDbContext _context = context;

    public async Task<CategoryId> AddCatergoryAsync(
        Category category,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _context.Categories.AddAsync(category, cancellationToken);
        return entity.Entity.Id;
    }

    public async Task<Category> GetCatogoryAsync(
        CategoryId id,
        CancellationToken cancellationToken = default
    )
    {
        var category = await _context.Categories.FindAsync([id], cancellationToken);

        EntityNotFoundException.ThrowIf(category is null);

        return category;
    }
}
