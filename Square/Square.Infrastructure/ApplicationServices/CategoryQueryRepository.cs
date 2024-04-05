using Microsoft.EntityFrameworkCore;
using Square.Application.CategoryServices;
using Square.Domain.CategoryAggregate.CategoryEntity;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.ApplicationServices
{
    internal sealed class CategoryQueryRepository(SquareQueryDbContext context)
        : ICategoryQueryRepository
    {
        private readonly SquareQueryDbContext _context = context;

        public void AddCategory(CategoryModel category)
        {
            _context.Categories.Add(category);
        }

        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public Task<CategoryModel?> GetCategoryAsync(CategoryId id)
        {
            return _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
