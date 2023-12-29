namespace SastImg.Application.AlbumServices.GetAlbum
{
    public interface IGetAlbumRepository
    {
        Task<DetailedAlbumDto?> GetDetailedAlbumByUserAsync(
            long albumId,
            long requesterId,
            CancellationToken cancellationToken = default
        );

        Task<DetailedAlbumDto?> GetDetailedAlbumByAdminAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Provide album accessible to anonymous users.
        /// </summary>
        /// <remarks>Used only in cache.</remarks>
        Task<DetailedAlbumDto?> GetDetailedAlbumByAnonymousAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );
    }
}
