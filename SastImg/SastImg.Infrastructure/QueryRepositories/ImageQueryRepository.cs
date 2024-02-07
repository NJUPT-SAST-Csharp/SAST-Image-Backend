using System.Data;
using Dapper;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.ImageServices.GetRemovedImages;
using SastImg.Application.ImageServices.SearchImages;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using SastImg.Domain.CategoryEntity;
using SastImg.Domain.TagEntity;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.QueryRepositories
{
    internal sealed class ImageQueryRepository(IDbConnectionFactory factory)
        : IGetImageRepository,
            IGetImagesRepository,
            ISearchImagesRepository,
            IGetRemovedImagesRepository
    {
        private readonly IDbConnection _connection = factory.GetConnection();

        private const int numPerPage = 20;

        #region GetImage

        public Task<DetailedImageDto?> GetImageByAdminAsync(
            ImageId imageId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "i.id AS ImageId, "
                + "i.title AS Title, "
                + "i.uploaded_at AS UploadedAt, "
                + "i.is_removed as IsRemoved, "
                + "i.tags as Tags, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a "
                + "ON i.album_id = a.id "
                + "WHERE i.id = @imageId "
                + "AND NOT a.is_removed "
                + "LIMIT 1";
            return _connection.QueryFirstOrDefaultAsync<DetailedImageDto>(
                sql,
                new { imageId = imageId.Value }
            );
        }

        public Task<DetailedImageDto?> GetImageByAnonymousAsync(
            ImageId imageId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "i.id AS ImageId, "
                + "i.title AS Title, "
                + "i.uploaded_at AS UploadedAt, "
                + "i.is_removed as IsRemoved, "
                + "i.tags as Tags, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a "
                + "ON i.album_id = a.id "
                + "WHERE i.id = @imageId "
                + "AND NOT i.is_removed "
                + "AND NOT a.is_removed "
                + "AND a.accessibility = @PUBLIC "
                + "LIMIT 1";
            return _connection.QueryFirstOrDefaultAsync<DetailedImageDto>(
                sql,
                new { imageId = imageId.Value, PUBLIC = Accessibility.Public }
            );
        }

        public Task<DetailedImageDto?> GetImageByUserAsync(
            ImageId imageId,
            UserId requesterId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "i.id AS ImageId, "
                + "i.title AS Title, "
                + "i.uploaded_at AS UploadedAt, "
                + "i.is_removed as IsRemoved, "
                + "i.tags as Tags, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a "
                + "ON i.album_id = a.id "
                + "WHERE i.id = @imageId "
                + "AND NOT a.is_removed "
                + "AND ( "
                + " ( a.accessibility <> @PRIVATE AND NOT i.is_removed )"
                + " OR ( a.author_id = @requesterId )"
                + " OR ( @requesterId = ANY( a.collaborators ) AND NOT i.is_removed ) "
                + ") "
                + "LIMIT 1";
            return _connection.QueryFirstOrDefaultAsync<DetailedImageDto>(
                sql,
                new
                {
                    imageId = imageId.Value,
                    requesterId = requesterId.Value,
                    PRIVATE = Accessibility.Private
                }
            );
        }

        #endregion

        #region GetImages

        public Task<IEnumerable<AlbumImageDto>> GetImagesByAdminAsync(
            AlbumId albumId,
            int page,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "i.id AS ImageId, "
                + "i.title AS Title, "
                + "i.album_id AS AlbumId, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a "
                + "ON a.id = i.album_id "
                + "WHERE a.id = @albumId "
                + "AND NOT i.is_removed "
                + "AND NOT a.is_removed "
                + "ORDER BY i.uploaded_at DESC "
                + "LIMIT @take "
                + "OFFSET @skip";
            return _connection.QueryAsync<AlbumImageDto>(
                sql,
                new
                {
                    albumId = albumId.Value,
                    take = numPerPage,
                    skip = page * numPerPage
                }
            );
        }

        public Task<IEnumerable<AlbumImageDto>> GetImagesByAnonymousAsync(
            AlbumId albumId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "i.id AS ImageId, "
                + "i.title AS Title, "
                + "i.album_id AS AlbumId, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a "
                + "ON a.id = i.album_id "
                + "WHERE a.id = @albumId "
                + "AND NOT i.is_removed "
                + "AND NOT a.is_removed "
                + "AND a.accessibility = @PUBLIC "
                + "ORDER BY i.uploaded_at DESC "
                + "LIMIT @take";

            return _connection.QueryAsync<AlbumImageDto>(
                sql,
                new
                {
                    albumId = albumId.Value,
                    take = numPerPage,
                    PUBLIC = Accessibility.Public
                }
            );
        }

        public Task<IEnumerable<AlbumImageDto>> GetImagesByUserAsync(
            AlbumId albumId,
            int page,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "i.id AS ImageId, "
                + "i.title AS Title, "
                + "i.album_id AS AlbumId, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a ON a.id = i.album_id "
                + "WHERE a.id = @albumId "
                + "AND NOT i.is_removed "
                + "AND NOT a.is_removed "
                + "AND ( a.accessibility <> @PRIVATE OR a.author_id = @requesterId OR @requesterId = ANY( a.collaborators ) ) "
                + "ORDER BY i.uploaded_at DESC "
                + "LIMIT @take "
                + "OFFSET @skip";

            return _connection.QueryAsync<AlbumImageDto>(
                sql,
                new
                {
                    albumId = albumId.Value,
                    take = numPerPage,
                    skip = page * numPerPage,
                    PRIVATE = Accessibility.Private
                }
            );
        }

        #endregion

        #region SearchImages

        public Task<IEnumerable<SearchedImageDto>> SearchImagesByAdminAsync(
            int page,
            CategoryId categoryId,
            TagId[] tags,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "i.id AS ImageId, "
                + "i.title AS Title, "
                + "i.album_id AS AlbumId, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a ON a.id = i.album_id "
                + "WHERE a.id = @albumId "
                + "AND NOT i.is_removed "
                + "AND NOT a.is_removed "
                + "AND i.tags @> @tags "
                + "ORDER BY i.uploaded_at DESC "
                + "LIMIT @take OFFSET @skip";

            return _connection.QueryAsync<SearchedImageDto>(
                sql,
                new
                {
                    albumId = categoryId.Value,
                    tags = tags.Select(t => t.Value),
                    take = numPerPage,
                    skip = page * numPerPage
                }
            );
        }

        public Task<IEnumerable<SearchedImageDto>> SearchImagesByUserAsync(
            int page,
            CategoryId categoryId,
            TagId[] tags,
            UserId requesterId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "i.id AS ImageId, "
                + "i.title AS Title, "
                + "i.album_id AS AlbumId, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a ON a.id = i.album_id "
                + "WHERE a.id = @albumId "
                + "AND NOT i.is_removed "
                + "AND NOT a.is_removed "
                + "AND i.tags @> @tags "
                + "AND ( a.accessibility <> @PRIVATE OR a.author_id = @requesterId OR @requesterId = ANY( a.collaborators ) ) "
                + "ORDER BY i.uploaded_at DESC "
                + "LIMIT @take OFFSET @skip";

            return _connection.QueryAsync<SearchedImageDto>(
                sql,
                new
                {
                    albumId = categoryId.Value,
                    tags = tags.Select(t => t.Value),
                    requesterId = requesterId.Value,
                    take = numPerPage,
                    skip = page * numPerPage,
                    PRIVATE = Accessibility.Private
                }
            );
        }

        #endregion

        #region GetRemovedImages

        public Task<IEnumerable<AlbumImageDto>> GetImagesByUserAsync(
            UserId requesterId,
            AlbumId albumId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "i.id AS ImageId, "
                + "i.title AS Title, "
                + "i.album_id AS AlbumId, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a ON a.id = i.album_id "
                + "WHERE a.author_id = @authorId "
                + "AND a.id = @albumId"
                + "AND NOT a.is_removed "
                + "AND i.is_removed "
                + "ORDER BY a.updated_at DESC";

            return _connection.QueryAsync<AlbumImageDto>(
                sql,
                new { authorId = requesterId.Value, albumId = albumId.Value }
            );
        }

        public Task<IEnumerable<AlbumImageDto>> GetImagesByAdminAsync(
            AlbumId albumId,
            CancellationToken cancellationToken = default
        )
        {
            const string sql =
                "SELECT "
                + "i.id AS ImageId, "
                + "i.title AS Title, "
                + "i.album_id AS AlbumId, "
                + "i.url as Url "
                + "FROM images AS i "
                + "WHERE i.album_id = @albumId "
                + "AND i.is_removed "
                + "ORDER BY a.updated_at DESC";

            return _connection.QueryAsync<AlbumImageDto>(sql, new { albumId = albumId.Value });
        }

        #endregion
    }
}
