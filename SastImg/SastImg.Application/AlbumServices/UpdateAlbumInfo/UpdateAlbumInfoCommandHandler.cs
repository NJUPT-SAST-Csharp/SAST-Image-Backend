﻿using Primitives.Command;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.AlbumServices.UpdateAlbumInfo
{
    public sealed class UpdateAlbumInfoCommandHandler(
        IAlbumRepository respository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateAlbumInfoCommand>
    {
        private readonly IAlbumRepository _repository = respository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(
            UpdateAlbumInfoCommand request,
            CancellationToken cancellationToken
        )
        {
            var album = await _repository.GetAlbumAsync(request.AlbumId, cancellationToken);

            album.UpdateAlbumInfo(
                request.Title,
                request.Description,
                request.CategoryId,
                request.Accessibility
            );

            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
