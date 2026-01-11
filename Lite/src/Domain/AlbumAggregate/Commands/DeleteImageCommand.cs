using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public record class DeleteImageCommand(AlbumId Album, ImageId Image, Actor Actor) : ICommand;

internal sealed class DeleteImageCommandHandler(IAlbumRepository repository)
    : ICommandHandler<DeleteImageCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteImageCommand command,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(command.Album, cancellationToken);

        album.DeleteImage(command);

        return Unit.Value;
    }
}
