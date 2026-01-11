using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class UpdateAccessLevelCommand(
    AlbumId Album,
    AccessLevel AccessLevel,
    Actor Actor
) : ICommand { }

internal sealed class UpdateAccessLevelCommandHandler(IAlbumRepository repository)
    : ICommandHandler<UpdateAccessLevelCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateAccessLevelCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.UpdateAccessLevel(request);

        return Unit.Value;
    }
}
