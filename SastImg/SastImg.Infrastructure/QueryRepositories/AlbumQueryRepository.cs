using System.Data;
using Dapper;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.QueryRepositories
{
    internal sealed class AlbumQueryRepository(IDbConnectionFactory factory) : IGetAlbumsRepository
    {
        private readonly IDbConnection _connection = factory.GetConnection();

        private const int numPerPage = 20;

        public Task<IEnumerable<AlbumDto>> GetAlbumsAnonymousAsync(int page, long authorId)
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "accessibility as Accessibility, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE accessibility = 0 AND ( NOT is_removed ) "
                + "AND (@authorId = 0 OR author_id = @authorId) "
                + "ORDER BY updated_at DESC "
                + "LIMIT @take "
                + "OFFSET @skip";

            return _connection.QueryAsync<AlbumDto>(
                sql,
                new
                {
                    take = numPerPage,
                    skip = page * numPerPage,
                    authorId
                }
            );
        }

        public Task<IEnumerable<AlbumDto>> GetAlbumsByAdminAsync(int page, long authorId)
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "accessibility as Accessibility, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE ( NOT is_removed ) AND (@authorId = 0 OR author_id = @authorId) "
                + "ORDER BY updated_at DESC "
                + "LIMIT @take "
                + "OFFSET @skip";

            return _connection.QueryAsync<AlbumDto>(
                sql,
                new
                {
                    take = numPerPage,
                    skip = page * numPerPage,
                    authorId
                }
            );
        }

        public Task<IEnumerable<AlbumDto>> GetAlbumsByUserAsync(
            int page,
            long authorId,
            long requesterId
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "accessibility as Accessibility, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE ( NOT is_removed ) "
                + "AND ( @authorId = 0 OR author_id = @authorId ) "
                + "AND ( accessibility NOT 2 OR author_id = @requesterId ) "
                + " (@authorId = 0 OR author_id = @authorId) "
                + "ORDER BY updated_at DESC "
                + "LIMIT @take "
                + "OFFSET @skip";

            return _connection.QueryAsync<AlbumDto>(
                sql,
                new
                {
                    take = numPerPage,
                    skip = page * numPerPage,
                    authorId,
                    requesterId
                }
            );
        }
    }
}
