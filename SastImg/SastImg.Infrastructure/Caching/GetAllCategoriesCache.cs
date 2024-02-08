using System.Text.Json;
using SastImg.Application.CategoryServices;
using SastImg.Application.CategoryServices.GetAllCategory;
using SastImg.Application.SeedWorks;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Caching
{
    internal class GetAllCategoriesCache(
        IConnectionMultiplexer connection,
        ICategoryQueryRepository repository
    ) : ICache<IEnumerable<CategoryDto>>
    {
        private readonly IDatabase _database = connection.GetDatabase();
        private readonly ICategoryQueryRepository _repository = repository;

        public Task DeleteCachingAsync(string key, CancellationToken cancellationToken = default)
        {
            return _database.KeyDeleteAsync(nameof(CacheKeys.Categories));
        }

        public async Task<IEnumerable<CategoryDto>?> GetCachingAsync(
            string key,
            CancellationToken cancellationToken = default
        )
        {
            var categories = await _database.StringGetAsync(nameof(CacheKeys.Categories));
            if (categories.HasValue)
            {
                JsonSerializer.Deserialize<IEnumerable<CategoryDto>>(categories!);
            }
            var results = await _repository.GetAllCategoriesAsync();
            var serialized = JsonSerializer.Serialize(results);
            await _database.StringSetAsync(nameof(CacheKeys.Categories), serialized);

            return results;
        }

        public Task ResetCachingAsync(
            string key,
            IEnumerable<CategoryDto>? value,
            CancellationToken cancellationToken = default
        )
        {
            string str = string.Empty;
            if (value is not null)
            {
                str = JsonSerializer.Serialize(value);
            }
            return _database.StringSetAsync(nameof(CacheKeys.Categories), str);
        }
    }
}
