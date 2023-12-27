namespace SastImg.Application.AlbumServices.GetAlbum
{
    public interface IGetAlbumRepository
    {
        Task<DetailedAlbumDto?> GetDetailedAlbumAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );
    }
}
