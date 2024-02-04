namespace SNS.Domain.AlbumEntity
{
    public interface IAlbumRepository
    {
        public Task<AlbumId> AddNewAlbumAsync(
            Album album,
            CancellationToken cancellationToken = default
        );
    }
}
