namespace SastImg.Application.AlbumServices.GetAlbum
{
    public interface IGetAlbumCache
    {
        public Task<DetailedAlbumDto?> GetAlbumAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );
        public Task RemoveAlbumAsync(long albumId, CancellationToken cancellationToken = default);
    }
}
