using Exceptions.Exceptions;
using Primitives.Command;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.AlbumServices.UpdateCollaborators
{
    internal sealed class UpdateCollaboratorsCommandHandler(
        IAlbumRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<UpdateCollaboratorsCommand>
    {
        private readonly IAlbumRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(
            UpdateCollaboratorsCommand request,
            CancellationToken cancellationToken
        )
        {
            var album = await _repository.GetAlbumAsync(request.AlbumId, cancellationToken);

            if (request.Requester.IsAdmin || album.IsOwnedBy(request.Requester.Id))
            {
                album.UpdateCollaborators(request.Collaborators);
                await _unitOfWork.CommitChangesAsync(cancellationToken);
            }

            throw new NoPermissionException();
        }
    }
}
