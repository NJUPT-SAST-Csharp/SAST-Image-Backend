using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class RemoveImageCommand(AlbumId Album, ImageId Image, Actor Actor)
    : ICommand { }

internal sealed class RemoveImageCommandHandler(IAlbumRepository repository)
    : ICommandHandler<RemoveImageCommand>
{
    public async ValueTask<Unit> Handle(
        RemoveImageCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.RemoveImage(request);

        return Unit.Value;
    }
}
