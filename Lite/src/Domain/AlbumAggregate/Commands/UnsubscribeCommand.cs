using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class UnsubscribeCommand(AlbumId Album, Actor Actor) : ICommand { }

internal sealed class UnsubscribeCommandHandler(IAlbumRepository repository)
    : ICommandHandler<UnsubscribeCommand>
{
    public async ValueTask<Unit> Handle(
        UnsubscribeCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.Unsubscribe(request);

        return Unit.Value;
    }
}
