using System.Data;
using Dapper;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.ImageServices.GetRemovedImages;
using SastImg.Application.ImageServices.SearchImages;
using SastImg.Domain.AlbumAggregate;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.Domain.ImageEntity
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
            long imageId,
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
                + "i.url as Url, "
                + "i.views as Views, "
                + "i.likes as Likes "
                + "FROM images AS i "
                + "INNER JOIN albums AS a "
                + "ON i.album_id = a.id "
                + "WHERE i.id = @imageId "
                + "AND NOT a.is_removed "
                + "LIMIT 1";
            return _connection.QueryFirstOrDefaultAsync<DetailedImageDto>(sql, new { imageId });
        }

        public Task<DetailedImageDto?> GetImageByAnonymousAsync(
            long imageId,
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
                + "i.url as Url, "
                + "i.views as Views, "
                + "i.likes as Likes "
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
                new { imageId, PUBLIC = Accessibility.Public }
            );
        }

        public Task<DetailedImageDto?> GetImageByUserAsync(
            long imageId,
            long requesterId,
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
                + "i.url as Url, "
                + "i.views as Views, "
                + "i.likes as Likes "
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
                    imageId,
                    requesterId,
                    PRIVATE = Accessibility.Private
                }
            );
        }

        #endregion

        #region GetImages

        public Task<IEnumerable<ImageDto>> GetImagesByAdminAsync(
            long albumId,
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
            return _connection.QueryAsync<ImageDto>(
                sql,
                new
                {
                    albumId,
                    take = numPerPage,
                    skip = page * numPerPage
                }
            );
        }

        public Task<IEnumerable<ImageDto>> GetImagesByAnonymousAsync(
            long albumId,
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

            return _connection.QueryAsync<ImageDto>(
                sql,
                new
                {
                    albumId,
                    take = numPerPage,
                    PRIVATE = Accessibility.Public
                }
            );
        }

        public Task<IEnumerable<ImageDto>> GetImagesByUserAsync(
            long albumId,
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

            return _connection.QueryAsync<ImageDto>(
                sql,
                new
                {
                    albumId,
                    take = numPerPage,
                    skip = page * numPerPage,
                    PRIVATE = Accessibility.Private
                }
            );
        }

        #endregion

        #region SearchImages

        public Task<IEnumerable<ImageDto>> SearchImagesByAdminAsync(
            int page,
            long categoryId,
            SearchOrder order,
            long[] tags,
            CancellationToken cancellationToken = default
        )
        {
            const string prefix =
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
                + "AND i.tags @> @tags ";

            const string suffix = "LIMIT @take OFFSET @skip";

            string orderStr = order switch
            {
                SearchOrder.ByDate => "ORDERBY i.uploaded_at DESC ",
                SearchOrder.ByLikes => "ORDERBY i.likes DESC ",
                SearchOrder.ByViews => "ORDERBY i.views DESC ",
                _ => throw new ArgumentOutOfRangeException(nameof(order))
            };

            string sql = prefix + orderStr + suffix;

            return _connection.QueryAsync<ImageDto>(
                sql,
                new
                {
                    albumId = categoryId,
                    tags,
                    take = numPerPage,
                    skip = page * numPerPage
                }
            );
        }

        public Task<IEnumerable<ImageDto>> SearchImagesByUserAsync(
            int page,
            long categoryId,
            SearchOrder order,
            long[] tags,
            long requesterId,
            CancellationToken cancellationToken = default
        )
        {
            const string prefix =
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
                + "AND ( a.accessibility <> @PRIVATE OR a.author_id = @requesterId OR @requesterId = ANY( a.collaborators ) ) ";

            const string suffix = "LIMIT @take OFFSET @skip";

            string orderStr = order switch
            {
                SearchOrder.ByDate => "ORDERBY i.uploaded_at DESC ",
                SearchOrder.ByLikes => "ORDERBY i.likes DESC ",
                SearchOrder.ByViews => "ORDERBY i.views DESC ",
                _ => throw new ArgumentOutOfRangeException(nameof(order))
            };

            string sql = prefix + orderStr + suffix;

            return _connection.QueryAsync<ImageDto>(
                sql,
                new
                {
                    albumId = categoryId,
                    tags,
                    requesterId,
                    take = numPerPage,
                    skip = page * numPerPage,
                    PRIVATE = Accessibility.Private
                }
            );
        }

        #endregion

        #region GetRemovedImages

        public Task<IEnumerable<ImageDto>> GetImagesByUserAsync(
            long requesterId,
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
                + "AND NOT a.is_removed "
                + "AND i.is_removed "
                + "ORDER BY a.updated_at DESC";

            return _connection.QueryAsync<ImageDto>(sql, new { authorId = requesterId });
        }

        public Task<IEnumerable<ImageDto>> GetImagesByAdminAsync(
            long authorId,
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
                + "AND NOT a.is_removed "
                + "AND i.is_removed "
                + "ORDER BY a.updated_at DESC";

            return _connection.QueryAsync<ImageDto>(sql, new { authorId });
        }

        #endregion
    }
}
