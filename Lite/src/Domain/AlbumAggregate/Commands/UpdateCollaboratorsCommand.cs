using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Services;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class UpdateCollaboratorsCommand(
    AlbumId Album,
    Collaborators Collaborators,
    Actor Actor
) : ICommand { }

internal sealed class UpdateCollaboratorsCommandHandler(
    IAlbumRepository repository,
    ICollaboratorsExistenceChecker checker
) : ICommandHandler<UpdateCollaboratorsCommand>
{
    private readonly ICollaboratorsExistenceChecker _checker = checker;

    public async ValueTask<Unit> Handle(
        UpdateCollaboratorsCommand request,
        CancellationToken cancellationToken
    )
    {
        await _checker.CheckAsync(request.Collaborators, cancellationToken);

        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.UpdateCollaborators(request);

        return Unit.Value;
    }
}
