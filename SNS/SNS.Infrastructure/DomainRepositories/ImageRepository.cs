using SNS.Domain.ImageAggregate.ImageEntity;
using SNS.Infrastructure.Persistence;

namespace SNS.Infrastructure.DomainRepositories
{
    public sealed class ImageRepository(SNSDbContext context) : IImageRepository
    {
        private readonly SNSDbContext _context = context;

        public async Task<ImageId> AddNewImageAsync(
            Image image,
            CancellationToken cancellationToken = default
        )
        {
            var i = await _context.Images.AddAsync(image, cancellationToken);
            return i.Entity.Id;
        }
    }
}
