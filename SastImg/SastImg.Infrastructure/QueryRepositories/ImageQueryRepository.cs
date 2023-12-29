using System.Data;
using Dapper;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.ImageServices.SearchImages;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.QueryRepositories
{
    internal sealed class ImageQueryRepository(IDbConnectionFactory factory)
        : IGetImageRepository,
            IGetImagesRepository,
            ISearchImagesRepository
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
                + "i.is_nsfw AS IsNsfw, "
                + "i.uploaded_at AS UploadedAt, "
                + "i.tags as Tags, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a "
                + "ON i.album_id = a.id "
                + "WHERE i.id = @imageId "
                + "AND NOT i.is_removed "
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
                + "i.is_nsfw AS IsNsfw, "
                + "i.uploaded_at AS UploadedAt, "
                + "i.tags as Tags, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a "
                + "ON i.album_id = a.id "
                + "WHERE i.id = @imageId "
                + "AND NOT i.is_removed "
                + "AND NOT a.is_removed "
                + "AND a.accessibility = 0 "
                + "LIMIT 1";
            return _connection.QueryFirstOrDefaultAsync<DetailedImageDto>(sql, new { imageId });
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
                + "i.is_nsfw AS IsNsfw, "
                + "i.uploaded_at AS UploadedAt, "
                + "i.tags as Tags, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a "
                + "ON i.album_id = a.id "
                + "WHERE i.id = @imageId "
                + "AND NOT i.is_removed "
                + "AND NOT a.is_removed "
                + "AND ( a.accessibility = 0 OR a.author_id = @requesterId OR @requesterId = ANY( a.collaborators ) ) "
                + "LIMIT 1";
            return _connection.QueryFirstOrDefaultAsync<DetailedImageDto>(
                sql,
                new { imageId, requesterId }
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
                + "i.is_nsfw AS IsNsfw, "
                + "i.uploaded_at AS UploadedAt, "
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
                + "i.is_nsfw AS IsNsfw, "
                + "i.uploaded_at AS UploadedAt, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a "
                + "ON a.id = i.album_id "
                + "WHERE a.id = @albumId "
                + "AND NOT i.is_removed "
                + "AND NOT a.is_removed "
                + "AND a.accessibility = 0 "
                + "ORDER BY i.uploaded_at DESC"
                + "LIMIT @take";

            return _connection.QueryAsync<ImageDto>(sql, new { albumId, take = numPerPage });
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
                + "i.is_nsfw AS IsNsfw, "
                + "i.uploaded_at AS UploadedAt, "
                + "i.url as Url "
                + "FROM images AS i "
                + "INNER JOIN albums AS a "
                + "ON a.id = i.album_id "
                + "WHERE a.id = @albumId "
                + "AND NOT i.is_removed "
                + "AND NOT a.is_removed "
                + "AND ( a.accessibility = 0 OR a.author_id = @requesterId OR @requesterId = ANY( a.collaborators ) ) "
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

        #endregion

        #region SearchImages

        public Task<IEnumerable<ImageDto>> SearchImagesByAdminAsync(
            int page,
            SearchOrder order,
            long categoryId,
            long[] tags,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ImageDto>> SearchImagesByUserAsync(
            int page,
            SearchOrder order,
            long categoryId,
            long[] tags,
            long requesterId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
