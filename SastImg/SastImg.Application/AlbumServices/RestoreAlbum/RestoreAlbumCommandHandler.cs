using Exceptions.Exceptions;
using Primitives;
using Primitives.Command;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.AlbumServices.RestoreAlbum
{
    internal sealed class RestoreAlbumCommandHandler(
        IAlbumRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<RestoreAlbumCommand>
    {
        private readonly IAlbumRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(RestoreAlbumCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAlbumAsync(request.AlbumId, cancellationToken);
            if (request.Requester.IsAdmin || album.IsOwnedBy(request.Requester.Id))
            {
                album.Restore();
                await _unitOfWork.CommitChangesAsync(cancellationToken);
            }
            else
            {
                throw new NoPermissionException();
            }
        }
    }
}
