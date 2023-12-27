using System.Data;
using System.Text.Json;
using SastImg.Application.AlbumServices.GetAlbums;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Cache
{
    internal sealed class RedisCache(
        IConnectionMultiplexer connectionMultiplexer,
        IGetAlbumsRepository repository
    ) : IGetAlbumsAnonymousCache
    {
        private readonly IConnectionMultiplexer _redisConnection = connectionMultiplexer;
        private readonly IGetAlbumsRepository _repository = repository;

        private const int numPerPage = 20;

        public async Task<IEnumerable<AlbumDto>> GetAlbumsAsync(
            int page,
            long categoryId,
            CancellationToken cancellationToken = default
        )
        {
            var database = _redisConnection.GetDatabase();

            var values = await database.ListRangeAsync(categoryId.ToString());

            var albums = values
                .Where(v => v != string.Empty)
                .Select(v => JsonSerializer.Deserialize<AlbumDto>(v.ToString())!);

            if (values.Length == 0)
            {
                albums = await _repository.GetAlbumsAnonymousAsync(categoryId);
                if (!albums.Any())
                {
                    _ = database.ListRightPushAsync(categoryId.ToString(), string.Empty);
                }
                else
                {
                    _ = SetAlbumsAsync(albums, categoryId);
                }
            }

            int skip = page * numPerPage;

            if (skip > values.Length)
            {
                return albums.Take(skip);
            }
            else
            {
                return albums.Skip(skip).Take(numPerPage);
            }
        }

        public Task RemoveAlbumsAsync(long authorId, CancellationToken cancellationToken = default)
        {
            var database = _redisConnection.GetDatabase();
            return database.KeyDeleteAsync(authorId.ToString());
        }

        public Task SetAlbumsAsync(
            IEnumerable<AlbumDto> albums,
            long authorId,
            CancellationToken cancellationToken = default
        )
        {
            var database = _redisConnection.GetDatabase();
            var values = albums.Select(a => (RedisValue)JsonSerializer.Serialize(a)).ToArray();
            return database.ListRightPushAsync(authorId.ToString(), values);
        }
    }
}
