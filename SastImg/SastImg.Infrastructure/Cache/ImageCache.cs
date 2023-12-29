using SastImg.Application.ImageServices.GetImages;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Cache
{
    internal sealed class ImageCache(IConnectionMultiplexer connection, IGetImagesRepository images)
        : IGetImagesCache
    {
        private readonly IDatabase _database = connection.GetDatabase();
        private readonly IGetImagesRepository _images = images;

        public Task<IEnumerable<ImageDto>> GetImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        ) { }

        public Task<IEnumerable<ImageDto>> RemoveImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }
    }
}
