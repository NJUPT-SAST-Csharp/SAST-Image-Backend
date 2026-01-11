using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class UnlikeImageCommand(AlbumId Album, ImageId Image, Actor Actor)
    : ICommand { }

internal sealed class UnlikeCommandHandler(IAlbumRepository repository)
    : ICommandHandler<UnlikeImageCommand>
{
    public async ValueTask<Unit> Handle(
        UnlikeImageCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.UnlikeImage(request);

        return Unit.Value;
    }
}
