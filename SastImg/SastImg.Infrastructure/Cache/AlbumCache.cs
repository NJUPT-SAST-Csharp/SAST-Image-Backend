using System.Text.Json;
using SastImg.Application.AlbumServices.GetAlbum;
using SastImg.Application.AlbumServices.GetAlbums;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Cache
{
    internal sealed class AlbumCache(
        IConnectionMultiplexer connectionMultiplexer,
        IGetAlbumsRepository albums,
        IGetAlbumRepository album
    ) : IGetAlbumsCache, IGetAlbumCache
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
        private readonly IGetAlbumsRepository _albums = albums;
        private readonly IGetAlbumRepository _album = album;

        const int numPerPage = 20;

        public async Task<IEnumerable<AlbumDto>> GetAlbumsAsync(
            int page,
            long categoryId,
            CancellationToken cancellationToken = default
        )
        {
            // TODO: Refactor
            var values = await _database.ListRangeAsync(categoryId.ToString());
            var albumStrs = values
                .Where(v => string.IsNullOrWhiteSpace(v) == false)
                .Select(v => JsonSerializer.Deserialize<AlbumDto>(v.ToString())!);

            if (values.Length == 0)
            {
                albumStrs = await _albums.GetAlbumsAnonymousAsync(categoryId, cancellationToken);
                if (!albumStrs.Any())
                {
                    _ = _database.ListRightPushAsync(categoryId.ToString(), string.Empty);
                }
                else
                {
                    _ = SetAlbumsAsync(albumStrs, categoryId, cancellationToken);
                }
            }

            int skip = page * numPerPage;

            if (skip > values.Length)
            {
                return albumStrs.Take(skip);
            }
            else
            {
                return albumStrs.Skip(skip).Take(numPerPage);
            }
        }

        public Task RemoveAlbumsAsync(long authorId, CancellationToken cancellationToken = default)
        {
            return _database.KeyDeleteAsync(authorId.ToString());
        }

        public async Task<DetailedAlbumDto?> GetAlbumAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            var cacheAlbum = await _database.HashGetAsync(
                CacheKeys.Albums.ToString(),
                albumId.ToString()
            );

            if (cacheAlbum.IsNull)
            {
                var album = await _album.GetDetailedAlbumByAnonymousAsync(
                    albumId,
                    cancellationToken
                );

                _ = SetAlbumAsync(album, albumId, cancellationToken);

                return album;
            }
            else if (cacheAlbum == string.Empty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<DetailedAlbumDto>(cacheAlbum.ToString())
                ?? throw new InvalidOperationException(
                    "Couldn't deserialize the specific album (output null but shouldn't)."
                );
        }

        public Task RemoveAlbumAsync(long albumId, CancellationToken cancellationToken = default)
        {
            return _database.HashDeleteAsync(CacheKeys.Albums.ToString(), albumId.ToString());
        }

        #region InnerMethods

        private Task<long> SetAlbumsAsync(
            IEnumerable<AlbumDto> albums,
            in long categoryId,
            CancellationToken cancellationToken = default
        )
        {
            var values = albums.Select(a => (RedisValue)JsonSerializer.Serialize(a)).ToArray();
            return _database.ListRightPushAsync(categoryId.ToString(), values);
        }

        private Task<bool> SetAlbumAsync(
            DetailedAlbumDto? album,
            in long albumId,
            CancellationToken cancellationToken = default
        )
        {
            string cacheValue = string.Empty;

            if (album is not null)
            {
                cacheValue = JsonSerializer.Serialize(album);
            }

            return _database.HashSetAsync(
                CacheKeys.Albums.ToString(),
                albumId.ToString(),
                cacheValue
            );
        }

        #endregion
    }
}
