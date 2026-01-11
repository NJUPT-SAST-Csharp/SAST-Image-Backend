using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class UpdateImageTagsCommand(
    AlbumId AlbumId,
    ImageId ImageId,
    ImageTags Tags,
    Actor Actor
) : ICommand;

internal sealed class UpdateImageTagsCommandHandler(IAlbumRepository repository)
    : ICommandHandler<UpdateImageTagsCommand>
{
    public async ValueTask<Unit> Handle(
        UpdateImageTagsCommand command,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(command.AlbumId, cancellationToken);

        album.UpdateImageTags(command);

        return Unit.Value;
    }
}
