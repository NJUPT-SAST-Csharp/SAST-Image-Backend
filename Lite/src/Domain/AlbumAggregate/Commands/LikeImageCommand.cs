using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class LikeImageCommand(AlbumId Album, ImageId Image, Actor Actor)
    : ICommand { }

internal sealed class LikeCommandHandler(IAlbumRepository repository)
    : ICommandHandler<LikeImageCommand>
{
    public async ValueTask<Unit> Handle(
        LikeImageCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        album.LikeImage(request);

        return Unit.Value;
    }
}
