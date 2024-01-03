using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Domain.AlbumAggregate
{
    public interface IAlbumRepository
    {
        public Task<Album> GetAlbumAsync(long id, CancellationToken cancellationToken = default);

        public Task<long> AddAlbumAsync(Album album, CancellationToken cancellationToken = default);
    }
}
