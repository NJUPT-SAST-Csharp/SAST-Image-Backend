using System.Data;
using Dapper;
using SastImg.Application.Albums.Dtos;
using SastImg.Application.Services;
using Shared.Primitives.Query;

namespace SastImg.Application.Albums.GetAlbums
{
    public class GetAlbumsQueryHandler : IQueryHandler<GetAlbumsQuery, IEnumerable<AlbumDto>>
    {
        private readonly IDbConnection _connection;

        public GetAlbumsQueryHandler(IDbConnectionProvider connection)
        {
            _connection = connection.DbConnection;
        }

        public Task<IEnumerable<AlbumDto>> Handle(
            GetAlbumsQuery request,
            CancellationToken cancellationToken
        )
        {
            const string sql =
                "SELECT "
                + "author_id as AuthorId, "
                + "id as AlbumId, "
                + "title as Title, "
                + "accessibility as Accessibility, "
                + "cover_uri as CoverUri "
                + "FROM albums";
            var albums = _connection.QueryAsync<AlbumDto>(sql);
            return albums;
        }
    }
}
