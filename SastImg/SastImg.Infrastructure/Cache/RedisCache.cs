using System.Data;
using System.Text.Json;
using Dapper;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Infrastructure.Persistence.QueryDatabase;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Cache
{
    internal sealed class RedisCache(
        IConnectionMultiplexer connectionMultiplexer,
        IDbConnectionFactory factory
    ) : IGetAlbumsAnonymousCache
    {
        private readonly IConnectionMultiplexer _redisConnection = connectionMultiplexer;
        private readonly IDbConnection _connection = factory.GetConnection();
        private const int numPerPage = 20;

        public async Task<IEnumerable<AlbumDto>> GetAlbumsAsync(int page, long authorId)
        {
            var database = _redisConnection.GetDatabase();

            var values = await database.ListRangeAsync(authorId.ToString());

            var albums = values
                .Where(v => v != string.Empty)
                .Select(v => JsonSerializer.Deserialize<AlbumDto>(v.ToString())!);

            if (values.Length == 0)
            {
                albums = await GetAlbumsFromReposotory(authorId);
                if (!albums.Any())
                {
                    _ = database.ListRightPushAsync(authorId.ToString(), string.Empty);
                }
                else
                {
                    _ = SetAlbumsAsync(albums, authorId);
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

        public Task RemoveAlbumsAsync(long authorId)
        {
            var database = _redisConnection.GetDatabase();
            return database.KeyDeleteAsync(authorId.ToString());
        }

        public Task SetAlbumsAsync(IEnumerable<AlbumDto> albums, long authorId)
        {
            var database = _redisConnection.GetDatabase();
            var values = albums.Select(a => (RedisValue)JsonSerializer.Serialize(a)).ToArray();
            return database.ListRightPushAsync(authorId.ToString(), values);
        }

        private async Task<IEnumerable<AlbumDto>> GetAlbumsFromReposotory(long authorId)
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "accessibility as Accessibility, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE ( accessibility = 0 AND NOT is_removed ) "
                + "AND (@authorId = 0 OR author_id = @authorId) "
                + "ORDER BY updated_at DESC ";

            return await _connection.QueryAsync<AlbumDto>(sql, new { authorId });
        }
    }
}
