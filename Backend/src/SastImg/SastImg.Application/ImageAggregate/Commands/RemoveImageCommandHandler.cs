using Mediator;
using SastImg.Domain.AlbumAggregate;
using SastImg.Domain.AlbumAggregate.ImageEntity.Commands;

namespace SastImg.Application.ImageAggregate.Commands;

public sealed class RemoveImageCommandHandler(IAlbumRepository repository)
    : ICommandHandler<RemoveImageCommand>
{
    public async ValueTask<Unit> Handle(
        RemoveImageCommand command,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAlbumAsync(command.AlbumId, cancellationToken);

        album.RemoveImage(command);

        return Unit.Value;
    }
}
