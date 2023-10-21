namespace SastImg.Domain.Repositories
{
    public interface IAlbumRepository
    {
        public Task<Entities.Album> GetAlbumByIdAsync(
            long id,
            CancellationToken cancellationToken = default
        );

        public Task RemoveAlbumByIdAsync(long id, CancellationToken cancellationToken = default);

        public Task<int> DeleteAllMarkedAlbums(CancellationToken cancellationToken = default);

        public Task AddAlbumAsync(
            Entities.Album album,
            CancellationToken cancellationToken = default
        );

        public Task UpdateAlbumInfo(
            long id,
            string name,
            string description,
            CancellationToken cancellationToken = default
        );
    }
}
