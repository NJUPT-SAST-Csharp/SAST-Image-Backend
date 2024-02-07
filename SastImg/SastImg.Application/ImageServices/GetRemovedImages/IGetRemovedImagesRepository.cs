﻿using SastImg.Application.ImageServices.GetImages;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.ImageServices.GetRemovedImages
{
    public interface IGetRemovedImagesRepository
    {
        public Task<IEnumerable<AlbumImageDto>> GetImagesByUserAsync(
            UserId requesterId,
            AlbumId albumId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumImageDto>> GetImagesByAdminAsync(
            AlbumId authorId,
            CancellationToken cancellationToken = default
        );
    }
}
