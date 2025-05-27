using Exceptions.Exceptions;
using Mediator;
using Primitives;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.AlbumServices.UpdateCollaborators;

public sealed class UpdateCollaboratorsCommandHandler(
    IAlbumRepository repository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateCollaboratorsCommand>
{
    private readonly IAlbumRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async ValueTask<Unit> Handle(
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
        else
        {
            throw new NoPermissionException();
        }

        return Unit.Value;
    }
}
