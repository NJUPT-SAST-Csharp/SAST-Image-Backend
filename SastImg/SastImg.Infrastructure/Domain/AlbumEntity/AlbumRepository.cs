using Microsoft.EntityFrameworkCore;
using SastImg.Domain.AlbumAggregate;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Infrastructure.Persistence;

namespace SastImg.Infrastructure.Domain.AlbumEntity
{
    internal class AlbumRepository(SastImgDbContext context) : IAlbumRepository
    {
        private readonly SastImgDbContext _context = context;

        public async Task<long> AddAlbumAsync(
            Album album,
            CancellationToken cancellationToken = default
        )
        {
            var a = await _context.Albums.AddAsync(album, cancellationToken);
            return a.Entity.Id;
        }

        public Task<Album> GetAlbumAsync(long id, CancellationToken cancellationToken = default)
        {
            var album = _context.Albums.SingleAsync(a => a.Id == id, cancellationToken);
            return album;
        }
    }
}
