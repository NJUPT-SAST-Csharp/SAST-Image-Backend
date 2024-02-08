using SastImg.Application.CategoryServices.GetAllCategories;

namespace SastImg.Application.CategoryServices
{
    public interface ICategoryQueryRepository
    {
        public Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(
            CancellationToken cancellationToken = default
        );
    }
}
