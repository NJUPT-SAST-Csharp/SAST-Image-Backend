﻿namespace SastImg.Application.AlbumServices.GetAlbums
{
    public interface IGetAlbumsRepository
    {
        /// <summary>
        /// Provide all albums accessible to anonymous users.
        /// </summary>
        /// <remarks>Used only in cache.</remarks>
        /// <param name="categoryId">Get <b>all albums</b> when <c>categoryId = 0</c></param>
        public Task<IEnumerable<AlbumDto>> GetAlbumsAnonymousAsync(
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumDto>> GetAlbumsByAdminAsync(
            int page,
            long authorId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumDto>> GetAlbumsByUserAsync(
            int page,
            long authorId,
            long requesterId,
            CancellationToken cancellationToken = default
        );
    }
}
