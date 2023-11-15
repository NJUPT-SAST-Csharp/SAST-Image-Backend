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
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_uri as CoverUri, "
                + "accessibility as Accessibility, "
                + "author_id as AuthorId "
                + "FROM albums";
            var albums = await _database.QueryAsync<AlbumDto>(
                sql,
                cancellationToken: cancellationToken
            );
            return albums;
        }
    }
}
