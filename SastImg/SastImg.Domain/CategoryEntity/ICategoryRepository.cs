using SastImg.Domain.CategoryEntity;

namespace SastImg.Domain.Categories
{
    public interface ICategoryRepository
    {
        public Task<long> CreateCatetoryAsync(
            string name,
            string description = "",
            CancellationToken cancellationToken = default
        );

        public Task DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        public Task DeleteCategoryByNameAsync(
            string name,
            CancellationToken cancellationToken = default
        );

        public Task UpdateCatoryInfoAsync(
            string name,
            string? description = null,
            CancellationToken cancellationToken = default
        );

        public Task<Category> GetCatogoryByIdAsync(
            int id,
            CancellationToken cancellationToken = default
        );

        public Task<Category> GetCategoryByNameAsync(
            string name,
            CancellationToken cancellationToken = default
        );
    }
}
