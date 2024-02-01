using Exceptions.Exceptions;
using Primitives.Command;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.AlbumServices.RemoveAlbum
{
    internal sealed class RemoveAlbumCommandHandler(
        IAlbumRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<RemoveAlbumCommand>
    {
        private readonly IAlbumRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(RemoveAlbumCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAlbumAsync(request.AlbumId, cancellationToken);

            if (request.RequesterInfo.IsAdmin || album.IsOwnedBy(request.RequesterInfo.Id))
            {
                album.Remove();
                await _unitOfWork.CommitChangesAsync(cancellationToken);
            }
            else
            {
                throw new NoPermissionException();
            }
        }
    }
}
