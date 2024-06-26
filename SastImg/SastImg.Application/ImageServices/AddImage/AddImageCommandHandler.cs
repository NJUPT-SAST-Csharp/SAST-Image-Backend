﻿using Exceptions.Exceptions;
using Primitives;
using Primitives.Command;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.ImageServices.AddImage
{
    internal sealed class AddImageCommandHandler(
        IAlbumRepository repository,
        IImageStorageRepository client,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<AddImageCommand, ImageInfoDto>
    {
        private readonly IAlbumRepository _repository = repository;
        private readonly IImageStorageRepository _client = client;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ImageInfoDto> Handle(
            AddImageCommand request,
            CancellationToken cancellationToken
        )
        {
            var album = await _repository.GetAlbumAsync(request.AlbumId, cancellationToken);

            if (
                request.Requester.IsAdmin == false
                && album.IsManagedBy(request.Requester.Id) == false
            )
            {
                throw new NoPermissionException();
            }

            var url = await _client.UploadImageAsync(request.ImageFile, cancellationToken);

            var imageId = album.AddImage(request.Title, request.Description, url, request.Tags);

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return new ImageInfoDto(imageId);
        }
    }
}
