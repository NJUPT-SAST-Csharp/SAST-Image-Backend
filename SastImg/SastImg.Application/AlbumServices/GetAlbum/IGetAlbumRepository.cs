namespace SastImg.Application.AlbumServices.GetAlbum
{
    public interface IGetAlbumRepository
    {
        Task<DetailedAlbumDto?> GetAlbumByUserAsync(
            long albumId,
            long requesterId,
            CancellationToken cancellationToken = default
        );

        Task<DetailedAlbumDto?> GetAlbumByAdminAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Provide album accessible to anonymous users.
        /// </summary>
        /// <remarks>Used only in cache.</remarks>
        Task<DetailedAlbumDto?> GetAlbumByAnonymousAsync(
            string albumId,
            CancellationToken cancellationToken = default
        );
    }
}
