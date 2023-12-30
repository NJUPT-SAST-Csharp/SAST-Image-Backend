using System.Text.Json;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.SeedWorks;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Domain.ImageEntity.Caching
{
    internal sealed class GetImagesCache(
        IConnectionMultiplexer connection,
        IGetImagesRepository repository
    ) : ICache<IEnumerable<ImageDto>>
    {
        private readonly IDatabase _database = connection.GetDatabase();
        private readonly IGetImagesRepository _repository = repository;

        public Task DeleteCachingAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ImageDto>?> GetCachingAsync(
            string key,
            CancellationToken cancellationToken = default
        )
        {
            var value = await _database.HashGetAsync(CacheKeys.Images.ToString(), key);
            if (value.IsNull)
            {
                var images = await _repository.GetImagesByAnonymousAsync(
                    long.Parse(key),
                    cancellationToken
                );
                _ = ResetCachingAsync(key, images, cancellationToken);
                return images;
            }

            return JsonSerializer.Deserialize<IEnumerable<ImageDto>>(value!)
                ?? throw new JsonException("Couldn't deserialize the albums string.");
        }

        public Task ResetCachingAsync(
            string key,
            IEnumerable<ImageDto>? value,
            CancellationToken cancellationToken = default
        )
        {
            string caching = value is null ? string.Empty : JsonSerializer.Serialize(value);
            return _database.HashSetAsync(CacheKeys.Images.ToString(), key, caching);
        }
    }
}
