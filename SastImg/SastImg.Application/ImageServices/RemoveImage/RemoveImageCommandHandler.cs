using Exceptions.Exceptions;
using Primitives;
using Primitives.Command;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.ImageServices.RemoveImage
{
    internal sealed class RemoveImageCommandHandler(
        IAlbumRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<RemoveImageCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAlbumRepository _repository = repository;

        public async Task Handle(RemoveImageCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAlbumAsync(request.AlbumId, cancellationToken);

            if (request.Requester.IsAdmin || album.IsManagedBy(request.Requester.Id))
            {
                album.RemoveImage(request.ImageId);
                await _unitOfWork.CommitChangesAsync(cancellationToken);
            }
            else
            {
                throw new NoPermissionException();
            }
        }
    }
}
