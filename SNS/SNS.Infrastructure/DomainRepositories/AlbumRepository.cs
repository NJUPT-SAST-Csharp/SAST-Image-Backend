using SNS.Domain.AlbumEntity;
using SNS.Infrastructure.Persistence;

namespace SNS.Infrastructure.DomainRepositories
{
    public sealed class AlbumRepository(SNSDbContext context) : IAlbumRepository
    {
        private readonly SNSDbContext _context = context;

        public async Task<AlbumId> AddNewAlbumAsync(
            Album album,
            CancellationToken cancellationToken = default
        )
        {
            var a = await _context.Albums.AddAsync(album, cancellationToken);
            return a.Entity.Id;
        }
    }
}
