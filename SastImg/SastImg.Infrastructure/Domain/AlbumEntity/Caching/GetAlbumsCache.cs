using System.Text.Json;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.SeedWorks;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Domain.AlbumEntity.Caching
{
    internal sealed class GetAlbumsCache(
        IConnectionMultiplexer connectionMultiplexer,
        IGetAlbumsRepository albums
    ) : ICache<IEnumerable<AlbumDto>>
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

        private readonly IGetAlbumsRepository _albums = albums;

        public async Task<IEnumerable<AlbumDto>?> GetCachingAsync(
            string key,
            CancellationToken cancellationToken = default
        )
        {
            var value = await _database.StringGetAsync(CacheKeys.Albums.ToString());
            if (value.IsNull)
            {
                var albums = await _albums.GetAlbumsAnonymousAsync(cancellationToken);
                _ = ResetCachingAsync(CacheKeys.Albums.ToString(), albums, cancellationToken);
                return albums;
            }
            return JsonSerializer.Deserialize<IEnumerable<AlbumDto>>(value!)
                ?? throw new JsonException("Couldn't deserialize the albums string.");
        }

        public Task DeleteCachingAsync(string key, CancellationToken cancellationToken = default)
        {
            return _database.KeyDeleteAsync(CacheKeys.Albums.ToString());
        }

        public Task ResetCachingAsync(
            string key,
            IEnumerable<AlbumDto>? value,
            CancellationToken cancellationToken = default
        )
        {
            string str = string.Empty;
            if (value is not null)
            {
                str = JsonSerializer.Serialize(value);
            }
            return _database.StringSetAsync(CacheKeys.Albums.ToString(), str);
        }
    }
}
