﻿namespace SastImg.Application.AlbumServices.GetAlbums
{
    public interface IGetAlbumsCache
    {
        public Task<IEnumerable<AlbumDto>> GetAlbumsAsync(
            int page,
            long categoryId,
            CancellationToken cancellationToken = default
        );

        public Task RemoveAlbumsAsync(long authorId, CancellationToken cancellationToken = default);
    }
}