using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class RestoreImageCommand(AlbumId Album, ImageId Image, Actor Actor)
    : ICommand;

internal sealed class RestoreImageCommandHandler(IAlbumRepository repository)
    : ICommandHandler<RestoreImageCommand>
{
    public async ValueTask<Unit> Handle(
        RestoreImageCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.RestoreImage(request);

        return Unit.Value;
    }
}
