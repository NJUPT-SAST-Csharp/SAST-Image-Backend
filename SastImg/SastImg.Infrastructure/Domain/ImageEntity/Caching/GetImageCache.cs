using System.Text.Json;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.SeedWorks;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Domain.ImageEntity.Caching
{
    internal sealed class GetImageCache(
        IConnectionMultiplexer connection,
        IGetImageRepository repository
    ) : ICache<DetailedImageDto>
    {
        private readonly IGetImageRepository _repository = repository;
        private readonly IDatabase _database = connection.GetDatabase();

        public Task DeleteCachingAsync(string key, CancellationToken cancellationToken = default)
        {
            return _database.HashDeleteAsync(CacheKeys.DetailedImages.ToString(), key);
        }

        public async Task<DetailedImageDto?> GetCachingAsync(
            string key,
            CancellationToken cancellationToken = default
        )
        {
            var value = await _database.HashGetAsync(CacheKeys.DetailedImages.ToString(), key);

            if (value.IsNull)
            {
                var image = await _repository.GetImageByAnonymousAsync(
                    long.Parse(key),
                    cancellationToken
                );
                _ = ResetCachingAsync(key, image, cancellationToken);
                return image;
            }
            else if (value == string.Empty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<DetailedImageDto>(value!)
                ?? throw new JsonException("Couldn't deserialize the image string.");
        }

        public Task ResetCachingAsync(
            string key,
            DetailedImageDto? value,
            CancellationToken cancellationToken = default
        )
        {
            string caching = value is null ? string.Empty : JsonSerializer.Serialize(value);
            return _database.HashSetAsync(CacheKeys.DetailedImages.ToString(), key, caching);
        }
    }
}
