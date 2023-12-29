using System.Data;
using SastImg.Application.ImageServices.GetImage;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Infrastructure.Persistence.QueryDatabase;

namespace SastImg.Infrastructure.QueryRepositories
{
    internal sealed class ImageQueryRepository(IDbConnectionFactory factory)
        : IGetImageRepository,
            IGetImagesRepository
    {
        private readonly IDbConnection _connection = factory.GetConnection();

        #region GetImage

        public Task<DetailedImageDto?> GetImageByAdminAsync(
            long imageId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<DetailedImageDto?> GetImageByAnonymousAsync(
            string imageId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<DetailedImageDto?> GetImageByUserAsync(
            long imageId,
            long requesterId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GetImages

        public Task<IEnumerable<ImageDto>> GetImagesByAdminAsync(
            long albumId,
            int page,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ImageDto>> GetImagesByAnonymousAsync(
            string albumId,
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ImageDto>> GetImagesByUserAsync(
            long albumId,
            int page,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
