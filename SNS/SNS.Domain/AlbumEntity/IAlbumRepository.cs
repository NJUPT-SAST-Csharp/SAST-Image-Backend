namespace SNS.Domain.AlbumEntity
{
    public interface IAlbumRepository
    {
        public Task<Album> AddNewAlbumAsync(
            Album album,
            CancellationToken cancellationToken = default
        );
        public Task<Album> GetAlbumByIdAsync(
            AlbumId albumId,
            CancellationToken cancellationToken = default
        );
    }
}
