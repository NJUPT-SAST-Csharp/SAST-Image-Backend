using Square.Domain.CategoryAggregate.CategoryEntity;

namespace Square.Domain.CategoryAggregate
{
    public interface ICategoryRepository
    {
        public void AddCategory(Category category);
        public void DeleteCategory(Category category);
        public Task<Category?> GetCategoryAsync(CategoryId categoryId);
    }
}
