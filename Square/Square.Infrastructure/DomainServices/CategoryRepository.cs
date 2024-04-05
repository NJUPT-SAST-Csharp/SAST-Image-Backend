using Microsoft.EntityFrameworkCore;
using Square.Domain.CategoryAggregate;
using Square.Domain.CategoryAggregate.CategoryEntity;
using Square.Infrastructure.Persistence;

namespace Square.Infrastructure.DomainServices
{
    internal sealed class CategoryRepository(SquareDbContext context) : ICategoryRepository
    {
        private readonly SquareDbContext _context = context;

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
        }

        public void DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
        }

        public Task<Category?> GetCategoryAsync(CategoryId categoryId)
        {
            return _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        }
    }
}
