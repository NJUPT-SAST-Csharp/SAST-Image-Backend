using System.Data;
using Dapper;
using SastImg.Application.AlbumServices.GetAlbum;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.AlbumServices.SearchAlbum;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.QueryRepositories
{
    internal sealed class AlbumQueryRepository(IDbConnectionFactory factory)
        : IGetAlbumsRepository,
            IGetAlbumRepository,
            ISearchAlbumsRepository
    {
        private readonly IDbConnection _connection = factory.GetConnection();

        #region GetAlbums

        private const int numPerPage = 20;

        public Task<IEnumerable<AlbumDto>> GetAlbumsAnonymousAsync(
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
                + "AND NOT is_removed "
                + "ORDER BY updated_at DESC "
                + "LIMIT 20;";

            return _connection.QueryAsync<AlbumDto>(sql);
        }

        public Task<IEnumerable<AlbumDto>> GetAlbumsByAdminAsync(
            int page,
            long authorId,
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
                }
            );
        }

        public Task<IEnumerable<AlbumDto>> GetAlbumsByUserAsync(
            int page,
            long authorId,
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

        #endregion

        #region GetAlbum

        public Task<DetailedAlbumDto?> GetAlbumByUserAsync(
            long albumId,
            long requesterId,
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
                + "collaborators as Collaborators, "
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE id = @albumId "
                + "AND NOT is_removed "
                + "AND ( accessibility <> 2 OR author_id = @requesterId OR @requesterId = ANY( collaborators ) ) "
                + "LIMIT 1";

            return _connection.QueryFirstOrDefaultAsync<DetailedAlbumDto>(
                sql,
                new { albumId, requesterId }
            );
        }

        public Task<DetailedAlbumDto?> GetAlbumByAdminAsync(
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
                + "collaborators as Collaborators, "
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE id = @albumId "
                + "AND NOT is_removed "
                + "LIMIT 1";
            return _connection.QueryFirstOrDefaultAsync<DetailedAlbumDto>(sql, new { albumId });
        }

        public Task<DetailedAlbumDto?> GetAlbumByAnonymousAsync(
            string albumId,
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
                + "collaborators as Collaborators, "
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE id = @albumId "
                + "AND NOT is_removed "
                + "AND accessibility = 0 "
                + "LIMIT 1";
            return _connection.QueryFirstOrDefaultAsync<DetailedAlbumDto>(sql, new { albumId });
        }

        #endregion

        #region SearchAlbums

        public Task<IEnumerable<AlbumDto>> SearchAlbumsByAdminAsync(
            long categoryId,
            string title,
            int page
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AlbumDto>> SearchAlbumsByUserAsync(
            long categoryId,
            string title,
            int page,
            long requesterId
        )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
