using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class RestoreAlbumCommand(AlbumId Album, Actor Actor) : ICommand { }

internal sealed class RestoreAlbumCommandHandler(IAlbumRepository repository)
    : ICommandHandler<RestoreAlbumCommand>
{
    public async ValueTask<Unit> Handle(
        RestoreAlbumCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.Restore(request);

        return Unit.Value;
    }
}
