using System.Data;
using Dapper;
using SastImg.Application.AlbumServices.GetAlbum;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.QueryRepositories
{
    internal sealed class AlbumQueryRepository(IDbConnectionFactory factory)
        : IGetAlbumsRepository,
            IGetAlbumRepository
    {
        private readonly IDbConnection _connection = factory.GetConnection();

        private const int numPerPage = 20;

        public Task<IEnumerable<AlbumDto>> GetAlbumsAnonymousAsync(
            long categoryId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "accessibility as Accessibility, "
                + "author_id as AuthorId, "
                + "updated_at as UpdatedAt "
                + "FROM albums "
                + "WHERE accessibility = 0 "
                + "AND ( NOT is_removed ) "
                + "AND ( @categoryId = 0 OR category_id = @categoryId ) "
                + "ORDER BY updated_at DESC ";

            return _connection.QueryAsync<AlbumDto>(sql, new { categoryId });
        }

        public Task<IEnumerable<AlbumDto>> GetAlbumsByAdminAsync(
            int page,
            long authorId,
            long categoryId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "accessibility as Accessibility, "
                + "author_id as AuthorId, "
                + "updated_at as UpdatedAt "
                + "FROM albums "
                + "WHERE ( NOT is_removed ) "
                + "AND ( @authorId = 0 OR author_id = @authorId ) "
                + "AND ( @categoryId = 0 OR category_id = @categoryId ) "
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
                    categoryId
                }
            );
        }

        public Task<IEnumerable<AlbumDto>> GetAlbumsByUserAsync(
            int page,
            long authorId,
            long categoryId,
            long requesterId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "accessibility as Accessibility, "
                + "updated_at as UpdatedAt, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE ( NOT is_removed ) "
                + "AND ( @authorId = 0 OR author_id = @authorId ) "
                + "AND ( accessibility <> 2 OR author_id = @requesterId OR @requesterId = ANY( collaborators ) ) "
                + "AND ( @categoryId = 0 OR category_id = @categoryId ) "
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
                    categoryId,
                    requesterId
                }
            );
        }

        public Task<DetailedAlbumDto?> GetDetailedAlbumAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "description as Description, "
                + "cover_url as CoverUri, "
                + "accessibility as Accessibility, "
                + "updated_at as UpdatedAt, "
                + "author_id as AuthorId, "
                + "is_removed as IsRemoved, "
                + "collaborators as Collaborators, "
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE id = @albumId "
                + "LIMIT 1";

            return _connection.QueryFirstOrDefaultAsync<DetailedAlbumDto>(sql, new { albumId });
        }
    }
}
