using Microsoft.EntityFrameworkCore;
using Square.Application.CategoryServices;
using Square.Domain.CategoryAggregate.CategoryEntity;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.ApplicationServices
{
    internal sealed class CategoryQueryRepository(SquareDbContext context)
        : ICategoryQueryRepository
    {
        private readonly SquareDbContext _context = context;

        public void AddCategory(CategoryModel category)
        {
            _context.CategoryModels.Add(category);
        }

        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
        {
            return await _context.CategoryModels.ToListAsync();
        }

        public Task<CategoryModel?> GetCategoryAsync(CategoryId id)
        {
            return _context.CategoryModels.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
