using System.Text.Json;
using SastImg.Application.AlbumServices.GetDetailedAlbum;
using SastImg.Application.SeedWorks;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Caching
{
    internal sealed class GetAlbumCache(
        IConnectionMultiplexer connection,
        IGetDetailedAlbumRepository repository
    ) : ICache<DetailedAlbumDto>
    {
        private readonly IDatabase _database = connection.GetDatabase();
        private readonly IGetDetailedAlbumRepository _repository = repository;

        public Task DeleteCachingAsync(string key, CancellationToken cancellationToken = default)
        {
            return _database.HashDeleteAsync(CacheKeys.DetailedAlbums.ToString(), key);
        }

        public async Task<DetailedAlbumDto?> GetCachingAsync(
            string key,
            CancellationToken cancellationToken = default
        )
        {
            var value = await _database.HashGetAsync(CacheKeys.DetailedAlbums.ToString(), key);

            if (value.IsNullOrEmpty)
            {
                var album = await _repository.GetDetailedAlbumByAnonymousAsync(
                    new(long.Parse(key)),
                    cancellationToken
                );
                _ = ResetCachingAsync(key, album, cancellationToken);
                return album;
            }
            else
            {
                return JsonSerializer.Deserialize<DetailedAlbumDto>(value!)
                    ?? throw new JsonException("Couldn't deserialize the album string.");
            }
        }

        public Task ResetCachingAsync(
            string key,
            DetailedAlbumDto? value,
            CancellationToken cancellationToken = default
        )
        {
            string str = string.Empty;
            if (value is not null)
            {
                str = JsonSerializer.Serialize(value);
            }
            return _database.HashSetAsync(CacheKeys.DetailedAlbums.ToString(), key, str);
        }
    }
}
