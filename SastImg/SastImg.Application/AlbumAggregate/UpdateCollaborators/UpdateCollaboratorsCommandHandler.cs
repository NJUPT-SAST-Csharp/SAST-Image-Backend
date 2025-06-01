using Mediator;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.AlbumAggregate.UpdateCollaborators;

public sealed class UpdateCollaboratorsCommandHandler(IAlbumRepository repository)
    : ICommandHandler<UpdateCollaboratorsCommand>
{
    private readonly IAlbumRepository _repository = repository;

    public async ValueTask<Unit> Handle(
        UpdateCollaboratorsCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await _repository.GetAlbumAsync(request.AlbumId, cancellationToken);

        album.UpdateCollaborators(request.Collaborators);

        return Unit.Value;
    }
}
