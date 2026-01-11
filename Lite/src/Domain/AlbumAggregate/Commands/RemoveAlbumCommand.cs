using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class RemoveAlbumCommand(AlbumId Album, Actor Actor) : ICommand { }

internal sealed class RemoveAlbumCommandHandler(IAlbumRepository repository)
    : ICommandHandler<RemoveAlbumCommand>
{
    public async ValueTask<Unit> Handle(
        RemoveAlbumCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.Remove(request);

        return Unit.Value;
    }
}
