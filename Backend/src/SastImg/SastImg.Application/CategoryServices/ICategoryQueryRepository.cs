using SastImg.Application.CategoryServices.GetAllCategory;

namespace SastImg.Application.CategoryServices;

public interface ICategoryQueryRepository
{
    public Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(
        CancellationToken cancellationToken = default
    );
}
