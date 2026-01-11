using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class SubscribeCommand(AlbumId Album, Actor Actor) : ICommand { }

internal sealed class SubscribeCommandHandler(IAlbumRepository repository)
    : ICommandHandler<SubscribeCommand>
{
    public async ValueTask<Unit> Handle(
        SubscribeCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.Subscribe(request);

        return Unit.Value;
    }
}
