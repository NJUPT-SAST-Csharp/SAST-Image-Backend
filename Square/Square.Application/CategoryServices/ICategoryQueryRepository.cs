using Square.Domain.CategoryAggregate.CategoryEntity;

namespace Square.Application.CategoryServices
{
    public interface ICategoryQueryRepository
    {
        public Task<CategoryModel?> GetCategoryAsync(CategoryId id);

        public Task<IEnumerable<CategoryModel>> GetCategoriesAsync();

        public void AddCategory(CategoryModel category);
    }
}
