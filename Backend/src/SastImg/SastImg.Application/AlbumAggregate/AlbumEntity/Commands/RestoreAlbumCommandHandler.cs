using Mediator;
using SastImg.Domain.AlbumAggregate;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Commands;

namespace SastImg.Application.AlbumAggregate.AlbumEntity.Commands;

public sealed class RestoreAlbumCommandHandler(IAlbumRepository repository)
    : ICommandHandler<RestoreAlbumCommand>
{
    public async ValueTask<Unit> Handle(
        RestoreAlbumCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAlbumAsync(request.AlbumId, cancellationToken);

        album.Restore(request);

        return Unit.Value;
    }
}
