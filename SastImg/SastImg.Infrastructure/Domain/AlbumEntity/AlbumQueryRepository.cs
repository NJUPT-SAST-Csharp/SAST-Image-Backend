using System.Data;
using Dapper;
using SastImg.Application.AlbumServices.GetAlbum;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.AlbumServices.GetRemovedAlbums;
using SastImg.Application.AlbumServices.SearchAlbums;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.Domain.AlbumEntity
{
    internal sealed class AlbumQueryRepository(IDbConnectionFactory factory)
        : IGetAlbumsRepository,
            IGetAlbumRepository,
            ISearchAlbumsRepository,
            IGetRemovedAlbumsRepository
    {
        private readonly IDbConnection _connection = factory.GetConnection();
        private const int numPerPage = 20;

        #region GetAlbums

        public Task<IEnumerable<AlbumDto>> GetAlbumsAnonymousAsync(
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE accessibility = @PUBLIC "
                + "AND NOT is_removed "
                + "ORDER BY updated_at DESC "
                + "LIMIT @take;";

            return _connection.QueryAsync<AlbumDto>(
                sql,
                new { take = numPerPage, PUBLIC = Accessibility.Public }
            );
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
                + "author_id as AuthorId "
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
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE ( NOT is_removed ) "
                + "AND ( @authorId = 0 OR author_id = @authorId ) "
                + "AND ( accessibility <> @PRIVATE OR author_id = @requesterId OR @requesterId = ANY( collaborators ) ) "
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
                    requesterId,
                    PRIVATE = Accessibility.Private
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
                + "is_removed as IsRemoved, "
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE id = @albumId "
                + "AND ("
                + " ( accessibility <> @PRIVATE AND NOT is_removed )"
                + " OR ( author_id = @requesterId )"
                + " OR ( @requesterId = ANY( collaborators ) AND NOT is_removed ) "
                + ") "
                + "LIMIT 1";

            return _connection.QueryFirstOrDefaultAsync<DetailedAlbumDto>(
                sql,
                new
                {
                    albumId,
                    requesterId,
                    PRIVATE = Accessibility.Private
                }
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
                + "is_removed as IsRemoved, "
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE id = @albumId "
                + "LIMIT 1";
            return _connection.QueryFirstOrDefaultAsync<DetailedAlbumDto>(sql, new { albumId });
        }

        public Task<DetailedAlbumDto?> GetAlbumByAnonymousAsync(
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
                + "is_removed as IsRemoved, "
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE id = @albumId "
                + "AND NOT is_removed "
                + "AND accessibility = @PUBLIC "
                + "LIMIT 1";
            return _connection.QueryFirstOrDefaultAsync<DetailedAlbumDto>(
                sql,
                new { albumId, PUBLIC = Accessibility.Public }
            );
        }

        #endregion

        #region SearchAlbums

        public Task<IEnumerable<AlbumDto>> SearchAlbumsByAdminAsync(
            long categoryId,
            string title,
            int page,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE NOT is_removed "
                + "AND ( @categoryId = 0 OR category_id = @categoryId ) "
                + "AND ( @title = '' or title ILIKE @title ) "
                + "ORDER BY updated_at DESC "
                + "LIMIT @take "
                + "OFFSET @skip";
            return _connection.QueryAsync<AlbumDto>(
                sql,
                new
                {
                    take = numPerPage,
                    skip = page * numPerPage,
                    categoryId,
                    title = $"%{title}%"
                }
            );
        }

        public Task<IEnumerable<AlbumDto>> SearchAlbumsByUserAsync(
            long categoryId,
            string title,
            int page,
            long requesterId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE ( NOT is_removed ) "
                + "AND ( @categoryId = 0 OR category_id = @categoryId ) "
                + "AND ( accessibility <> @PRIVATE OR author_id = @requesterId OR @requesterId = ANY( collaborators ) ) "
                + "AND ( @title = '' OR title ILIKE @title ) "
                + "ORDER BY updated_at DESC "
                + "LIMIT @take "
                + "OFFSET @skip";

            return _connection.QueryAsync<AlbumDto>(
                sql,
                new
                {
                    take = numPerPage,
                    skip = page * numPerPage,
                    categoryId,
                    title = $"%{title}%",
                    requesterId,
                    PRIVATE = Accessibility.Private
                }
            );
        }

        #endregion

        #region GetRemovedAlbums

        public Task<IEnumerable<AlbumDto>> GetAlbumsByAdminAsync(
            long authorId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE is_removed "
                + "AND author_id = @authorId "
                + "ORDER BY updated_at DESC";

            return _connection.QueryAsync<AlbumDto>(sql, new { authorId });
        }

        public Task<IEnumerable<AlbumDto>> GetAlbumsByUserAsync(
            long requesterId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_url as CoverUri, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE is_removed "
                + "AND author_id = @authorId "
                + "ORDER BY updated_at DESC";

            return _connection.QueryAsync<AlbumDto>(sql, new { authorId = requesterId });
        }

        #endregion
    }
}
