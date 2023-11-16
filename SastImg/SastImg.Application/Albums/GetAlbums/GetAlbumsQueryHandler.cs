using System.Text.Json;
using SastImg.Application.Albums.Dtos;
using SastImg.Application.Services;
using Shared.Primitives.Query;

namespace SastImg.Application.Albums.GetAlbums
{
    public class GetAlbumsQueryHandler(IQueryDatabase database, ICache cache)
        : IQueryHandler<GetAlbumsQuery, IEnumerable<AlbumDto>>
    {
        private readonly IQueryDatabase _database = database;
        private readonly ICache _cache = cache;

        public async Task<IEnumerable<AlbumDto>> Handle(
            GetAlbumsQuery request,
            CancellationToken cancellationToken
        )
        {
            const int numPerPage = 20;

            int page = (request.Page < 0 || request.Page >= int.MaxValue / 20) ? 0 : request.Page;

            var albums = await GetFromCacheAsync(page);

            if (albums is not null)
                return albums;

            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_uri as CoverUri, "
                + "accessibility as Accessibility, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "ORDER BY updated_at DESC "
                + "LIMIT @take "
                + "OFFSET @skip";
            albums = await _database.QueryAsync<AlbumDto>(
                sql,
                new { take = numPerPage, skip = page * numPerPage },
                cancellationToken
            );

            _ = SetCacheAsync(page, albums);

            return albums;
        }

        private async Task<IEnumerable<AlbumDto>?> GetFromCacheAsync(int page)
        {
            var json = await _cache.HashGetAsync<string>(CacheKey.MainPages, page);
            if (json is null)
                return null;
            var albums = JsonSerializer.Deserialize<IEnumerable<AlbumDto>>(json);
            return albums;
        }

        private Task<bool> SetCacheAsync(int page, IEnumerable<AlbumDto> albums)
        {
            return _cache.HashSetAsync("MainPages", page, JsonSerializer.Serialize(albums));
        }
    }
}
