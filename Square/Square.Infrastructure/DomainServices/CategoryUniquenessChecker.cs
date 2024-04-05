using Microsoft.EntityFrameworkCore;
using Square.Domain.CategoryAggregate;
using Square.Domain.CategoryAggregate.CategoryEntity;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.DomainServices
{
    internal sealed class CategoryUniquenessChecker(SquareDbContext context)
        : ICategoryUniquenessChecker
    {
        private readonly SquareDbContext _context = context;

        public Task<bool> IsConflictAsync(CategoryName title)
        {
            return _context
                .Categories.AsNoTracking()
                .Select(c => new { Name = EF.Property<CategoryName>(c, "_name") })
                .AnyAsync(c => c.Name == title);
        }
    }
}
