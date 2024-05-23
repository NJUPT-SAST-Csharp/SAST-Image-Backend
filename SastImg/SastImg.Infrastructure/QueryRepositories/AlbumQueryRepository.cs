using System.Data;
using Dapper;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.AlbumServices.GetDetailedAlbum;
using SastImg.Application.AlbumServices.GetRemovedAlbums;
using SastImg.Application.AlbumServices.GetUserAlbums;
using SastImg.Application.AlbumServices.SearchAlbums;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.CategoryEntity;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.QueryRepositories
{
    internal sealed class AlbumQueryRepository(IDbConnectionFactory factory)
        : IGetUserAlbumsRepository,
            IGetDetailedAlbumRepository,
            IGetAlbumsRepository,
            ISearchAlbumsRepository,
            IGetRemovedAlbumsRepository
    {
        private readonly IDbConnection _connection = factory.GetConnection();
        private const int numPerPage = 24;

        #region GetUserAlbums

        public Task<IEnumerable<UserAlbumDto>> GetUserAlbumsByAdminAsync(
            UserId authorId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_id as CoverId, "
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE ( NOT is_removed ) "
                + "AND ( @authorId = 0 OR author_id = @authorId ) "
                + "ORDER BY updated_at DESC ";

            return _connection.QueryAsync<UserAlbumDto>(sql, new { authorId = authorId.Value, });
        }

        public Task<IEnumerable<UserAlbumDto>> GetUserAlbumsByUserAsync(
            UserId authorId,
            UserId requesterId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_id as CoverId, "
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE ( NOT is_removed ) "
                + "AND ( @authorId = 0 OR author_id = @authorId ) "
                + "AND ( accessibility <> @PRIVATE OR author_id = @requesterId OR @requesterId = ANY( collaborators ) ) "
                + "ORDER BY updated_at DESC ";

            return _connection.QueryAsync<UserAlbumDto>(
                sql,
                new
                {
                    authorId = authorId.Value,
                    requesterId = requesterId.Value,
                    PRIVATE = Accessibility.Private
                }
            );
        }

        #endregion

        #region GetDetailedAlbum

        public Task<DetailedAlbumDto?> GetDetailedAlbumByUserAsync(
            AlbumId albumId,
            UserId requesterId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "description as Description, "
                + "cover_id as CoverId, "
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
                    albumId = albumId.Value,
                    requesterId = requesterId.Value,
                    PRIVATE = Accessibility.Private
                }
            );
        }

        public Task<DetailedAlbumDto?> GetDetailedAlbumByAdminAsync(
            AlbumId albumId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "description as Description, "
                + "cover_id as CoverId, "
                + "accessibility as Accessibility, "
                + "updated_at as UpdatedAt, "
                + "author_id as AuthorId, "
                + "collaborators as Collaborators, "
                + "is_removed as IsRemoved, "
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE id = @albumId "
                + "LIMIT 1";
            return _connection.QueryFirstOrDefaultAsync<DetailedAlbumDto>(
                sql,
                new { albumId = albumId.Value }
            );
        }

        public Task<DetailedAlbumDto?> GetDetailedAlbumByAnonymousAsync(
            AlbumId albumId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "description as Description, "
                + "cover_id as CoverId, "
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
                new { albumId = albumId.Value, PUBLIC = Accessibility.Public }
            );
        }

        #endregion

        #region SearchAlbums

        public Task<IEnumerable<SearchAlbumDto>> SearchAlbumsByAdminAsync(
            CategoryId categoryId,
            string title,
            int page,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_id as CoverId, "
                + "category_id as CategoryId, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE NOT is_removed "
                + "AND ( @categoryId = 0 OR category_id = @categoryId ) "
                + "AND ( @title = '' or title ILIKE @title ) "
                + "ORDER BY updated_at DESC "
                + "LIMIT @take "
                + "OFFSET @skip";
            return _connection.QueryAsync<SearchAlbumDto>(
                sql,
                new
                {
                    take = numPerPage,
                    skip = page * numPerPage,
                    categoryId = categoryId.Value,
                    title = $"%{title}%"
                }
            );
        }

        public Task<IEnumerable<SearchAlbumDto>> SearchAlbumsByUserAsync(
            CategoryId categoryId,
            string title,
            int page,
            UserId requesterId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_id as CoverId, "
                + "category_id as CategoryId, "
                + "author_id as AuthorId "
                + "FROM albums "
                + "WHERE ( NOT is_removed ) "
                + "AND ( @categoryId = 0 OR category_id = @categoryId ) "
                + "AND ( accessibility <> @PRIVATE OR author_id = @requesterId OR @requesterId = ANY( collaborators ) ) "
                + "AND ( @title = '' OR title ILIKE @title ) "
                + "ORDER BY updated_at DESC "
                + "LIMIT @take "
                + "OFFSET @skip";

            return _connection.QueryAsync<SearchAlbumDto>(
                sql,
                new
                {
                    take = numPerPage,
                    skip = page * numPerPage,
                    categoryId = categoryId.Value,
                    title = $"%{title}%",
                    requesterId = requesterId.Value,
                    PRIVATE = Accessibility.Private
                }
            );
        }

        #endregion

        #region GetRemovedAlbums

        public Task<IEnumerable<RemovedAlbumDto>> GetRemovedAlbumsByAdminAsync(
            UserId authorId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_id as CoverId "
                + "FROM albums "
                + "WHERE is_removed "
                + "AND author_id = @authorId "
                + "ORDER BY updated_at DESC";

            return _connection.QueryAsync<RemovedAlbumDto>(sql, new { authorId = authorId.Value });
        }

        public Task<IEnumerable<RemovedAlbumDto>> GetRemovedAlbumsByUserAsync(
            UserId requesterId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_id as CoverId "
                + "FROM albums "
                + "WHERE is_removed "
                + "AND author_id = @authorId "
                + "ORDER BY updated_at DESC";

            return _connection.QueryAsync<RemovedAlbumDto>(
                sql,
                new { authorId = requesterId.Value }
            );
        }

        #endregion

        #region GetAlbums

        public Task<IEnumerable<AlbumDto>> GetAlbumsByAdminAsync(
            CategoryId categoryId,
            int page,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_id as CoverId, "
                + "author_id as AuthorId,"
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE NOT is_removed "
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
                    categoryId = categoryId.Value
                }
            );
        }

        public Task<IEnumerable<AlbumDto>> GetAlbumsByUserAsync(
            CategoryId categoryId,
            int page,
            UserId requester,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_id as CoverId, "
                + "author_id as AuthorId,"
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE NOT is_removed "
                + "AND ( @categoryId = 0 OR category_id = @categoryId ) "
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
                    categoryId = categoryId.Value,
                    requesterId = requester.Value,
                    PRIVATE = Accessibility.Private
                }
            );
        }

        public Task<IEnumerable<AlbumDto>> GetAlbumsAnonymousAsync(
            CategoryId categoryId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "id as AlbumId, "
                + "title as Title, "
                + "cover_id as CoverId, "
                + "author_id as AuthorId,"
                + "category_id as CategoryId "
                + "FROM albums "
                + "WHERE NOT is_removed "
                + "AND ( @categoryId = 0 OR category_id = @categoryId ) "
                + "AND accessibility = @PUBLIC "
                + "ORDER BY updated_at DESC "
                + "LIMIT @take";

            return _connection.QueryAsync<AlbumDto>(
                sql,
                new
                {
                    categoryId = categoryId.Value,
                    PUBLIC = Accessibility.Public,
                    take = numPerPage
                }
            );
        }

        #endregion
    }
}
