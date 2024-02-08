using SastImg.Domain.CategoryEntity;

namespace SastImg.Domain.Categories
{
    public interface ICategoryRepository
    {
        public Task<CategoryId> AddCatergoryAsync(
            Category category,
            CancellationToken cancellationToken = default
        );

        public Task<Category> GetCatogoryAsync(
            CategoryId id,
            CancellationToken cancellationToken = default
        );
    }
}
